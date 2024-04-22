using FirstStepsAspnet.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FirstStepsAspnet.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly TodoContext _todoContext;
    private readonly IConfiguration _configuration;
    private readonly IPasswordHasher<object> _encoder;

    public AuthController(TodoContext todoContext, IConfiguration configuration, IPasswordHasher<object> encoder)
    {
      _todoContext = todoContext;
      _configuration = configuration;
      _encoder = encoder;
    }

    [HttpPost("Register")]
    public async Task<ActionResult<User>> Register(User user)
    {
      user.Password = _encoder.HashPassword(user, user.Password);
      var userCreated = _todoContext.Users.Add(user);
      await _todoContext.SaveChangesAsync();

      return Created("http://localhost/api/profile", userCreated.Entity);
    }

    [HttpPost("Login")]
    public async Task<ActionResult<string>> Login(User user)
    {
      var userAuth = await _todoContext.Users
          .Where(u =>
              u.Username == user.Username
          )
          .FirstOrDefaultAsync();

      if (userAuth == null)
        return Unauthorized();


      var result = _encoder.VerifyHashedPassword(user, userAuth.Password, user.Password);

      if (result == PasswordVerificationResult.Failed)
        return Unauthorized();

      var token = GenerateToken(userAuth);

      return Ok(new { Token = token });
    }

    private string GenerateToken(User user)
    {
      var key = _configuration["JwtSettings:Key"]!;
      var bytesKey = Encoding.UTF8.GetBytes(key);
      var securityKey = new SymmetricSecurityKey(bytesKey);
      var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
      var userClaims = new[]
      {
          new Claim("id", user.Id.ToString()),
          new Claim("user", user.Username),
      };
      var userToken = new JwtSecurityToken(
        issuer: _configuration["JwtSettings:Issuer"],
        audience: _configuration["JwtSettings:Audience"],
        claims: userClaims,
        expires: DateTime.UtcNow.AddDays(7),
        signingCredentials: credentials
      );

      var tokenHandler = new JwtSecurityTokenHandler();
      return tokenHandler.WriteToken(userToken);
    }
  }
}

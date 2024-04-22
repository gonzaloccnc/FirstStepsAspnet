using FirstStepsAspnet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstStepsAspnet.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  [Authorize]
  public class ProfileController : ControllerBase
  {
    private readonly TodoContext _todoContext;

    public ProfileController(TodoContext todoContext)
    {
      _todoContext = todoContext;
    }

    [HttpGet]
    public async Task<ActionResult<User>> GetProfile()
    {
      var userId = User.FindFirst("Id")?.Value!;
      var userProfile = await _todoContext.Users.FindAsync(long.Parse(userId));

      if (userProfile == null)
        return NotFound();

      return Ok(userProfile);
    }
  }
}

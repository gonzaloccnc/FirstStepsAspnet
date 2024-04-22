using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace FirstStepsAspnet.Infrastructure
{
  public class GloblaExceptionHandler : IExceptionHandler
  {

    private readonly ILogger<GloblaExceptionHandler> _logger;

    public GloblaExceptionHandler(ILogger<GloblaExceptionHandler> logger)
    {
      _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
      HttpContext httpContext,
      Exception exception,
      CancellationToken cancellationToken
      )
    {
      _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);
      var problemDetails = new ProblemDetails
      {
        Status = StatusCodes.Status500InternalServerError,
        Title = exception.Source,
      };

      httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
      await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
      return true;
    }
  }
}

using Core.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Framework;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
[Route(Route)]
public class ExceptionHandlerController : ControllerBase
{
    public const string Route = "api/error";

    private readonly ILogger<ExceptionHandlerController> _logger;

    public ExceptionHandlerController(ILogger<ExceptionHandlerController> logger)
    {
        _logger = logger;
    }

    // DO NOT ADD HTTP VERB ATTRIBUTE
    // Attribute is determined by redirect internally within dotnet
    // Setting this as a get means that post requests will never arrive here.
    public IActionResult HandleError()
    {
        var exceptionHandlerFeature =
            HttpContext.Features.Get<IExceptionHandlerFeature>()!;
        var exception = exceptionHandlerFeature.Error;
        if (exception is DomainException domainException)
        {
            _logger.LogError(exception, exception.Message);
            var problemDetails = new ProblemDetails
            {
                Title = domainException.Title,
                Detail = domainException.Detail,
                Status = domainException.StatusCode,
                Type = $"https://httpstatuses.com/{domainException.StatusCode}",
                Instance = exceptionHandlerFeature.Path
            };
            foreach (var kv in domainException.Extensions)
            {
                problemDetails.Extensions.Add(kv.Key, kv.Value);
            }
            return new ObjectResult(problemDetails)
            {
                StatusCode = problemDetails.Status
            };
        }

        _logger.LogError(exception, "Unhandled Exception");
        return Problem();
    }
}
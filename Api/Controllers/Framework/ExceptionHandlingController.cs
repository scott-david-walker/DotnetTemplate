using Core.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Framework;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
[Route(Route)]
public class ExceptionHandlerController(ILogger<ExceptionHandlerController> logger) : ControllerBase
{
    public const string Route = "api/error";
    
    // DO NOT ADD HTTP VERB ATTRIBUTE
    // Attribute is determined by redirect internally within dotnet
    // Setting this as a get means that post requests will never arrive here.
    public IActionResult HandleError()
    {
        var exceptionHandlerFeature =
            HttpContext.Features.Get<IExceptionHandlerFeature>()!;
        var exception = exceptionHandlerFeature.Error;
        var handler = ExceptionHandlerFactory.CreateExceptionHandler(exception);
        if (handler != null)
        {
            return handler.HandleError(exceptionHandlerFeature);
        }
        logger.LogError(exception, "Unhandled Exception");
        return Problem();
    }
}

file static class ExceptionHandlerFactory
{
    public static IExceptionHandler? CreateExceptionHandler(Exception exception)
    {
        return exception switch
        {
            DomainException domainException => new DomainExceptionHandler(domainException),
            UnauthorizedAccessException => new UnauthorizedAccessExceptionHandler(),
            _ => null
        };
    }
}
file interface IExceptionHandler
{
    IActionResult HandleError(IExceptionHandlerFeature exceptionHandlerFeature);
}

file class UnauthorizedAccessExceptionHandler : IExceptionHandler
{
    public IActionResult HandleError(IExceptionHandlerFeature _)
    {
        return new ForbidResult();
    }
}

file class DomainExceptionHandler(DomainException exception) : IExceptionHandler
{
    public IActionResult HandleError(IExceptionHandlerFeature exceptionHandlerFeature)
    {
        var problemDetails = new ProblemDetails
        {
            Title = exception.Title,
            Detail = exception.Detail,
            Status = exception.StatusCode,
            Type = $"https://httpstatuses.com/{exception.StatusCode}",
            Instance = exceptionHandlerFeature.Path
        };
        foreach (var kv in exception.Extensions)
        {
            problemDetails.Extensions.Add(kv.Key, kv.Value);
        }
        return new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails.Status
        };
    }
}
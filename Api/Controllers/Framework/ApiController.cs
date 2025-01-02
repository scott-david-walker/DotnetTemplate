using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Framework;

[ApiController]
public abstract class ApiController : ControllerBase
{
    private ISender? _mediator;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}
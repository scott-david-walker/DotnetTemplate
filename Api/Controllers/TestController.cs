using Api.Controllers.Framework;
using Core;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController(ICurrentUser currentUser) : AuthController
{
    [HttpGet(Name = "Test")]
    public async Task<IActionResult> Get()
    {
        var result = await Mediator.Send(new TestMediatrCommand(await currentUser.Id()));
        return Ok(result);
    }
}

public record TestMediatrCommand(string Title) : IRequest<Guid>;

// ReSharper disable once UnusedType.Global
public class TestMediatrValidator : AbstractValidator<TestMediatrCommand>
{
    public TestMediatrValidator()
    {
        RuleFor(v => v.Title)
            .NotEmpty();
    }
}

// ReSharper disable once UnusedType.Global
public class TestMediatrHandler
    : IRequestHandler<TestMediatrCommand, Guid>
{
    public async Task<Guid> Handle(TestMediatrCommand request,
        CancellationToken cancellationToken)
    {
        return await Task.FromResult(Guid.NewGuid());
    }
}

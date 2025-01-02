using Api.Behaviours;
using Api.Controllers.Framework;
using Api.Framework;
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
        var result = await Mediator.Send(new TestMediatrCommand(currentUser.Email));
        return Ok(result);
    }
}

public record TestMediatrCommand(string Title) : IRequest<Guid>, ICommand;

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
public class TestMediatrAuthoriser : IAuthoriser<TestMediatrCommand>
{
    public Task<bool> Authorise(TestMediatrCommand request)
    {
        return Task.FromResult(true);
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

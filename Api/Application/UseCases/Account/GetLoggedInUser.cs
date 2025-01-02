using Core;
using Core.Entities;
using Core.Extensions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Api.Application.UseCases.Account;

public record GetLoggedInUserQuery : IRequest<GetLoggedInUserResponseModel>;
public record GetLoggedInUserResponseModel(string Id, string Email);

// ReSharper disable once UnusedType.Global
public class GetLoggedInUserHandler(UserManager<User> userManager, ICurrentUser currentUser)
    : IRequestHandler<GetLoggedInUserQuery, GetLoggedInUserResponseModel>
{
    public async Task<GetLoggedInUserResponseModel> Handle(GetLoggedInUserQuery request,
        CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(currentUser.Id);
        if (user == null)
        {
            user.ThrowNotFoundIfNull(currentUser.Id);
        }

        return new(user.Id, user.Email!);
    }
}

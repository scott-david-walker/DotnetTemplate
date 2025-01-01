using System.Security.Claims;
using Core;
using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Api;

public class CurrentUser(
    IHttpContextAccessor httpContextAccessor,
    UserManager<User> userManager)
    : ICurrentUser
{
    private readonly ClaimsPrincipal _contextUser = httpContextAccessor!.HttpContext!.User;

    public async Task<string> Id()
    {
        var user = await userManager.GetUserAsync(_contextUser);
        return user!.Id;
    }
}
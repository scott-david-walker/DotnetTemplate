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
    private User? _user;

    private async Task<User> GetUser()
    {
        if (_user != null)
        {
            return _user;
        }
        _user = await userManager.FindByNameAsync(_contextUser.Identity!.Name!);
        return _user!;
    }
    public async Task<string> Id()
    {
        var user = await GetUser();
        return user.Id;
    }
}
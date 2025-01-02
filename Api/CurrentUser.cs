using System.Security.Claims;
using Api.Framework.Constants;
using Core;

namespace Api;

public class CurrentUser(
    IHttpContextAccessor httpContextAccessor)
    : ICurrentUser
{
    private readonly ClaimsPrincipal _contextUser = httpContextAccessor.HttpContext!.User;
    public string Email => _contextUser.FindFirstValue(ClaimTypes.Name)!;
    public string Id => _contextUser.FindFirstValue(CustomClaimTypes.IdClaim)!;
}
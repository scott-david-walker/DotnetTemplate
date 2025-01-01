using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers;

[Authorize]
public abstract class AuthController : ApiController;
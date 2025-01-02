using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers.Framework;

[Authorize]
public abstract class AuthController : ApiController;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Api.Application.UseCases.Account;
using Api.Controllers.Framework;
using Api.Framework.Constants;
using Api.Settings;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Api.Controllers;
public record LoginPostModel(string Email, string Password);

[Route("[controller]")]
public class AccountController(JwtSettings jwtSettings, UserManager<User> userManager) : ApiController
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginPostModel model)
    {
        var user = await userManager.FindByNameAsync(model.Email);
        if (user == null)
        {
            return Unauthorized();
        }
        var isCorrect = await userManager.CheckPasswordAsync(user, model.Password);
        if(!isCorrect)
        {
            return Unauthorized();
        }
        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new([
                new(ClaimTypes.Name, user.UserName),
                new(CustomClaimTypes.IdClaim, user.Id)
            ]),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new(new SymmetricSecurityKey(jwtSettings.GetKey()), SecurityAlgorithms.HmacSha256Signature),
            Issuer = jwtSettings.Issuer,
            Audience = jwtSettings.Audience
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return Ok(new { Token = tokenString });
    }
    
    [Authorize]
    [HttpGet("loggedInUser")]
    public async Task<IActionResult> GetLoggedInUser()
    {
        var result = await Mediator.Send(new GetLoggedInUserQuery());
        return Ok(result);
    }
}
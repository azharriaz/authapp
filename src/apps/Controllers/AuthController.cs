using AuthApp.Api.Controllers;
using AuthApp.Application.ApplicationUser.Queries.GetToken;
using AuthApp.Application.Auth.Commands;
using AuthApp.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuthApp.API.Controllers;

/// <summary>
/// auth controller, manages the basic sign in operations
/// </summary>
public class AuthController : BaseApiController
{
    [HttpPost("login")]
    public async Task<ActionResult<ServiceResult<LoginResponse>>> Login([FromBody] LoginCommand loginCommand, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(loginCommand, cancellationToken);
        return Ok(result);
    }
}

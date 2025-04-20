using AuthApp.Api.Controllers;
using AuthApp.Application.Auth.Commands.Login;
using AuthApp.Application.Auth.Commands.RefreshToken;
using AuthApp.Application.Dto;
using AuthApp.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthApp.API.Controllers;

/// <summary>
/// auth controller, manages the basic sign in operations
/// </summary>
public class AuthController : BaseApiController
{
    /// <summary>
    /// Logs in user after validating the client credentials and returns the user details along with jwt.
    /// </summary>
    /// <param name="loginCommand"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("login")]
    public async Task<ActionResult<ServiceResult<LoginDto>>> Login([FromBody] LoginCommand loginCommand, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(loginCommand, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Refreshes the JWT access token using a valid refresh token
    /// </summary>
    /// <param name="command">Refresh token command containing expired access token and refresh token</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>New access and refresh tokens</returns>
    [AllowAnonymous]
    [HttpPost("refresh-token")]
    public async Task<ActionResult<ServiceResult<LoginDto>>> RefreshToken([FromBody] RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }
}

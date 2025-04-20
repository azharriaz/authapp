using AuthApp.Api.Controllers;
using AuthApp.Application.ApplicationUser.Commands.CreateUser;
using AuthApp.Application.ApplicationUser.Commands.Delete;
using AuthApp.Application.ApplicationUser.Commands.UpdateUser;
using AuthApp.Application.ApplicationUser.Queries.GetById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthApp.API.Controllers;

[Authorize]
public class UsersController : BaseApiController
{
    /// <summary>
    /// Creates a new user
    /// </summary>
    [HttpPost]
    public async Task<ActionResult> Create(CreateUserCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }

    /// <summary>
    /// Updates an existing user
    /// </summary>
    [HttpPut]
    public async Task<ActionResult> Update(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }

    /// <summary>
    /// Deletes a user by ID
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(new DeleteUserCommand { UserId = id }, cancellationToken));
    }

    /// <summary>
    /// Gets a user by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(string id, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(new GetUserByIdQuery { UserId = id }, cancellationToken));
    }
}

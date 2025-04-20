using AuthApp.Api.Controllers;
using AuthApp.Application.ApplicationRole.Commands.CreateRole;
using AuthApp.Application.ApplicationRole.Commands.Delete;
using AuthApp.Application.ApplicationRole.Commands.Update;
using AuthApp.Application.ApplicationRole.Queries.GetById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class RolesController : BaseApiController
{
    /// <summary>
    /// Creates a new role
    /// </summary>
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateRoleCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }

    /// <summary>
    /// Updates an existing role
    /// </summary>
    [HttpPut]
    public async Task<ActionResult> Update([FromBody] UpdateRoleCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }

    /// <summary>
    /// Deletes a role by ID
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(new DeleteRoleCommand { RoleId = id }, cancellationToken));
    }

    /// <summary>
    /// Gets a role by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(string id, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(new GetRoleByIdQuery { RoleId = id }, cancellationToken));
    }
}

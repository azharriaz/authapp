using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace AuthApp.Api.Controllers;

/// <summary>
/// Base api controller
/// </summary>
[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    private ISender _mediator;

    /// <summary>
    /// Mediator sender
    /// </summary>
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>();
}
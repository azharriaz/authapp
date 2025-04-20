using AuthApp.Application.Common.Interfaces;
using AuthApp.Domain.Models;
using MediatR;

namespace AuthApp.Application.ApplicationRole.Commands.Delete;

public class DeleteRoleCommand : IRequestWrapper<Unit>
{
    public string RoleId { get; set; }
}

public class DeleteRoleCommandHandler : IRequestHandlerWrapper<DeleteRoleCommand, Unit>
{
    private readonly IIdentityService _identityService;

    public DeleteRoleCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<ServiceResult<Unit>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var result = await _identityService.DeleteRoleAsync(request.RoleId);
        
        return (ServiceResult<Unit>)(result.Succeeded
            ? ServiceResult.Success(Unit.Value)
            : ServiceResult.Failed(ServiceError.CustomMessage(result.Errors.First())));
    }
}

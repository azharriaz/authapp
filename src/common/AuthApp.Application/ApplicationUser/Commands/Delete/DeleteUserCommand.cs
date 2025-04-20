using AuthApp.Application.Common.Interfaces;
using AuthApp.Domain.Models;
using MediatR;

namespace AuthApp.Application.ApplicationUser.Commands.Delete;

public class DeleteUserCommand : IRequestWrapper<Unit>
{
    public string UserId { get; set; }
}

public class DeleteUserCommandHandler(IIdentityService identityService) : IRequestHandlerWrapper<DeleteUserCommand, Unit>
{
    public async Task<ServiceResult<Unit>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var result = await identityService.DeleteUserAsync(request.UserId);
        return (ServiceResult<Unit>)(result.Succeeded
            ? ServiceResult.Success(Unit.Value)
            : ServiceResult.Failed(ServiceError.CustomMessage(result.Errors.First())));
    }
}

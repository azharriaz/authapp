using AuthApp.Application.Common.Interfaces;
using AuthApp.Application.Dto;
using AuthApp.Domain.Models;

namespace AuthApp.Application.Auth.Commands.UpdateUser;

public class UpdateUserCommand : IRequestWrapper<ApplicationUserDto>
{
    public string UserId { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public List<string>? Roles { get; set; }
}

public class UpdateUserCommandHandler(IIdentityService identityService)
    : IRequestHandlerWrapper<UpdateUserCommand, ApplicationUserDto>
{
    public async Task<ServiceResult<ApplicationUserDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var updateResult = await identityService.UpdateUserAsync(
            request.UserId,
            request.Username,
            request.Email
        );

        if (!updateResult.Succeeded)
        {
            return ServiceResult.Failed<ApplicationUserDto>(ServiceError.CustomMessage(updateResult.Errors.First()));
        }
            

        if (request.Roles is not null)
        {
            await identityService.UpdateUserRoles(request.UserId, request.Roles);
        }

        var user = await identityService.GetUserByIdAsync(request.UserId);
        return ServiceResult.Success(user);
    }
}

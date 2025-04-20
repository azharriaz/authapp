using AuthApp.Application.Common.Interfaces;
using AuthApp.Application.Dto;
using AuthApp.Domain.Models;
using MapsterMapper;

namespace AuthApp.Application.ApplicationRole.Commands.Update;

public class UpdateRoleCommand : IRequestWrapper<ApplicationRoleDto>
{
    public string RoleId { get; set; }
    public string NewName { get; set; }
    public string NewDescription { get; set; }
}

public class UpdateRoleCommandHandler(IIdentityService identityService, IMapper mapper) : IRequestHandlerWrapper<UpdateRoleCommand, ApplicationRoleDto>
{
    public async Task<ServiceResult<ApplicationRoleDto>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var result = await identityService.UpdateRoleAsync(
            request.RoleId,
            request.NewName,
            request.NewDescription
        );

        if (!result.Succeeded)
        {
            return ServiceResult.Failed<ApplicationRoleDto>(ServiceError.CustomMessage(result.Errors.First()));
        }

        var role = await identityService.GetRoleByIdAsync(request.RoleId);
        return ServiceResult.Success(role);
    }
}

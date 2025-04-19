using AuthApp.Application.Common.Interfaces;
using AuthApp.Application.Dto;
using AuthApp.Domain.Models;

namespace AuthApp.Application.Auth.Commands.CreateRole;

public class CreateRoleCommand : IRequestWrapper<ApplicationRoleDto>
{
    public string Name { get; set; }
    public string Description { get; set; }
}

public class CreateRoleCommandHandler(IIdentityService identityService)
    : IRequestHandlerWrapper<CreateRoleCommand, ApplicationRoleDto>
{
    public async Task<ServiceResult<ApplicationRoleDto>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var (result, roleId) = await identityService.CreateRoleAsync(request.Name, request.Description);

        if (!result.Succeeded)
        {
            return ServiceResult.Failed<ApplicationRoleDto>(ServiceError.CustomMessage(result.Errors.First()));
        }
            
        var role = await identityService.GetRoleByIdAsync(roleId);
        
        return ServiceResult.Success(role);
    }
}
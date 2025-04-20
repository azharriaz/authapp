using AuthApp.Application.Common.Interfaces;
using AuthApp.Application.Dto;
using AuthApp.Domain.Models;

namespace AuthApp.Application.ApplicationRole.Queries.GetById;

public class GetRoleByIdQuery : IRequestWrapper<ApplicationRoleDto>
{
    public string RoleId { get; set; }
}

public class GetRoleByIdQueryHandler : IRequestHandlerWrapper<GetRoleByIdQuery, ApplicationRoleDto>
{
    private readonly IIdentityService _identityService;

    public GetRoleByIdQueryHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<ServiceResult<ApplicationRoleDto>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var role = await _identityService.GetRoleByIdAsync(request.RoleId);
        
        return role is not null
            ? ServiceResult.Success(role)
            : ServiceResult.Failed<ApplicationRoleDto>(ServiceError.NotFound);
    }
}

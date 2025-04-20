using AuthApp.Application.Common.Interfaces;
using AuthApp.Application.Dto;
using AuthApp.Domain.Models;
using MapsterMapper;

namespace AuthApp.Application.ApplicationUser.Queries.GetById;

public class GetUserByIdQuery : IRequestWrapper<ApplicationUserDto>
{
    public string UserId { get; set; }
}

public class GetUserByIdQueryHandler(IIdentityService identityService, IMapper mapper) : IRequestHandlerWrapper<GetUserByIdQuery, ApplicationUserDto>
{
    public async Task<ServiceResult<ApplicationUserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await identityService.GetUserByIdAsync(request.UserId);

        if (user == null)
        {
            return ServiceResult.Failed<ApplicationUserDto>(ServiceError.NotFound);
        }

        var roles = await identityService.GetUserRolesAsync(request.UserId);
        user.Roles = roles.ToList();

        return ServiceResult.Success(user);
    }
}

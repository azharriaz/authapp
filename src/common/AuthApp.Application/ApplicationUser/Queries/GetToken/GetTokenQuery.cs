using AuthApp.Application.Common.Interfaces;
using AuthApp.Domain.Models;

namespace AuthApp.Application.ApplicationUser.Queries.GetToken;

public class GetTokenQuery : IRequestWrapper<LoginResponse>
{
    public string Email { get; set; }

    public string Password { get; set; }
}

public class GetTokenQueryHandler(IIdentityService identityService, ITokenService tokenService) : IRequestHandlerWrapper<GetTokenQuery, LoginResponse>
{
    public async Task<ServiceResult<LoginResponse>> Handle(GetTokenQuery request, CancellationToken cancellationToken)
    {
        var user = await identityService.CheckUserPassword(request.Email, request.Password);

        if (user is null)
            return ServiceResult.Failed<LoginResponse>(ServiceError.ForbiddenError);


        return ServiceResult.Success(new LoginResponse
        {
            User = user,
            Token = tokenService.CreateJwtSecurityToken(user.Id)
        });
    }

}

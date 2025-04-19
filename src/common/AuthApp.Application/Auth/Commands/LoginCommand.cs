using AuthApp.Application.ApplicationUser.Queries.GetToken;
using AuthApp.Application.Common.Interfaces;
using AuthApp.Domain.Models;

namespace AuthApp.Application.Auth.Commands;

public class LoginCommand : IRequestWrapper<LoginResponse>
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class LoginCommandHandler(IIdentityService identityService, ITokenService tokenService) : IRequestHandlerWrapper<LoginCommand, LoginResponse>
{
    public async Task<ServiceResult<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await identityService.CheckUserPassword(request.Username, request.Password);

        if (user is null)
        {
            return ServiceResult.Failed<LoginResponse>(ServiceError.ForbiddenError);
        }

        return ServiceResult.Success(new LoginResponse
        {
            User = user,
            Token = tokenService.CreateJwtSecurityToken(user.Id)
        });
    }
}
using AuthApp.Application.Common.Interfaces;
using AuthApp.Application.Dto;
using AuthApp.Domain.Models;

namespace AuthApp.Application.Auth.Commands.Login;

public class LoginCommand : IRequestWrapper<LoginDto>
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class LoginCommandHandler(IIdentityService identityService, ITokenService tokenService) : IRequestHandlerWrapper<LoginCommand, LoginDto>
{
    public async Task<ServiceResult<LoginDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await identityService.CheckUserPasswordAsync(request.Username, request.Password);

        if (user is null)
        {
            return ServiceResult.Failed<LoginDto>(ServiceError.ForbiddenError);
        }

        var jwt = tokenService.CreateJwtSecurityToken(user.Id);
        var refreshToken = tokenService.GenerateRefreshToken();

        await tokenService.SaveRefreshToken(user.Id, refreshToken);

        return ServiceResult.Success(new LoginDto
        {
            User = user,
            Token = jwt,
            RefreshToken = refreshToken
        });
    }
}
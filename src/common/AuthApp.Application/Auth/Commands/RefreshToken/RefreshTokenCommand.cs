using AuthApp.Application.ApplicationUser.Queries.GetToken;
using AuthApp.Application.Common.Interfaces;
using AuthApp.Domain.Models;
using System.Security.Claims;

namespace AuthApp.Application.Auth.Commands.RefreshToken;

public class RefreshTokenCommand : IRequestWrapper<LoginResponse>
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}

public class RefreshTokenCommandHandler(IIdentityService identityService, ITokenService tokenService)
    : IRequestHandlerWrapper<RefreshTokenCommand, LoginResponse>
{
    public async Task<ServiceResult<LoginResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var principal = tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

        var isValidRefreshToken = await tokenService.ValidateRefreshToken(userId, request.RefreshToken);
        if (!isValidRefreshToken)
        {
            return ServiceResult.Failed<LoginResponse>(ServiceError.ForbiddenError);
        }
            
        var newToken = tokenService.CreateJwtSecurityToken(userId);
        var newRefreshToken = tokenService.GenerateRefreshToken();

        await tokenService.SaveRefreshToken(userId, newRefreshToken);

        return ServiceResult.Success(new LoginResponse
        {
            Token = newToken,
            RefreshToken = newRefreshToken,
            User = await identityService.GetUserByIdAsync(userId)
        });
    }
}
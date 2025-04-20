using AuthApp.Application.Common.Interfaces;
using AuthApp.Application.Dto;
using AuthApp.Domain.Models;
using System.Security.Claims;

namespace AuthApp.Application.Auth.Commands.RefreshToken;

public class RefreshTokenCommand : IRequestWrapper<LoginDto>
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}

public class RefreshTokenCommandHandler(IIdentityService identityService, ITokenService tokenService)
    : IRequestHandlerWrapper<RefreshTokenCommand, LoginDto>
{
    public async Task<ServiceResult<LoginDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var principal = tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

        var isValidRefreshToken = await tokenService.ValidateRefreshToken(userId, request.RefreshToken);
        if (!isValidRefreshToken)
        {
            return ServiceResult.Failed<LoginDto>(ServiceError.ForbiddenError);
        }

        var newToken = tokenService.CreateJwtSecurityToken(userId);
        var newRefreshToken = tokenService.GenerateRefreshToken();

        await tokenService.SaveRefreshToken(userId, newRefreshToken);

        return ServiceResult.Success(new LoginDto
        {
            User = await identityService.GetUserByIdAsync(userId),
            Token = newToken,
            RefreshToken = newRefreshToken
        });
    }
}
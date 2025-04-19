using System.Security.Claims;

namespace AuthApp.Application.Common.Interfaces;

public interface ITokenService
{
    string CreateJwtSecurityToken(string userId);
    string GenerateRefreshToken();
    Task<bool> ValidateRefreshToken(string userId, string refreshToken);
    Task SaveRefreshToken(string userId, string refreshToken);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}

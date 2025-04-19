using AuthApp.Domain.Models;

namespace AuthApp.Application.Common.Interfaces;

public interface IRefreshTokenRepository
{
    Task<RefreshToken> GetByUserIdAsync(string userId);
    Task<RefreshToken> GetByTokenAsync(string token);
    Task SaveAsync(RefreshToken token);
    Task RevokeTokenAsync(string tokenId);
    Task RevokeAllTokensForUserAsync(string userId);
}

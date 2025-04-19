using AuthApp.Application.Common.Interfaces;
using AuthApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthApp.Infrastructure.Persistence.Repositories;

public class EfRefreshTokenRepository(ApplicationDbContext context) : IRefreshTokenRepository
{
    public async Task<RefreshToken> GetByUserIdAsync(string userId)
    {
        return await context.RefreshTokens
            .FirstOrDefaultAsync(t => t.UserId == userId && t.Expires > DateTime.UtcNow && !t.IsRevoked);
    }

    public async Task<RefreshToken> GetByTokenAsync(string token)
    {
        return await context.RefreshTokens
            .FirstOrDefaultAsync(t => t.Token == token);
    }

    public async Task SaveAsync(RefreshToken token)
    {
        var existingToken = await context.RefreshTokens
            .FirstOrDefaultAsync(t => t.UserId == token.UserId);

        if (existingToken is not null)
        {
            existingToken.Revoked = DateTime.UtcNow;
        }

        context.RefreshTokens.Add(token);
        
        await context.SaveChangesAsync();
    }

    public async Task RevokeTokenAsync(string tokenId)
    {
        var token = await context.RefreshTokens.FindAsync(tokenId);
        if (token is not null)
        {
            token.Revoked = DateTime.UtcNow;
            await context.SaveChangesAsync();
        }
    }

    public async Task RevokeAllTokensForUserAsync(string userId)
    {
        var tokens = await context.RefreshTokens
            .Where(t => t.UserId == userId)
            .ToListAsync();

        foreach (var token in tokens)
        {
            token.Revoked = DateTime.UtcNow;
        }

        await context.SaveChangesAsync();
    }
}

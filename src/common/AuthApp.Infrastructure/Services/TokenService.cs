using AuthApp.Application.Common.Interfaces;
using AuthApp.Domain.Authentication;
using AuthApp.Domain.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthApp.Infrastructure.Services;

public class TokenService(IOptions<JwtOptions> jwtOptions, IRefreshTokenRepository refreshTokenRepository) : ITokenService
{
    public string CreateJwtSecurityToken(string id)
    {
        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.Secret));

        var token = new JwtSecurityToken(
            issuer: jwtOptions.Value.ValidIssuer,
            audience: jwtOptions.Value.ValidAudience,
            expires: DateTime.Now.AddDays(90),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );
       
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task SaveRefreshToken(string userId, string refreshToken)
    {
        await refreshTokenRepository.SaveAsync(new RefreshToken
        {
            UserId = userId,
            Token = refreshToken,
            Expires = DateTime.UtcNow.AddDays(jwtOptions.Value.RefreshTokenLifetimeDays)
        });
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];

        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        return Convert.ToBase64String(randomNumber);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var principal = tokenHandler.ValidateToken(
                token,
                TokenValidationParameters,
                out var securityToken
            );

            // Validate security token type
            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(
                    SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase
                ))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
        catch (SecurityTokenException ex)
        {
            // Log security token exception
            return null;
        }
    }

    public async Task<bool> ValidateRefreshToken(string userId, string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(refreshToken))
            return false;

        // Get stored refresh token
        var storedToken = await refreshTokenRepository.GetByUserIdAsync(userId);

        if (storedToken == null)
            return false;

        // Check if token matches and hasn't expired
        return storedToken.Token == refreshToken &&
               storedToken.Expires > DateTime.UtcNow &&
               !storedToken.IsRevoked;
    }

    private TokenValidationParameters TokenValidationParameters => new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions.Value.Secret)),
        ValidateIssuer = true,
        ValidIssuer = jwtOptions.Value.ValidIssuer,
        ValidateAudience = true,
        ValidAudience = jwtOptions.Value.ValidAudience,
        ValidateLifetime = false, // We validate expiration manually
        ClockSkew = TimeSpan.Zero
    };
}

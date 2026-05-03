using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TaskTrackerApp.Application.DTOs;
using TaskTrackerApp.Application.Interfaces;
using TaskTrackerApp.Domain.Entities;
using TaskTrackerApp.Infrastructure.Configuration;

namespace TaskTrackerApp.Infrastructure.Services;

public sealed class TokenProvider(IOptions<JwtOptions> options) : ITokenProvider
{
    private readonly JwtOptions _jwtOptions = options.Value;

    public TokenResult GenerateAccessToken(User user)
    {
        var fullName = $"{user.FirstName} {user.LastName}".Trim();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, fullName),
            new Claim(ClaimTypes.Email, user.Email),
        };
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
            SecurityAlgorithms.HmacSha256);

        var expiresDate = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiryMinutes);
        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: expiresDate,
            signingCredentials: credentials);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return new(tokenString, expiresDate);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}

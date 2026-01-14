using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.Dtos;
using Auth.Setting;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Services;

public static class TokenService
{

    public static string GenerateTokenJwt(JwtSettings jwtSettings, TokenPayloadDto payload)
    {
        DateTime Expiration = DateTime.UtcNow.AddHours(1);

        var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
        var Credential = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256Signature);

        var Claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, payload.Id.ToString()),
            new Claim(ClaimTypes.Name, payload.Name),
            new Claim(ClaimTypes.Email, payload.Email)
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings.Issuer,
            audience: jwtSettings.Audience,
            claims: Claims,
            expires: Expiration,
            signingCredentials: Credential
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static ClaimsPrincipal? ValidateToken(JwtSettings jwtSettings, string Token)
    {
        if (Token == null) return null;
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };

        return tokenHandler.ValidateToken(Token, validationParameters, out _);
    }
}
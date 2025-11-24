using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PasswordManager.DAL.Entities;
using PasswordManager.DAL.Repositories;
using PasswordManager.Features.Auth.Dtos.Reponses;
using PasswordManager.Features.Auth.Dtos.Requests;

namespace PasswordManager.Features.Auth;

public class AuthService
{
    private readonly IConfiguration _configuration;
    private readonly IConfiguration _jwtSettings;
    UserResitory userResitory;

    public AuthService(UserResitory userResitory, IConfiguration configuration)
    {
        this.userResitory = userResitory;
        this._configuration = configuration;
        this._jwtSettings = this._configuration.GetSection("JwtSettings");
    }

    public async Task<LoginResponseDto> token(LoginRequestDto payload)
    {
        if (string.IsNullOrEmpty(payload.Email) || string.IsNullOrEmpty(payload.Password))
            throw new InvalidDataException("E-mail ou Senha inválida!");

        User? user = await userResitory.GetUserByEmailAsync(payload.Email);
        
        if (user == null) throw new InvalidDataException("E-mail não cadastrado!");

        return new LoginResponseDto
        {
            Name = user.Name,
            Email = user.Email,
            Token = generateTokenJwt(user)
        };
    }


    string generateTokenJwt(User user)
    {
        var SecretKey = _jwtSettings["SecretKey"] ?? throw new InvalidDataException("Invalid JWT credentials");
        var Issuer = this._jwtSettings["Issuer"];
        var Audience = this._jwtSettings["Audience"];
        DateTime Expiration = DateTime.UtcNow.AddHours(1);

        var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
        var Credential = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256Signature);

        var Claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email)
        };


        var token = new JwtSecurityToken(
            issuer: Issuer,
            audience: Audience,
            claims: Claims,
            expires: Expiration,
            signingCredentials: Credential
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public ClaimsPrincipal? ValidateToken(string Token)
    {
        if (Token == null) return null;
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = _jwtSettings["SecretKey"] ?? throw new InvalidDataException("Invalid JWT credentials");
            var key = Encoding.UTF8.GetBytes(secretKey);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(Token, validationParameters, out var validatedToken);
            return principal;
        }
        catch (Exception)
        {
            return null;
        }
   } 
}
using Microsoft.AspNetCore.Identity;
using DAL.Entities;
using DAL.Repositories;
using Account.Features.Auth.Dtos.Reponses;
using Account.Features.Auth.Dtos.Requests;
using Auth.Services;
using Auth.Dtos;
using Auth.Setting;
using Account.Setting;

namespace Account.Features.Auth;

public class AuthService(
    UserResitory userResitory, 
    PasswordHasher<User> passwordHasher, 
    JwtSettings jwtSettings, 
    IHttpContextAccessor httpContextAccessor, 
    CookiesSettings cookiesSettings)
{
    private readonly PasswordHasher<User> passwordHasher = passwordHasher;
    private readonly UserResitory userResitory = userResitory;
    private readonly JwtSettings jwtSettings = jwtSettings;
    private readonly HttpContext httpContext = httpContextAccessor.HttpContext 
        ?? throw new ArgumentNullException(nameof(httpContextAccessor), "HttpContext is null.");
    private readonly CookiesSettings cookiesSettings = cookiesSettings;

    public async Task<LoginResponseDto> Token(LoginRequestDto payload)
    {
        User? user = await userResitory.GetUserByEmailAsync(payload.Email) 
            ?? throw new InvalidDataException("Email not registered!");
        
        var verifyPass = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, payload.Password);
        if(verifyPass != PasswordVerificationResult.Success) throw new InvalidDataException("Invalid password!");

    
        SetAuthCookie(httpContext, GenToken(user));

        return new LoginResponseDto
        {
            Name = user.Name,
            Email = user.Email,
        };
    }

    private void SetAuthCookie(HttpContext context, string token)
    {
        var cookieOptions = new CookieOptions{
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddHours(1)
        };

        context.Response.Cookies.Append(cookiesSettings.AuthCookie, token, cookieOptions);
    }

    private string GenToken(User user)
    {
        var payload = new TokenPayloadDto(){ Id = user.Id, Email = user.Email, Name = user.Name};
        
        return TokenService.GenerateTokenJwt(jwtSettings, payload);
    }
}
using Auth.Services;
using Auth.Setting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Extension;

public static class AuthExtension
{
    public static void AddAuthenticationConfig(this IServiceCollection services, JwtSettings jwtSettings, string jwtCookieName)
    {

        services.AddAuthentication((x) =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = true;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                ValidateLifetime = true,
                ValidateIssuer = false,
                ValidateAudience = false
            };
            x.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var logger = context.HttpContext.RequestServices.GetService<ILogger>();
                    var token = context.Request.Cookies[jwtCookieName];
                    if (token == null) return Task.CompletedTask;

                    try
                    {
                        var isValid = TokenService.ValidateToken(jwtSettings, token);
                        if (isValid != null) context.Token = token;
                    }
                    catch (SecurityTokenExpiredException ex)
                    {
                        logger?.LogWarning("Token expired: {Message}", ex.Message);
                    }
                    catch (SecurityTokenException ex)
                    {
                        logger?.LogWarning("Invalid token: {Message}", ex.Message);
                    }
                    catch (Exception ex)
                    {
                        logger?.LogError(ex, "Erro no AuthExtension: {Message}", ex.Message);
                    }
                    return Task.CompletedTask;
                },
            };
        });
    }
}
using System.Security.Claims;
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
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                ValidateLifetime = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                ClockSkew = TimeSpan.Zero
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
                        var claims = TokenService.ValidateToken(jwtSettings, token);
                        if (claims != null) context.Token = token;
                    }
                    catch (SecurityTokenExpiredException ex)
                    {
                        logger?.LogWarning("Token expired: {Message}", ex.Message);
                        throw new UnauthorizedAccessException("Token expired. Please log in again.");
                    }
                    catch (SecurityTokenException ex)
                    {
                        logger?.LogWarning("Invalid token: {Message}", ex.Message);
                    }
                    catch (Exception ex)
                    {
                        logger?.LogError(ex, "Error in AuthExtension: {Message}", ex.Message);
                    }

                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    var logger = context.HttpContext.RequestServices.GetService<ILogger>();

                    var claims = context.Principal;

                    var IsMFAEnabled = GetClainmValue<bool>(claims!, "IsMFAEnabled");
                    var IsMFAPending = GetClainmValue<bool>(claims!, "IsMFAPending");

                    logger?.LogInformation("Token received. IsMFAEnabled: {IsMFAEnabled}, IsMFAPending: {IsMFAPending}", IsMFAEnabled, IsMFAPending);

                    if (IsMFAEnabled && IsMFAPending)
                    {
                        var isMFARoute = context.Request.Path.StartsWithSegments("/api/v1/auth");
                        if (!isMFARoute)
                        {
                            logger?.LogInformation("MFA is pending. Rejecting token.");
                            throw new UnauthorizedAccessException("MFA verification is pending. Please complete MFA verification.");
                        }
                    }

                    return Task.CompletedTask;
                }
            };
        });
    }

    private static T GetClainmValue<T>(ClaimsPrincipal claimsPrincipal, string claimType)
    {
        var claim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == claimType)
            ?? throw new Exception($"Claim {claimType} not found");

        return (T)Convert.ChangeType(claim.Value, typeof(T));
    }
}
using Microsoft.AspNetCore.Http;
using Auth.Services;
using Microsoft.IdentityModel.Tokens;
using Auth.Setting;
using Microsoft.Extensions.Logging;

namespace Auth.Middleware;

public class AuthMiddleware(RequestDelegate next, JwtSettings jwtSettings, ILogger<AuthMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly string AuthorizationHeader = "Authorization";
    private readonly ILogger<AuthMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            if (!context.Request.Headers.TryGetValue(AuthorizationHeader, out var token))        
            {
                _logger.LogWarning($"Header '{AuthorizationHeader}' não encontrado!");
                throw new InvalidDataException($"Not found Header {AuthorizationHeader}");
            }

            token = token.ToString().Substring("Bearer".Length).Trim() 
                ?? throw new InvalidDataException("Invalid token!"); 
            
            _logger.LogInformation($"Token recebido: {token.ToString().Substring(0, Math.Min(20, token.ToString().Length))}...");
            
            var principal = TokenService.ValidateToken(jwtSettings, token!) 
                ?? throw new InvalidDataException("InvalidToken");
            context.User = principal;

            await _next(context);
        }
        catch (SecurityTokenExpiredException ex)
        {
            _logger.LogWarning("Token expirado: {Message}", ex.Message);
            throw new UnauthorizedAccessException("Your authentication token has expired. Please sign in again to continue.");
        }
        catch (SecurityTokenException ex)
        {
            _logger.LogWarning("Token inválido: {Message}", ex.Message);
            throw new UnauthorizedAccessException("Invalid authentication token. Please sign in again.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro no AuthMiddleware: {Message}", ex.Message);
            throw;
        }
    }
}

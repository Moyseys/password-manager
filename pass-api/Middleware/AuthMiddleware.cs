using System.Net;
using PasswordManager.Features.Auth;

namespace PasswordManager.Middleware;

class AuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string AuthorizationHeader = "Authorization";

    public AuthMiddleware(RequestDelegate next)
    {
        this._next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var authService = context.RequestServices.GetRequiredService<AuthService>();
        if (!context.Request.Headers.TryGetValue(AuthorizationHeader, out var token))        
            throw new InvalidDataException($"Not found Header {AuthorizationHeader}");

        token = token.ToString().Substring("Bearer".Length).Trim() ?? throw new InvalidDataException("Invalid token!"); 
        
        var principal = authService.ValidateToken(token!);
        if (principal == null)
            throw new InvalidDataException("InvalidToken");
            
        context.User = principal;

        await _next(context);
    }
}

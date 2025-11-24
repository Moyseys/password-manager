using System.Net;
using System.Text.Json;
using PasswordManager.Features.Auth;

namespace PasswordManager.Middleware;

class AuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly String AuthorizationHeader = "Authorization";

    public AuthMiddleware(RequestDelegate next)
    {
        this._next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var msg = new { message = "Token de Authenticação inválido!" };
        var authService = context.RequestServices.GetRequiredService<AuthService>();
        if (!context.Request.Headers.TryGetValue(AuthorizationHeader, out var token))
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsync(JsonSerializer.Serialize(msg));
            return;
        }
        token = token.ToString().Substring("Bearer".Length).Trim();
        var principal = authService.ValidateToken(token);
        if (principal == null)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsync(JsonSerializer.Serialize(msg));
            return;
        }
        context.User = principal;

        await _next(context);
    }
}

//Todo expiração do token
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Core.Contexts;

public class UserContext(IHttpContextAccessor httpAcessor, IConfiguration configuration)
{
    private readonly IHttpContextAccessor _httpAcessor = httpAcessor;
    private readonly IConfiguration _configuration = configuration;

    // Fallback de desenvolvimento: quando a autenticação está desativada e nenhum
    // token está presente, usa o usuário configurado em "DevAuth:UserId".
    // Se a chave não existir (ex.: app Account), o comportamento normal é mantido.
    private Guid? DevUserId =>
        Guid.TryParse(_configuration["DevAuth:UserId"], out var devUserId) ? devUserId : null;

    public Guid? UserId
    {
        get
        {
            var userIdClaim = _httpAcessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(userIdClaim, out var userId) ? userId : DevUserId;
        }
    }

    public Guid GetUserIdOrThrow()
    {
        var userIdClaim = _httpAcessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (Guid.TryParse(userIdClaim, out var userId)) return userId;
        return DevUserId ?? throw new UnauthorizedAccessException("User is not found");
    }

    public string? Name => _httpAcessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value;

    public string? Email => _httpAcessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;

    public bool IsMFAEnabled => _httpAcessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == "IsMFAEnabled")?.Value == "True";

    public bool IsMFAPending => _httpAcessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == "IsMFAPending")?.Value == "True";
}
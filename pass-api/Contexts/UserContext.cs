using System.Security.Claims;

namespace PasswordManager.Contexts;

public class UserContext(IHttpContextAccessor httpAcessor)
{
    private readonly IHttpContextAccessor _httpAcessor = httpAcessor;    

    public Guid? UserId
    {
        get
        {
            var userIdClaim = _httpAcessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
        }
    }

    public Guid GetUserIdOrThrow()
    {
        var userIdClaim = _httpAcessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(userIdClaim, out var userId) ? userId : throw new UnauthorizedAccessException("User is not found");
    }
}
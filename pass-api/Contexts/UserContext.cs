using System.Security.Claims;

namespace PasswordManager.Contexts;

public class UserContext(IHttpContextAccessor httpAcessor)
{
    private readonly IHttpContextAccessor _httpAcessor = httpAcessor;
    private readonly ClaimsPrincipal? _userContext = httpAcessor.HttpContext?.User;
    

    public Guid? UserId
    {
        get
        {
            var userIdClaim = _userContext?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
        }
    }
}
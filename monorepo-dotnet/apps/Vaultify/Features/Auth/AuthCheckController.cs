using Microsoft.AspNetCore.Mvc;

namespace Vaultify.Features.Auth;

// Stub de desenvolvimento: com a autenticação desativada, expõe o usuário
// configurado em "DevAuth" para que o guard do frontend passe (evita 404/401
// em /api/v1/auth/check quando o frontend bate direto no Vaultify).
[ApiController]
[Route("api/v1/auth")]
public class AuthCheckController(IConfiguration configuration) : ControllerBase
{
    private readonly IConfiguration _configuration = configuration;

    [HttpGet("check")]
    public IActionResult Check()
    {
        return Ok(new
        {
            name = _configuration["DevAuth:Name"],
            email = _configuration["DevAuth:Email"],
            isMFAEnabled = false,
            isMFAPending = false
        });
    }

    [HttpPost("logout")]
    public IActionResult Logout() => Ok();
}

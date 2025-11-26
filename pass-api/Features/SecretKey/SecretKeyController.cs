using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace PasswordManager.Features.SecretKey;

[ApiController]
[Route("api/v1/secret-key")]
public class SecretKeyController : ControllerBase
{
    private readonly SecretKeyService secretKeyService;

    public SecretKeyController(SecretKeyService secretKeyService)
    {
        this.secretKeyService = secretKeyService;
    }

    [HttpPatch]
    async public Task<ActionResult<SecretKeyRequestDto>> Update([FromBody] SecretKeyRequestDto payload)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value ?? throw new InvalidOperationException("Invalid user");
    
            return Ok("Not implemented");
        }
        catch (BadHttpRequestException error)
        {
            return BadRequest(new { message = error.Message });
        }
    }
}
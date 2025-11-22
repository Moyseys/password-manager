using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.DAL.Entities;

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

    [HttpPost]
    async public Task<ActionResult<SecretKeyRequestDto>> Create([FromBody] SecretKeyRequestDto payload)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value ?? throw new InvalidOperationException("Invalid user");
    
        return Ok(await secretKeyService.Create(payload, Guid.Parse(userId)));
        }
        catch (BadHttpRequestException error)
        {
            return BadRequest(new { message = error.Message });
        }
    }
}
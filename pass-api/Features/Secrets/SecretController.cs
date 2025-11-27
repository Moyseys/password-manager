using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Features.Secrets;
using PasswordManager.Features.Secrets.Dtos.Requests;
using PasswordManager.SharedDtos;
using PasswordManager.Utils;

[ApiController]
[Route("api/v1/secrets")]
public class SecretController : ControllerBase
{
    private SecretService _secretService;

    public SecretController(SecretService secretService)
    {
        this._secretService = secretService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SecretRequestCreateDto payload)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            ?? throw new InvalidOperationException("User Id invalid!");


        await _secretService.CreateSecret(payload, Guid.Parse(userId));
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] PaginationDto pagination)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException("User Id invalid");
        var secrets = await _secretService.ListSecrets(Guid.Parse(userId), pagination);
        return Ok(secrets);            
    }

    [HttpPost("{secretId}")]
    public async Task<IActionResult> Show(string secretId, [FromBody] SecretRequestShowDto payload)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException("User Id invalid");
        return Ok(await _secretService.GetSecret(Guid.Parse(userId), Guid.Parse(secretId), payload));
    }
}
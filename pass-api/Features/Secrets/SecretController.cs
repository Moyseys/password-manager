using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Features.Secrets;
using PasswordManager.Features.Secrets.Dtos.Requests;
using PasswordManager.SharedDtos;

[ApiController]
[Route("api/v1/secrets")]
public class SecretController(SecretService secretService) : ControllerBase
{
    private readonly SecretService _secretService = secretService;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SecretRequestCreateDto payload)
    {
        await _secretService.CreateSecret(payload);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] PaginationDto pagination)
    {
        var secrets = await _secretService.ListSecrets(pagination);
        return Ok(secrets);            
    }

    [HttpPost("{secretId}")]
    public async Task<IActionResult> Show(string secretId, [FromBody] SecretRequestShowDto payload)
    {
        return Ok(await _secretService.GetSecret(Guid.Parse(secretId), payload));
    }

    [HttpPatch("{secretId}")]
    public async Task<IActionResult> Update(Guid secretId, [FromBody] SecretRequestUpdateDto payload, CancellationToken cancellationToken)
    {
        return Ok(await _secretService.UpdateSecret(secretId, payload, cancellationToken));
    }
}
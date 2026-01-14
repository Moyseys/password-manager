using DAL.Dtos;
using Microsoft.AspNetCore.Mvc;
using Vaultify.Features.Secrets;
using Vaultify.Features.Secrets.Dtos.Requests;

[ApiController]
[Route("api/v1/secrets")]
public class SecretController(SecretService secretService, ILogger<SecretController> logger) : ControllerBase
{
    private readonly SecretService _secretService = secretService;
    private readonly ILogger<SecretController> _logger = logger;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SecretRequestCreateDto payload)
    {
        await _secretService.CreateSecret(payload);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] PaginationDto pagination)
    {
        return Ok(await _secretService.ListSecrets(pagination));
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
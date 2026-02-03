using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vaultify.Features.SecretKeyF;
using Vaultify.Features.SecretKeyF.Dtos;


[ApiController]
[Route("api/v1/secret-key")]
[Authorize]
public class SecretKeyController(SecretKeyService secretKeyService) : ControllerBase
{
    private readonly SecretKeyService secretKeyService = secretKeyService;

    [HttpPost]
    public async Task<ActionResult<SecretKeyResponseDto>> Create([FromBody] SecretKeyRequestDto payload)
    {
        return Ok(await secretKeyService.CreateSecretKey(payload));
    }

    [HttpGet]
    public async Task<ActionResult<SecretKeyResponseDto>> Show()
    {
        return Ok(await secretKeyService.GetCurrentSecretKey());
    }
}
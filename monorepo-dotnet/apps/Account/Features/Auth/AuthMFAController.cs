using Account.Features.Auth.Dtos.Requests;
using Account.Features.Auth.Dtos.Responses;
using Account.Features.Auth.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Account.Features.Auth;

[ApiController]
[Authorize]
[Route("api/v1/auth/mfa")]
public class AuthMFAController(
    IAuthMFAService authMFAService
) : ControllerBase
{
    private readonly IAuthMFAService _authMFAService = authMFAService;

    [HttpPost("configure")]
    public async Task<ActionResult<MFAConfigureResponseDto>> ConfigureMFA([FromBody] MFASettingsRequestDto settings)
    {
        var response = await _authMFAService.ConfigureMFA(settings);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<MFASettingsResponseDto>>> ListMFASettings()
    {
        var response = await _authMFAService.ListMFASettings();
        return Ok(response);
    }

    [HttpPost("verify")]
    public async Task<ActionResult> VerifyMFA([FromBody] MFAVerificationRequestDto verification)
    {
        await _authMFAService.VerifyMFA(verification);
        return Ok();
    }

    [HttpPost("token")]
    public async Task<ActionResult<MFATokenGenerationResponseDto>> GenerateMFAToken([FromBody] MFATokenGenerationRequestDto request)
    {
        var response = await _authMFAService.GenerateMFAToken(request);
        return Ok(response);
    }
}
using Microsoft.AspNetCore.Authorization;
using Account.Features.Auth.Dtos.Reponses;
using Account.Features.Auth.Interfaces;
using Account.Features.Auth.Dtos.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Account.Features.Auth;

[ApiController]
[Authorize]
[Route("api/v1/auth")]
public class AuthController(IAuthService authController) : ControllerBase
{
    private readonly IAuthService _authService = authController;

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponseDto>> Token(LoginRequestDto payload)
    {
        LoginResponseDto res = await _authService.Token(payload);
        return Ok(res);
    }

    [HttpGet("check")]
    public ActionResult<CheckResponseDto> Check()
    {
        return Ok(_authService.Check());
    }

    [HttpPost("logout")]
    public ActionResult Logout()
    {
        _authService.RemoveAuthCookie();
        return Ok();
    }
}
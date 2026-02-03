using Microsoft.AspNetCore.Authorization;
using Account.Features.Auth.Dtos.Reponses;
using Account.Setting;
using Account.Features.Auth.Dtos.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Account.Features.Auth;

[ApiController]
[Route("api/v1/auth")]
[Authorize]
public class AuthController(AuthService authController, CookiesSettings cookiesSettings) : ControllerBase
{
    private readonly AuthService _authService = authController;
    private readonly CookiesSettings _cookiesSettings = cookiesSettings;

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponseDto>> Token(LoginRequestDto payload)
    {
        LoginResponseDto res = await _authService.Token(payload);
        return Ok(res);
    }

    [HttpGet("check")]
    public async Task<ActionResult> Check()
    {
        return Ok();
    }

    [HttpPost("logout")]
    public ActionResult Logout()
    {
        Response.Cookies.Delete(_cookiesSettings.AuthCookie);
        return Ok();
    }
}
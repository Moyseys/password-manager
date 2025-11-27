using Microsoft.AspNetCore.Mvc;
using PasswordManager.Features.Auth.Dtos.Reponses;
using PasswordManager.Features.Auth.Dtos.Requests;

namespace PasswordManager.Features.Auth;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authController)
    {
        _authService = authController;
    }

    [HttpPost]
    public async Task<ActionResult<LoginResponseDto>> Token(LoginRequestDto payload)
    {
        LoginResponseDto res = await _authService.token(payload);
        return Ok(res);
    }

    
}
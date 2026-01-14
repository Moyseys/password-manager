using Account.Features.Auth.Dtos.Reponses;
using Account.Features.Auth.Dtos.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Account.Features.Auth;

[ApiController]
[Route("api/v1/auth")]
public class AuthController(AuthService authController) : ControllerBase
{
    private readonly AuthService _authService = authController;

    [HttpPost]
    public async Task<ActionResult<LoginResponseDto>> Token(LoginRequestDto payload)
    {
        LoginResponseDto res = await _authService.Token(payload);
        return Ok(res);
    }

    
}
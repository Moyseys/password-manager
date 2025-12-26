
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Features.Users.Services;
using PasswordManager.Features.Users.Dtos;

namespace PasswordManager.Features.Users;

[ApiController]
[Route("api/v1/users")]
public class UserController(UserService userService) : ControllerBase
{
    private readonly UserService userService = userService;

    [HttpGet]
    public IActionResult Index()
    {
        return Ok("Oii");
    }

    [HttpPost]
    public async Task<IActionResult> Store([FromBody] CreateUserDto body)
    {
        await this.userService.CreateUser(body);
        return NoContent();
    }
}

using Microsoft.AspNetCore.Mvc;
using PasswordManager.Features.Users.Services;
using PasswordManager.Features.Users.Dtos;

namespace PasswordManager.Features.Users;

[ApiController]
[Route("api/v1/users")]
public class UserController : ControllerBase
{
    private readonly UserService userService;

    public UserController(UserService userService) {
        this.userService = userService;
    }

    [HttpGet]
    public IActionResult index()
    {
        return Ok("Oii");
    }

    [HttpPost]
    public async Task<IActionResult> store([FromBody] CreateUserDto body)
    {
        await this.userService.CreateUser(body);
        return NoContent();
    }
}
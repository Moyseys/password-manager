using Account.Features.Users.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedDto.Dtos;

namespace Account.Features.Users;

[ApiController]
[Authorize]
[Route("api/v1/users")]
public class UserController(UserService userService) : ControllerBase
{
    private readonly UserService userService = userService;

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Store([FromBody] CreateUserDto body)
    {
        await this.userService.CreateUser(body);
        return NoContent();
    }
}
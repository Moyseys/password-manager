using Account.Features.Users.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedDto.Dtos;

namespace Account.Features.Users;

[ApiController]
[Authorize]
[Route("api/v1/users")]
public class UserController(IUserService userService) : ControllerBase
{
    private readonly IUserService userService = userService;

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Store([FromBody] CreateUserDto body)
    {
        await this.userService.CreateUser(body);
        return NoContent();
    }

    [HttpDelete()]
    public async Task<IActionResult> Delete()
    {
        await this.userService.DeleteUser();
        return NoContent();
    }
}
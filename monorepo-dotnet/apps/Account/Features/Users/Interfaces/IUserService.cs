using SharedDto.Dtos;

namespace Account.Features.Users.Interfaces;

public interface IUserService
{
    Task CreateUser(CreateUserDto payload);
    Task DeleteUser();
}
namespace SharedDto.Dtos;

public class CreateUserDto
{
    public required string Email { set; get; }
    public required string Name { set; get; }
    public required string Password { set; get; }
}

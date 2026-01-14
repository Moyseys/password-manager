namespace Account.Features.Auth.Dtos.Reponses;

public class LoginResponseDto
{
    public required string Name { get; set; }
    public required string Email { get; set;  }
    public required string Token { get; set; }
}
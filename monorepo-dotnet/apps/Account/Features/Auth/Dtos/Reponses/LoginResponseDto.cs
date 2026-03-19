namespace Account.Features.Auth.Dtos.Reponses;

public class LoginResponseDto
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required bool IsMFAEnabled { get; set; }
    public required bool IsMFAPending { get; set; }
}
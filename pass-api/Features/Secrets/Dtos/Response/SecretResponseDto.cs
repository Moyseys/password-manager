namespace PasswordManager.Features.Secrets.Dtos.Response;

public class SecretResponseDto
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
}
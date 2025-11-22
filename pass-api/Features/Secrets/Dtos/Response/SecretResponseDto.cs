namespace PasswordManager.Features.Secrets.Dtos.Response;

public class SecretResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
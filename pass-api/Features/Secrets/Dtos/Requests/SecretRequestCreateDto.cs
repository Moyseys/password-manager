namespace PasswordManager.Features.Secrets.Dtos.Requests;

public class SecretRequestCreateDto
{
    public string Title { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}
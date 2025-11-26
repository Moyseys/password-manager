namespace PasswordManager.Features.Secrets.Dtos.Requests;

public class SecretRequestCreateDto
{
    public required string Title { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public required string MasterPassword{ get; set; }

}    
namespace PasswordManager.Features.Secrets.Dtos.Response;

using PasswordManager.DAL.Dtos;

public class SecretResponseListDto
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public required AuditableDto Audit { get; set;}
}
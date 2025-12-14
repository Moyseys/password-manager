namespace PasswordManager.Features.Secrets.Dtos.Response;

using PasswordManager.DAL.Dtos;

public class SecretResponseDto
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required AuditableDto Audit { get; set;}
}
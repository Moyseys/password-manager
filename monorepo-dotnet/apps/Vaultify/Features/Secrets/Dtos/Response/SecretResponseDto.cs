using DAL.Dtos;

namespace Vaultify.Features.Secrets.Dtos.Response;

public class SecretResponseDto
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required AuditableDto Audit { get; set;}
}
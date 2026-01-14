using DAL.Dtos;

namespace SharedDto.Dtos;

public class SecretResponseDto
{
    public required Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required AuditableDto Audit { get; set; }
}

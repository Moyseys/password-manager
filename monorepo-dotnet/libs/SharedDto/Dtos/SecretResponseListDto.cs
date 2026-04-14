using DAL.Dtos;
using DAL.Enums;

namespace SharedDto.Dtos;

public class SecretResponseListDto
{
    public required Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Username { get; set; }
    public string? Category { get; set; }
    public SecretStrengthEnum Strength { get; set; }
    public required string Password { get; set; }
    public required AuditableDto Audit { get; set; }
}

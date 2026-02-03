using DAL.Dtos;

namespace SharedDto.Dtos;

public class SecretResponseDto
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Username { get; set; }
    public required string Website { get; set; }
    public required byte[] CipherPassword { get; set; }
    public required byte[] IV { get; set; }
    public required AuditableDto Audit { get; set; }
}
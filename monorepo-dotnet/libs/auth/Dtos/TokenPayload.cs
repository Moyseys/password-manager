namespace Auth.Dtos;

public class TokenPayloadDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public bool IsMFAEnabled { get; set; }
    public bool IsMFAPending { get; set; }
}

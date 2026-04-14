using DAL.Enums;

namespace SharedDto.Dtos;

public class SecretRequestUpdateDto
{
    public string? Title { get; set; }
    public string? Username { get; set; }
    public string? Website { get; set; }
    public string? Category { get; set; }
    public SecretStrengthEnum? Strength { get; set; }
    public byte[]? CipherPassword { get; set; }
    public byte[]? IV { get; set; }
    public bool? Active { get; set; }
}

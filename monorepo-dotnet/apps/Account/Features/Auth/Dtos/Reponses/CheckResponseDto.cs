namespace Account.Features.Auth.Dtos.Reponses;

public record CheckResponseDto
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public bool? IsMFAEnabled { get; set; }
    public bool? IsMFAPending { get; set; }
}
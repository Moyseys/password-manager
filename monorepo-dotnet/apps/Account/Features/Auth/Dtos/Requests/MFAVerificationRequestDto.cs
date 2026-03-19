namespace Account.Features.Auth.Dtos.Requests;

public class MFAVerificationRequestDto
{
    public required Guid MFASettingsId { get; set; }
    public required string Token { get; set; }
}

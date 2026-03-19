namespace Account.Features.Auth.Dtos.Requests;

public class MFATokenGenerationRequestDto
{
    public required Guid MFASettingsId { get; set; }
}

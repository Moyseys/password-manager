namespace Account.Features.Auth.Dtos.Responses;

public class MFATokenGenerationResponseDto
{
    public required Guid MFASettingsId { get; set; }
    public required int ExpirationTimeInSeconds { get; set; }
}

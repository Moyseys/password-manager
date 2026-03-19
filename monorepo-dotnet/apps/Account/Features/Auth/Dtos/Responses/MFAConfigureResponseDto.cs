using DAL.Enums;

namespace Account.Features.Auth.Dtos.Responses;

public class MFAConfigureResponseDto
{
    public required Guid MFASettingsId { get; set; }
    public required MFASettingsState State { get; set; }
}

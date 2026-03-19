using DAL.Enums;

namespace Account.Features.Auth.Dtos.Requests;

public class MFASettingsRequestDto
{
    public required MFAType Type { get; set; }
}
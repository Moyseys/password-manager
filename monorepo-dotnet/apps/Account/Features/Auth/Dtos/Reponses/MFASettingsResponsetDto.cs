namespace Account.Features.Auth.Dtos.Responses;

using DAL.Dtos;
using DAL.Enums;

public class MFASettingsResponseDto
{
    public required Guid Id { get; set; }
    public required string Type { get; set; }
    public required MFASettingsState State { get; set; }
    public int? ExpirationTimeInSeconds { get; set; }
    public required AuditableDto Audit { get; set; }
}
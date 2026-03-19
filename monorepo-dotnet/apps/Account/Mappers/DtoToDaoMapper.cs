using Account.Features.Auth.Dtos.Requests;
using DAL.Entities;
using DAL.Enums;

namespace Account.Mappers;

public static class DtoToDaoMapper
{
    public static MFASettings ToMFASettings(this MFASettingsRequestDto dto, Guid userId)
    {
        return new MFASettings
        {
            UserId = userId,
            Type = Enum.Parse<MFAType>(dto.Type.ToString()),
            State = MFASettingsState.PendingVerification,
        };
    }

    public static MFAToken ToMFAToken(this MFASettings settings, string tokenHash, int tokenExpiresInSeconds)
    {
        return new MFAToken
        {
            MFASettingsId = settings.Id,
            TokenHash = tokenHash,
            ExpiresAt = DateTime.UtcNow.AddSeconds(tokenExpiresInSeconds),
            IsUsed = false,
            AttemptCount = 0
        };
    }

    public static Guid ToMFASettingsId(this MFATokenGenerationRequestDto request)
    {
        return request.MFASettingsId;
    }
}

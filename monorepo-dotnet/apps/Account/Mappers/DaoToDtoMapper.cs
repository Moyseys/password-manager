using Account.Features.Auth.Dtos.Responses;
using Auth.Dtos;
using DAL.Entities;
using DAL.Enums;
using DAL.Extensions;

namespace Account.Mappers;

public static class DaoToDtoMapper
{
    public static MFAConfigureResponseDto ToMFAConfigureResponseDto(this MFASettings mfaSettings)
    {
        return new MFAConfigureResponseDto
        {
            MFASettingsId = mfaSettings.Id,
            State = mfaSettings.State
        };
    }

    public static MFASettingsResponseDto ToMFASettingsResponseDto(this MFASettings mfaSettings, int? expirationTimeInSeconds = null)
    {
        return new MFASettingsResponseDto
        {
            Id = mfaSettings.Id,
            Type = mfaSettings.Type.ToString(),
            State = mfaSettings.State,
            ExpirationTimeInSeconds = expirationTimeInSeconds,
            Audit = mfaSettings.GetAudit()
        };
    }

    public static MFATokenGenerationResponseDto ToMFATokenGenerationResponseDto(this MFASettings mfaSettings, int expirationTimeInSeconds)
    {
        return new MFATokenGenerationResponseDto
        {
            MFASettingsId = mfaSettings.Id,
            ExpirationTimeInSeconds = expirationTimeInSeconds
        };
    }

    public static TokenPayloadDto ToTokenPayloadDto(this User user, bool isMFAEnabled, bool isMFAPending)
    {
        return new TokenPayloadDto
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name,
            IsMFAEnabled = isMFAEnabled,
            IsMFAPending = isMFAPending
        };
    }
}

using Account.Setting;
using Account.Features.Auth.Interfaces;
using Auth.Dtos;
using Auth.Services;
using Auth.Setting;

namespace Account.Features.Auth.Services;

public class AuthSessionService(
    JwtSettings jwtSettings,
    MFAEmailSettings mfaEmailSettings
) : IAuthSessionService
{
    private readonly JwtSettings _jwtSettings = jwtSettings;

    public int GetMFATokenExpirationInSeconds()
    {
        return _jwtSettings.MFATokenExpirationInSeconds;
    }

    public int GetAccessTokenExpirationInSeconds()
    {
        return _jwtSettings.AccessTokenExpirationInSeconds;
    }

    public string GenerateToken(TokenPayloadDto payload, int expirationInSeconds)
    {
        return TokenService.GenerateAccessToken(_jwtSettings, payload, expirationInSeconds);
    }
}
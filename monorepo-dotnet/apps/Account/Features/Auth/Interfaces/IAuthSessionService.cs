using Auth.Dtos;

namespace Account.Features.Auth.Interfaces;

public interface IAuthSessionService
{
    int GetMFATokenExpirationInSeconds();
    int GetAccessTokenExpirationInSeconds();
    string GenerateToken(TokenPayloadDto payload, int expirationInSeconds);
}
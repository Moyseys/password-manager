using Microsoft.AspNetCore.Identity;
using DAL.Entities;
using DAL.Repositories;
using Account.Features.Auth.Interfaces;
using Account.Mappers;
using Account.Features.Auth.Services;
using Account.Features.Auth.Dtos.Reponses;
using Account.Features.Auth.Dtos.Requests;
using Core.Contexts;
using DAL.Enums;

namespace Account.Features.Auth.Services;

public class AuthService(
    UserResitory userResitory,
    PasswordHasher<User> passwordHasher,
    UserContext userContext,
    IAuthSessionService authSessionService,
    IAuthCookieService authCookieService)
    : IAuthService
{
    private readonly PasswordHasher<User> passwordHasher = passwordHasher;
    private readonly UserResitory userResitory = userResitory;
    private readonly UserContext userContext = userContext;
    private readonly IAuthSessionService _authSessionService = authSessionService;
    private readonly IAuthCookieService _authCookieService = authCookieService;

    public async Task<LoginResponseDto> Token(LoginRequestDto payload)
    {
        User? user = await userResitory.GetUserByEmailWithMFAAsync(payload.Email)
            ?? throw new InvalidDataException("Invalid credentials!");

        var verifyPass = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, payload.Password);
        if (verifyPass != PasswordVerificationResult.Success) throw new InvalidDataException("Invalid credentials!");

        var isMFAEnabled = user.MFASettings != null && user.MFASettings.Any(mfaS => mfaS.State == MFASettingsState.Active);

        var expirationInSeconds = isMFAEnabled
            ? _authSessionService.GetMFATokenExpirationInSeconds()
            : _authSessionService.GetAccessTokenExpirationInSeconds();

        var tokenPayload = user.ToTokenPayloadDto(isMFAEnabled, isMFAPending: isMFAEnabled);

        var token = _authSessionService.GenerateToken(tokenPayload, expirationInSeconds);
        _authCookieService.SetAuthCookie(token, expirationInSeconds);

        return new LoginResponseDto
        {
            Name = user.Name,
            Email = user.Email,
            IsMFAEnabled = isMFAEnabled,
            IsMFAPending = isMFAEnabled
        };
    }

    public void RemoveAuthCookie()
    {
        _authCookieService.RemoveAuthCookie();
    }

    public CheckResponseDto Check()
    {
        return new CheckResponseDto
        {
            Name = userContext.Name,
            Email = userContext.Email,
            IsMFAEnabled = userContext.IsMFAEnabled,
            IsMFAPending = userContext.IsMFAPending
        };
    }
}
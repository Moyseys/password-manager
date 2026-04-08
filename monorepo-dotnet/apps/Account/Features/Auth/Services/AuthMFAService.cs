using System.Security.Cryptography;
using System.Text;
using Account.Features.Auth.Interfaces;
using Account.Features.Auth.Dtos.Requests;
using Account.Features.Auth.Dtos.Responses;
using Account.Mappers;
using Account.Publishers;
using Account.Setting;
using Core.Contexts;
using Core.Exceptions;
using DAL.Entities;
using DAL.Enums;
using DAL.Extensions;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using SharedDto.Messages;
using SharedDto.Enums;

namespace Account.Features.Auth.Services;

public class AuthMFAService(
    MFASettingsRepository mfaSettingsRepo,
    MFATokenRepository mfaTokenRepo,
    UserContext userContext,
    INotificationPublisher notificationPublisher,
    MFAEmailSettings mfaEmailSettings,
    IAuthSessionService authSessionService,
    IAuthCookieService authCookieService
    )
    : IAuthMFAService
{
    private readonly MFASettingsRepository _mfaSettingsRepo = mfaSettingsRepo;
    private readonly MFATokenRepository _mfaTokenRepo = mfaTokenRepo;
    private readonly UserContext _userContext = userContext;
    private readonly INotificationPublisher _notificationPublisher = notificationPublisher;
    private readonly MFAEmailSettings _mfaEmailSettings = mfaEmailSettings;
    private readonly IAuthSessionService _authSessionService = authSessionService;
    private readonly IAuthCookieService _authCookieService = authCookieService;

    public async Task<MFAConfigureResponseDto> ConfigureMFA(MFASettingsRequestDto settings)
    {
        var userId = _userContext.GetUserIdOrThrow();

        if (await _mfaSettingsRepo.ExistsByUserIdAndTypeAsync(userId, settings.Type))
            throw new InvalidDataException("MFA settings of this type already exist for the user.");

        var mfaSettings = await _mfaSettingsRepo.AddAsync(settings.ToMFASettings(userId));

        if (settings.Type == MFAType.Email)
            await GenerateAndSendMFAEmailToken(mfaSettings, "MFA Setup");

        return mfaSettings.ToMFAConfigureResponseDto();
    }

    public async Task<MFATokenGenerationResponseDto> GenerateMFAToken(MFATokenGenerationRequestDto request)
    {
        var userId = _userContext.GetUserIdOrThrow();
        var mfaSettingsId = request.ToMFASettingsId();

        var mfaSettings = await _mfaSettingsRepo.GetByIdAndUserIdAsync(mfaSettingsId, userId)
            ?? throw new MFAVerificationException("MFA settings not found for user.", MFAErrorCode.SettingsNotFound);

        var existTokenValid = await _mfaTokenRepo.ExistsValidTokenAsync(mfaSettings.Id);
        if (existTokenValid) throw new MFAVerificationException("A valid MFA token already exists. Please check your email.", MFAErrorCode.ActiveTokenAlreadyExists);

        if (mfaSettings.Type != MFAType.Email)
            throw new InvalidOperationException("Token generation is only supported for email MFA.");

        await _mfaTokenRepo.InvalidateActiveTokensAsync(mfaSettings.Id);
        await GenerateAndSendMFAEmailToken(mfaSettings, "MFA Verification");

        return mfaSettings.ToMFATokenGenerationResponseDto(_mfaEmailSettings.TokenExpiresInSeconds);
    }

    private async Task GenerateAndSendMFAEmailToken(MFASettings mfaSettings, string emailSubject)
    {
        var token = GenerateNumericToken();
        var tokenHash = HashToken(token);

        await _mfaTokenRepo.AddAsync(mfaSettings.ToMFAToken(tokenHash, _mfaEmailSettings.TokenExpiresInSeconds));
        var email = new EmailMessage(
            _userContext.Email ?? throw new InvalidOperationException("User email not found in context"),
            emailSubject,
            TemplateEnum.MFACode,
            new Dictionary<string, string>
            {
                ["UserName"] = _userContext.Name ?? "User",
                ["Code"] = token,
                ["ExpirationMinutes"] = (_mfaEmailSettings.TokenExpiresInSeconds / 60).ToString()
            },
            IsHtml: true
        );
        await _notificationPublisher.PublishEmailAsync(email);
    }


    public async Task<List<MFASettingsResponseDto>> ListMFASettings()
    {
        var userId = _userContext.GetUserIdOrThrow();

        return await _mfaSettingsRepo
            .QueryByUserId(userId)
            .Include(s => s.User)
            .Select(setting => new MFASettingsResponseDto
            {
                Id = setting.Id,
                Type = setting.Type.ToString(),
                State = setting.State,
                ExpirationTimeInSeconds = _mfaEmailSettings.TokenExpiresInSeconds,
                Audit = setting.GetAudit()
            })
            .ToListAsync();
    }

    public async Task VerifyMFA(MFAVerificationRequestDto verification)
    {
        var userId = _userContext.UserId
            ?? throw new UnauthorizedAccessException("UserId not found in context.");

        var mfaSettings = await _mfaSettingsRepo.GetByIdAndUserIdAsync(verification.MFASettingsId, userId)
            ?? throw new MFAVerificationException("MFA settings not found for user.", MFAErrorCode.SettingsNotFound);

        var user = mfaSettings.User ?? throw new InvalidOperationException("User not found for MFA settings.");

        var mfaToken = await _mfaTokenRepo.GetLatestUnusedTokenAsync(verification.MFASettingsId)
            ?? throw new MFAVerificationException("Token not found or already used.", MFAErrorCode.TokenNotFoundOrUsed);

        if (mfaToken.ExpiresAt < DateTime.UtcNow)
        {
            await _mfaTokenRepo.MarkTokenAsUsedAsync(mfaToken);
            throw new MFAVerificationException("Token has expired.", MFAErrorCode.TokenExpired);
        }

        var tokenHash = HashToken(verification.Token.Trim());
        if (mfaToken.TokenHash != tokenHash)
        {
            mfaToken.AttemptCount++;

            if (mfaToken.AttemptCount >= _mfaEmailSettings.MaxAttempts)
            {
                mfaToken.IsUsed = true;
                await _mfaTokenRepo.UpdateAsync(mfaToken);
                throw new MFAVerificationException("Too many failed attempts. Token blocked.", MFAErrorCode.TokenBlocked, StatusCodes.Status429TooManyRequests);
            }

            await _mfaTokenRepo.UpdateAsync(mfaToken);
            throw new MFAVerificationException("Invalid token.", MFAErrorCode.InvalidToken);
        }

        var expirationInSeconds = _authSessionService.GetAccessTokenExpirationInSeconds();
        var tokenPayload = user.ToTokenPayloadDto(isMFAEnabled: true, isMFAPending: false);

        var token = _authSessionService.GenerateToken(tokenPayload, expirationInSeconds);
        _authCookieService.SetAuthCookie(token, expirationInSeconds);

        await _mfaTokenRepo.MarkTokenAsUsedAsync(mfaToken);
        if (mfaSettings.State == MFASettingsState.PendingVerification)
            await _mfaSettingsRepo.UpdateStateAsync(mfaSettings.Id, MFASettingsState.Active);
    }

    private string HashToken(string token)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_mfaEmailSettings.HashSecret));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(token));
        return Convert.ToBase64String(hash);
    }

    private static string GenerateNumericToken()
    {
        return RandomNumberGenerator.GetInt32(0, 1_000_000).ToString("D6");
    }
}
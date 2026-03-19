using Account.Features.Auth.Dtos.Requests;
using Account.Features.Auth.Dtos.Responses;

namespace Account.Features.Auth.Interfaces;

public interface IAuthMFAService
{
    Task<MFAConfigureResponseDto> ConfigureMFA(MFASettingsRequestDto settings);
    Task<MFATokenGenerationResponseDto> GenerateMFAToken(MFATokenGenerationRequestDto request);
    Task<List<MFASettingsResponseDto>> ListMFASettings();
    Task VerifyMFA(MFAVerificationRequestDto verification);
}
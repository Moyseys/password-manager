using Core.Contexts;
using DAL.Dtos;
using DAL.Entities;
using DAL.Repositories;
using SharedDto.Dtos;
using SharedDto.Mappers;
using Vaultify.Core.Mappers;
using Vaultify.Features.Secrets.Dtos.Requests;

namespace Vaultify.Features.Secrets;

public class SecretService(
    SecretRepository secretRepository,
    UserContext userContext,
    ILogger<SecretService> logger
    )
{
    private readonly SecretRepository _secretRepository = secretRepository;
    private readonly UserContext _userContext = userContext;
    private readonly ILogger<SecretService> _logger = logger;

    public async Task CreateSecret(SecretRequestCreateDto payload)
    {
        var userId = _userContext.GetUserIdOrThrow();
        await _secretRepository.AddAsync(payload.ToSecretEntity(userId));
    }

    public async Task<PageableDto<SecretResponseListDto>> ListSecrets(PaginationDto pagination, string? search)
    {
        var userId = _userContext.GetUserIdOrThrow();
        _logger.LogInformation("[ListSecrets] UserId: {UserId}", userId);

        return await _secretRepository.GetSecretsPagedByUserId(
            userId,
            s => s.ToSecretReponseListDto(),
            pagination,
            search
        );
    }

    public async Task<SecretResponseDto> GetSecret(Guid secretId)
    {
        var userId = _userContext.GetUserIdOrThrow();
        var secret = await GetUserSecret(userId, secretId);

        return secret.ToSecretResponseDto();
    }

    public async Task<SecretRequestUpdateDto> UpdateSecret(Guid secretId, SecretRequestUpdateDto payload, CancellationToken cancellationToken)
    {
        var secret = await GetUserSecret(_userContext.UserId, secretId);

        _logger.LogInformation("[UpdateSecret] Updating secret {SecretId} for user {UserId}", secretId, _userContext.UserId);

        secret.ToSecretRequestUpdateDto(payload);
        await _secretRepository.UpdateSecretAsync(secret);

        return payload;
    }


    private async Task<Secret> GetUserSecret(Guid? userId, Guid secretId)
    {
        if (userId == null) throw new UnauthorizedAccessException("User is not authenticated!");
        return await _secretRepository.GetSecretByIdAndUserId(userId.Value, secretId) ?? throw new InvalidDataException("Secret not found!");
    }
};

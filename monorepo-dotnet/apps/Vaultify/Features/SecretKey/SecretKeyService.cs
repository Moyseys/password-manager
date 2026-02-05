using System.Security.Cryptography;
using System.Text;
using Core.Contexts;
using Core.Exceptions;
using DAL.Repositories;
using Vaultify.Core.Mappers;
using Vaultify.Features.SecretKeyF.Dtos;

namespace Vaultify.Features.SecretKeyF;

public class SecretKeyService(
    SecretKeyRepository secretKeyRepository,
    UserContext userContext)
{
    private readonly SecretKeyRepository _secretKeyRepository = secretKeyRepository;
    private readonly UserContext _userContext = userContext;

    public async Task<SecretKeyResponseDto> CreateSecretKey(SecretKeyRequestDto payload)
    {
        var userId = _userContext.GetUserIdOrThrow();

        if (await _secretKeyRepository.ExistByUserId(userId))
            throw new ConflictException("Secret key already exists for this user");

        var secretKey = payload.ToSecretKeyEntity(userId);

        await _secretKeyRepository.AddAsync(secretKey);

        return secretKey.ToSecretKeyResponseDto();
    }

    public async Task<SecretKeyResponseDto> GetCurrentSecretKey()
    {
        var userId = _userContext.GetUserIdOrThrow();
        var secretKey = await _secretKeyRepository.GetSecretKeyByUserId(userId)
            ?? throw new KeyNotFoundException("Secret key not found");

        return secretKey.ToSecretKeyResponseDto();
    }
}
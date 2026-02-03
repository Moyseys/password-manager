using DAL.Entities;
using Vaultify.Features.SecretKeyF;
using Vaultify.Features.Secrets.Dtos.Requests;

namespace Vaultify.Core.Mappers;

public static class DtoToDaoMapper
{
    public static SecretKey ToSecretKeyEntity(this SecretKeyRequestDto dto, Guid userId, byte[] keyBytes, byte[] salt)
    {
        return new SecretKey
        {
            UserId = userId,
            Key = keyBytes,
            KeySalt = salt
        };
    }

    public static Secret ToSecretEntity(this SecretRequestCreateDto dto, Guid userId)
    {
        return new Secret
        {
            Title = dto.Title,
            Username = dto.Username,
            Website = dto.Website,
            CipherPassword = dto.CipherPassword,
            IV = dto.IV,
            UserId = userId
        };
    }
}
using DAL.Entities;
using Vaultify.Features.SecretKeyF;
using Vaultify.Features.Secrets.Dtos.Requests;

namespace Vaultify.Core.Mappers;

public static class DtoToDaoMapper
{
    public static SecretKey ToSecretKeyEntity(this SecretKeyRequestDto dto, Guid userId)
    {
        try
        {
            return new SecretKey
            {
                UserId = userId,
                Key = Convert.FromBase64String(dto.Key),
                KeySize = dto.KeySize,
                KeyIV = Convert.FromBase64String(dto.KeyIV),
                Salt = Convert.FromBase64String(dto.Salt),
                SaltSize = dto.SaltSize,
                Iterations = dto.Iterations,
                Algorithm = dto.Algorithm,
                HashAlgorithm = dto.HashAlgorithm,
                DerivationAlgorithm = dto.DerivationAlgorithm
            };
        }
        catch (FormatException ex)
        {
            throw new FormatException("One or more encryption parameters (Key, KeyIV, or Salt) are not in a valid Base64 format. Please verify these values and try again.", ex);
        }
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
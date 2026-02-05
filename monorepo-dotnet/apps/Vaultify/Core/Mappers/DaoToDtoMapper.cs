using System.Text;
using DAL.Entities;
using DAL.Extensions;
using Vaultify.Features.SecretKeyF.Dtos;
using Vaultify.Features.Secrets.Dtos.Response;

namespace Vaultify.Core.Mappers;

public static class DaoToDtoMapper
{
	public static SecretKeyResponseDto ToSecretKeyResponseDto(this SecretKey entity)
	{
		return new SecretKeyResponseDto
		{
			Key = Convert.ToBase64String(entity.Key),
			KeySize = entity.KeySize,
			KeyIV = Convert.ToBase64String(entity.KeyIV),
			Salt = Convert.ToBase64String(entity.Salt),
			SaltSize = entity.SaltSize,
			Iterations = entity.Iterations,
			Algorithm = entity.Algorithm,
			HashAlgorithm = entity.HashAlgorithm,
			DerivationAlgorithm = entity.DerivationAlgorithm,
			UserId = entity.UserId.ToString()
		};
	}

}
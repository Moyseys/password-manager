using DAL.Dtos;
using DAL.Entities;
using DAL.Entities.Views;
using DAL.Extensions;
using Vaultify.Features.Dtos.Reponse;
using Vaultify.Features.SecretKeyF.Dtos;

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


	public static DashboardMetricsDto ToDashboardMetricsDto(this DashboardVaultify entity)
	{
		return new DashboardMetricsDto
		{
			TotalSecrets = entity.TotalSecrets,
			NumberOfStrongSecrets = entity.NumberOfStrongSecrets,
			NumberOfWeakSecrets = entity.NumberOfWeakSecrets,
			SecurityScore = entity.SecurityScore,
			RecentSecrets = entity.RecentSecrets
		};
	}
}
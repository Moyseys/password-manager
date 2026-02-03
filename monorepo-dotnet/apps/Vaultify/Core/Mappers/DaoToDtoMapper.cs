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
			UserId = entity.UserId.ToString()
		};
	}

}
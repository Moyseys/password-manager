using System.Text;
using DAL.Entities;
using DAL.Extensions;
using SharedDto.Dtos;

namespace SharedDto.Mappers;

public static class DaoToDtoMapper
{
    public static SecretResponseDto ToSecretResponseDto(this Secret entity)
    {
        return new SecretResponseDto()
        {
            Id = entity.Id,
            Title = entity.Title,
            Username = entity.Username,
            Password = Encoding.UTF8.GetString(entity.Password),
            Audit = entity.GetAudit()
        };
    }

    public static SecretResponseListDto ToSecretReponseListDto(this Secret entity)
    {
        return new SecretResponseListDto()
        {
            Id = entity.Id,
            Title = entity.Title,
            Username = entity.Username,
            Password = string.Empty,
            Audit = entity.GetAudit()
        };
    }
}

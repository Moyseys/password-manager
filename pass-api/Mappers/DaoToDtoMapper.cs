using System.Text;
using PasswordManager.DAL.Entities;
using PasswordManager.DAL.Extensions;
using PasswordManager.Features.Secrets.Dtos.Response;

namespace PasswordManager.Mappers;

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
using System.Text;
using DAL.Entities;
using SharedDto.Dtos;

namespace SharedDto.Mappers;

public static class DtoToDaoMapper
{
    public static Secret ToSecretRequestUpdate(this Secret entity, SecretRequestUpdateDto dto)
    {
        if (dto.Title != null) entity.Title = dto.Title;
        if (dto.Username != null) entity.Username = dto.Username;
        if (dto.Website != null) entity.Website = dto.Website;
        if (dto.CipherPassword != null) entity.CipherPassword = dto.CipherPassword;
        if (dto.IV != null) entity.IV = dto.IV;
        if (dto.Active != null) entity.Active = dto.Active.Value;

        return entity;
    }

    public static User ToUser(this CreateUserDto dto)
    {
        return new User
        {
            Email = dto.Email,
            Name = dto.Name,
            Password = dto.Password,
            PasswordHash = "",
        };
    }

    public static Secret ToSecretRequestUpdateDto(this Secret entity, SecretRequestUpdateDto dto)
    {
        if (dto.Title != null) entity.Title = dto.Title;
        if (dto.Username != null) entity.Username = dto.Username;
        if (dto.CipherPassword != null) entity.CipherPassword = dto.CipherPassword;
        if (dto.IV != null) entity.IV = dto.IV;
        if (dto.Active != null) entity.Active = dto.Active.Value;

        return entity;
    }
}

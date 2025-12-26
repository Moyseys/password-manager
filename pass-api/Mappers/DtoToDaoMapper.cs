using System.Text;
using PasswordManager.DAL.Entities;
using PasswordManager.Features.Secrets.Dtos.Requests;
using PasswordManager.Features.Users.Dtos;

namespace PasswordManager.Mappers;

public static class DtoToDaoMapper
{
    public static Secret ToSecretRequestUpdate(this Secret entity, SecretRequestUpdateDto dto)
    {
        if(dto.Title != null) entity.Title = dto.Title;
        if(dto.Username != null) entity.Username = dto.Username;
        if(dto.Password != null) entity.Password = Encoding.UTF8.GetBytes(dto.Password);
        if(dto.Active != null) entity.Active = dto.Active.Value;

        return entity;
    }


    public static User ToUser(this CreateUserDto dto, byte[] masterPasswordSalt)
    {
        return new User
        {
            Email = dto.Email,
            Name = dto.Name,
            Password = dto.Password,
            PasswordHash = "",
            MasterPasswordSalt = masterPasswordSalt
        };
    }

}
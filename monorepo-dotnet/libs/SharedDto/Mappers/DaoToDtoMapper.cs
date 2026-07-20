using System.Net;
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
            Website = entity.Website,
            CategoryId = entity.CategoryId,
            Category = entity.Category.ToSecretCategoryDto(),
            Strength = entity.Strength,
            CipherPassword = entity.CipherPassword,
            IV = entity.IV,
            Audit = entity.GetAudit()
        };
    }


    public static SecretCategoryDto? ToSecretCategoryDto(this Category? category)
    {
        if (category is null) return null;

        return new SecretCategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Color = category.Color
        };
    }

    public static SecretResponseListDto ToSecretReponseListDto(this Secret entity)
    {
        return new SecretResponseListDto()
        {
            Id = entity.Id,
            Title = entity.Title,
            Username = entity.Username,
            CategoryId = entity.CategoryId,
            Category = entity.Category.ToSecretCategoryDto(),
            Strength = entity.Strength,
            Password = string.Empty,
            Audit = entity.GetAudit()
        };
    }
}

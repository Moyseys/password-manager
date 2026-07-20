using DAL.Entities;
using DAL.Extensions;
using Vaultify.Features.Categories.Dtos.Requests;
using Vaultify.Features.Categories.Dtos.Response;

namespace Vaultify.Features.Categories;

public static class CategoryMapper
{
    public static Category ToCategoryEntity(this CategoryRequestCreateDto dto, Guid userId)
    {
        return new Category
        {
            Name = dto.Name,
            Color = dto.Color,
            UserId = userId
        };
    }

    public static CategoryResponseDto ToCategoryResponseDto(this Category entity)
    {
        return new CategoryResponseDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Color = entity.Color,
            Audit = entity.GetAudit()
        };
    }
}

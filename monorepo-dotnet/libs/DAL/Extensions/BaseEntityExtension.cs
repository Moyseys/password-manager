using DAL.Dtos;
using DAL.Entities.Commons;

namespace DAL.Extensions;

public static class BaseEntityExtension
{
    public static AuditableDto GetAudit(this BaseAuditEntity entity)
    {
        return new AuditableDto(
            CreatedAt: entity.CreatedAt,
            CreatedBy: entity.CreatedBy,
            UpdatedAt: entity.UpdatedAt,
            UpdatedBy: entity.UpdatedBy
        );
    }
}
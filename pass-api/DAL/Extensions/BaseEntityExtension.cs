using PasswordManager.DAL.Dtos;
using PasswordManager.DAL.Entities.Commons;

namespace PasswordManager.DAL.Extensions;

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
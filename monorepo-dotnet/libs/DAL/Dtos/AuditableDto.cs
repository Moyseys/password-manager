namespace DAL.Dtos;

public record AuditableDto(
    DateTime? CreatedAt = null,
    Guid? CreatedBy = null,
    DateTime? UpdatedAt = null,
    Guid? UpdatedBy = null
);

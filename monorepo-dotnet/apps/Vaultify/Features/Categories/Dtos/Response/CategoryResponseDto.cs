using DAL.Dtos;

namespace Vaultify.Features.Categories.Dtos.Response;

public class CategoryResponseDto
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Color { get; set; }
    public required AuditableDto Audit { get; set; }
}

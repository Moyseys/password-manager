using System.ComponentModel.DataAnnotations;

namespace Vaultify.Features.Categories.Dtos.Requests;

public class CategoryRequestCreateDto
{
    [Required]
    public required string Name { get; set; }

    [Required]
    public required string Color { get; set; }
}

using System.ComponentModel.DataAnnotations;
using DAL.Entities.Commons;

namespace DAL.Entities;

public class Category : BaseEntity<Guid>
{
    [Required(ErrorMessage = "Name is required")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Color is required")]
    public required string Color { get; set; }

    public required Guid UserId { get; set; }

    public User? User { get; set; }

    public ICollection<Secret>? Secrets { get; set; }
}

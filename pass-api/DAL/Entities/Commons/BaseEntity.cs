using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PasswordManager.DAL.Entities.Commons;

public abstract class BaseEntity<IdType>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public IdType Id { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid? CreatedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public Boolean Active { get; set; }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities.Commons;

public abstract class BaseEntity<IdType> : BaseAuditEntity 
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public IdType Id { get; set; } = default!;
    public bool Active { get; set; } = true;
}
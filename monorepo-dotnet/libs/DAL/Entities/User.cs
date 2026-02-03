using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Entities.Commons;

namespace DAL.Entities;

public class User : BaseEntity<Guid>
{
    [Required(ErrorMessage = "Name is required")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "PasswordHash is required")]
    public required string PasswordHash { get; set; }

    [Required(ErrorMessage = "E-mail is required")]
    [EmailAddress]
    public required string Email { get; set; }

    [NotMapped()]
    public required string Password { get; set; }

    public ICollection<Secret>? Secrets { get; set; }

    public SecretKey? SecretKey { get; set; } //Opcional para 1:1 
}
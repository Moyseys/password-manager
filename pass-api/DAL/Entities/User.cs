using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PasswordManager.DAL.Entities;

//Validação via data anotation
public class User
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "PasswordHash is required")]
    public required string PasswordHash { get; set; }

    [Required(ErrorMessage = "E-mail is required")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Formato de e-mail inválido")]
    public required string Email { get; set; }

    [NotMapped()]
    public required string Password { get; set; }

    public ICollection<Secret>? Secrets { get; set; }
    
    public SecretKey? SecretKey { get; set;} //Opcional para 1:1 
}
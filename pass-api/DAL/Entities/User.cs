using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PasswordManager.DAL.Entities;

//Validação via data anotation
public class User
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    public required String Name { get; set; }

    [Required(ErrorMessage = "PasswordHash is required")]
    public required String PasswordHash { get; set; }

    [Required(ErrorMessage = "E-mail is required")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Formato de e-mail inválido")]
    public required String Email { get; set; }

    [NotMapped()]
    public required String Password { get; set; }

    public ICollection<Secret> Secrets { get; }
    public SecretKey SecretKey { get; set;} //Opcional para 1:1 
}
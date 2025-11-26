using System.ComponentModel.DataAnnotations;

namespace PasswordManager.DAL.Entities;

public class SecretKey
{
    [Key]
    public Guid Id { get; set;}
    public required byte[] Key { get; set; }
    public required Guid UserId { get; set; } //Obrigatório
    public User? User { get; set; } // Opcional
}
using System.ComponentModel.DataAnnotations;

namespace PasswordManager.DAL.Entities;

public class SecretKey
{
    [Key]
    public Guid Id { get; set;}
    public required string Key { get; set; }
    public required Guid UserId { get; set; } //Obrigatório
    public required User? User { get; set; } // Opcional
}
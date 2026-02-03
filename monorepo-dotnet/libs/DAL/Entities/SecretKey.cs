using DAL.Entities.Commons;

namespace DAL.Entities;

public class SecretKey : BaseEntity<Guid>
{
    public required byte[] Key { get; set; }
    public required byte[] KeySalt { get; set; }
    public required Guid UserId { get; set; } //Obrigatório
    public User? User { get; set; } // Opcional
}
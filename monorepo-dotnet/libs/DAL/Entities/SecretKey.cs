using DAL.Entities.Commons;

namespace DAL.Entities;

public class SecretKey : BaseEntity<Guid>
{
    public required byte[] Key { get; set; }
    public required int KeySize { get; set; }
    public required byte[] KeyIV { get; set; }
    public required byte[] Salt { get; set; }
    public required int SaltSize { get; set; }
    public required int Iterations { get; set; }
    public required string Algorithm { get; set; }
    public required string HashAlgorithm { get; set; }
    public required string DerivationAlgorithm { get; set; }
    public required Guid UserId { get; set; } //Obrigatório
    public User? User { get; set; } // Opcional
}
namespace Vaultify.Features.SecretKeyF.Dtos;

public class SecretKeyResponseDto
{
    public required string Key { get; set; }
    public required int KeySize { get; set; }
    public required string KeyIV { get; set; }
    public required string Salt { get; set; }
    public required int SaltSize { get; set; }
    public required int Iterations { get; set; }
    public required string Algorithm { get; set; }
    public required string HashAlgorithm { get; set; }
    public required string DerivationAlgorithm { get; set; }
    public required string UserId { get; set; }
}
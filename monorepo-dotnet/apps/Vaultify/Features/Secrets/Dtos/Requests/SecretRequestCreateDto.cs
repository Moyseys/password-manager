using System.ComponentModel.DataAnnotations;

namespace Vaultify.Features.Secrets.Dtos.Requests;

public class SecretRequestCreateDto
{
    [Required]
    public required string Title { get; set; }

    [Required]
    public required string Username { get; set; }

    public string? Website { get; set; }

    [Required]
    public required byte[] CipherPassword { get; set; }

    [Required]
    public required byte[] IV { get; set; }
}
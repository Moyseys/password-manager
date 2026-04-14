using System.ComponentModel.DataAnnotations;
using DAL.Enums;

namespace Vaultify.Features.Secrets.Dtos.Requests;

public class SecretRequestCreateDto
{
    [Required]
    public required string Title { get; set; }

    [Required]
    public required string Username { get; set; }

    public string? Website { get; set; }

    public string? Category { get; set; }

    public SecretStrengthEnum Strength { get; set; } = SecretStrengthEnum.Weak;

    [Required]
    public required string CipherPassword { get; set; }

    [Required]
    public required string IV { get; set; }
}
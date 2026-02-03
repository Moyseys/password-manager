using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using DAL.Entities.Commons;

namespace DAL.Entities;

public class Secret : BaseEntity<Guid>
{
    [Required(ErrorMessage = "Title is required")]
    public required string Title { get; set; }

    [Required(ErrorMessage = "Username is required")]
    public required string Username { get; set; }

    [AllowNull]
    public string? Website { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public required byte[] CipherPassword { get; set; }

    public required byte[] IV { get; set; }

    public required Guid UserId { get; set; }

    public User? User { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace PasswordManager.DAL.Entities;

public class Secret
{
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Title is required")]
    public required string Title { get; set; }
    [Required(ErrorMessage = "Username is required")]
    public required string Username { get; set; }
    [Required(ErrorMessage = "Password is required")]
    public required byte[] Password { get; set; }
    public required Guid UserId { get; set; }
    public User? User { get; set; }
}
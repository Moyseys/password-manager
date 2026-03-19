using System.ComponentModel.DataAnnotations;

namespace Account.Features.Auth.Dtos.Requests;

public class LoginRequestDto
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Email must be a valid email address.")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    public required string Password { get; set; }
}
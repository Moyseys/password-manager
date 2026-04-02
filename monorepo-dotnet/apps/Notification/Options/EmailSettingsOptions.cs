using System.ComponentModel.DataAnnotations;

namespace Notification.Options;

public record EmailSettingsOptions
{
    [Required]
    public required string Host { get; init; }
    [Required]
    public required int Port { get; init; }
    [Required]
    public required string FromEmail { get; init; }
    [Required]
    public required string FromName { get; init; }
    [Required]
    public required string Username { get; init; }
    [Required]
    public required string Password { get; init; }
}
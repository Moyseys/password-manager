using System.ComponentModel.DataAnnotations;

namespace Account.Setting;

public class MFAEmailSettings
{
    [Required]
    public required string HashSecret { get; set; }
    [Range(1, int.MaxValue)]
    public int TokenExpiresInSeconds { get; set; } = 300;
    [Range(1, int.MaxValue)]
    public int MaxAttempts { get; set; } = 5;
}

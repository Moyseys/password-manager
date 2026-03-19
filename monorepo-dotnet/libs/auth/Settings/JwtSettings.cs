using System.ComponentModel.DataAnnotations;

namespace Auth.Setting;

public class JwtSettings
{
    [Required]
    public required string SecretKey { get; set; }
    [Required]
    public string? Issuer { get; set; }
    [Required]
    public string? Audience { get; set; }
    public string? CookieName { get; set; }
    public string? CookieSameSite { get; set; }

    [Range(1, int.MaxValue)]
    public required int AccessTokenExpirationInSeconds { get; set; }
    [Range(1, int.MaxValue)]
    public required int MFATokenExpirationInSeconds { get; set; }
}

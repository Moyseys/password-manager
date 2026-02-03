namespace Auth.Setting;

public class JwtSettings
{
    public required string SecretKey { get; set; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public string? CookieName { get; set; }
}

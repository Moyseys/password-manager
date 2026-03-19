using System.ComponentModel.DataAnnotations;

namespace Account.Setting;

public class CookiesSettings
{
    [Required]
    public required string AuthCookie { get; set; }
}

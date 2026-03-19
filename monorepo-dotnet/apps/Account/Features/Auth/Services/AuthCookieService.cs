using Account.Setting;
using Account.Features.Auth.Interfaces;
using Auth.Setting;

namespace Account.Features.Auth.Services;

public class AuthCookieService(
    JwtSettings jwtSettings,
    CookiesSettings cookiesSettings,
    IHttpContextAccessor httpContextAccessor
) : IAuthCookieService
{
    private readonly JwtSettings _jwtSettings = jwtSettings;
    private readonly CookiesSettings _cookiesSettings = cookiesSettings;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public void SetAuthCookie(string token, int expirationInSeconds)
    {
        var context = _httpContextAccessor.HttpContext
            ?? throw new ArgumentNullException(nameof(_httpContextAccessor), "HttpContext is null.");

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = context.Request.IsHttps,
            SameSite = ResolveSameSiteMode(),
            Expires = DateTime.UtcNow.AddSeconds(expirationInSeconds)
        };

        context.Response.Cookies.Append(_cookiesSettings.AuthCookie, token, cookieOptions);
    }

    public void RemoveAuthCookie()
    {
        var context = _httpContextAccessor.HttpContext
            ?? throw new ArgumentNullException(nameof(_httpContextAccessor), "HttpContext is null.");

        context.Response.Cookies.Delete(_cookiesSettings.AuthCookie);
    }

    private SameSiteMode ResolveSameSiteMode()
    {
        var configuredSameSite = _jwtSettings.CookieSameSite;

        if (Enum.TryParse<SameSiteMode>(configuredSameSite, true, out var mode))
        {
            return mode;
        }

        return SameSiteMode.Strict;
    }
}
namespace Account.Features.Auth.Interfaces;

public interface IAuthCookieService
{
    void SetAuthCookie(string token, int expirationInSeconds);
    void RemoveAuthCookie();
}
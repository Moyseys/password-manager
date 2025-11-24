export class CookieService {
  static getCookie(name: string): string | null {
    const cookies = document.cookie.split('; ');
    for (const cookie of cookies) {
      const [key, ...rest] = cookie.split('=');
      if (key === name) {
        return decodeURIComponent(rest.join('='));
      }
    }
    return null;
  }

  static setCookie(name: string, value: string, expires: string): void {
    document.cookie = `${name}=${encodeURIComponent(value)}; expires=${expires}; path=/`;
  }

  static deleteCookie(name: string): void {
    document.cookie = `${name}=; expires=Thu, 01 Jan 1970 00:00:00 GMT; path=/`;
  }

  static hasCookie(name: string): boolean {
    return this.getCookie(name) !== null;
  }
}

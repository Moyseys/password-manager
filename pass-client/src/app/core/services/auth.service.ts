import { inject, Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { CookieService } from './cookie.service';
import { ActivatedRoute } from '@angular/router';
import { BaseHttpClientApi } from '../apis/base-http-client.api';
import { AuthApi } from '../apis/Auth.api';

export interface LoginResponse {
  name: string;
  email: string;
  token: string;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService extends BaseHttpClientApi {
  private readonly activeRoute = inject(ActivatedRoute);
  private readonly authApi = inject(AuthApi);

  public token: null | string = null;

  get redirectUrl() {
    return this.activeRoute.snapshot.queryParams['redirect'] || '/';
  }

  get isLogged() {
    if (this.token) return true;
    const cookieToken = CookieService.getCookie(environment.cookies.token);
    if (cookieToken) {
      this.token = cookieToken;
      return true;
    }

    return false;
  }

  login(email: string, password: string) {
    this.authApi.login(email, password).subscribe({
      next: (res: LoginResponse) => {
        const dateExpires = new Date(Date.now());
        dateExpires.setHours(dateExpires.getHours() + 1);
        CookieService.setCookie(environment.cookies.token, res.token, dateExpires.toUTCString());
        this.token = res.token;
        window.location.href = this.redirectUrl;
      },
    });
  }
}

import { inject, Injectable } from '@angular/core';
import { BaseHttpClientService } from './base-http-client.service';
import { environment } from 'src/environments/environment';
import { CookieService } from './cookie.service';
import { ActivatedRoute } from '@angular/router';

interface LoginResponse {
  name: string;
  email: string;
  token: string;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService extends BaseHttpClientService {
  private readonly resource = `${environment.api.url}/pass-api/v1`;
  private readonly activeRoute = inject(ActivatedRoute);

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

  async login(email: string, password: string) {
    this.http.post<LoginResponse>(`${this.resource}/auth`, { email, password }).subscribe({
      next: (res: LoginResponse) => {
        //TODO alinhar com a API o tempo de expiração do token
        CookieService.setCookie(environment.cookies.token, res.token, 1);
        this.token = res.token;
        window.location.href = this.redirectUrl;
      },
    });
  }
}

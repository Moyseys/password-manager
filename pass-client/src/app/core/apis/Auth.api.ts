import { environment } from 'src/environments/environment';
import { BaseHttpClientApi } from './base-http-client.api';
import { LoginResponse } from '../services/auth.service';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AuthApi extends BaseHttpClientApi {
  private readonly resource = `${environment.api.url}/pass-api/v1/auth`;

  login(email: string, password: string) {
    return this.http.post<LoginResponse>(`${this.resource}`, { email, password });
  }
}

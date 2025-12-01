import { environment } from 'src/environments/environment';
import { BaseHttpClientApi } from './base-http-client.api';
import { Injectable } from '@angular/core';

export interface PayloadCreateUser {
  email: string;
  name: string;
  password: string;
  masterPassword: string;
}

@Injectable({
  providedIn: 'root',
})
export class UsersApi extends BaseHttpClientApi {
  private readonly resource = `${environment.api.url}/pass-api/v1/users`;

  register(payload: PayloadCreateUser) {
    return this.http.post(`${this.resource}`, payload);
  }
}

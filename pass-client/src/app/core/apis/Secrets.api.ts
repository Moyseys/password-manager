import { environment } from 'src/environments/environment';
import { Pageable } from '../models/pageable.model';
import { BaseHttpClientService } from '../services/base-http-client.service';
import { SecretListInterface } from '../interfaces/secretList.interface';
import { SecretInterface } from '../interfaces/secret.interface';

export class SecretsApi extends BaseHttpClientService {
  private readonly resource = `${environment.api.url}/pass-api/v1/secrets`;

  get(page?: number, size?: number, sort?: string) {
    const params: any = {
      sort: sort || 'title,asc',
    };
    if (page !== undefined) params.page = page;
    if (size !== undefined) params.size = size;
    return this.http.get<Pageable<SecretListInterface>>(this.resource, { params });
  }

  create(data: { title: string; username: string; password: string }, masterPassword: string) {
    const body = { ...data, masterPassword };
    return this.http.post(this.resource, body);
  }

  getById(secretId: string, masterPassword: string) {
    const body = { masterPassword };
    return this.http.post<SecretInterface>(`${this.resource}/${secretId}`, body);
  }

  update(
    secretId: string,
    data: { title?: string; username?: string; password?: string },
    masterPassword: string
  ) {
    const body = { ...data, masterPassword };
    return this.http.put<SecretInterface>(`${this.resource}/${secretId}`, body);
  }
}

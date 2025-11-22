import { HttpClient } from '@angular/common/http';
import { inject } from '@angular/core';

export class BaseHttpClientService {
  protected readonly http = inject(HttpClient);
}

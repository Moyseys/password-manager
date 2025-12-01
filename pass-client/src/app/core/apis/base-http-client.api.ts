import { HttpClient } from '@angular/common/http';
import { inject } from '@angular/core';

export class BaseHttpClientApi {
  protected readonly http = inject(HttpClient);
}

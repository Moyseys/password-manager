import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { CookieService } from '../services/cookie.service';
import { environment } from 'src/environments/environment';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  // Obtém o token do cookie
  const token = CookieService.getCookie(environment.cookies.token);

  // Se não houver token, continua com a requisição original
  if (!token) {
    return next(req);
  }

  // Clona a requisição e adiciona o header Authorization
  const authReq = req.clone({
    setHeaders: {
      Authorization: `Bearer ${token}`,
      'Content-Type': 'application/json',
    },
  });

  // Continua com a requisição modificada
  return next(authReq);
};

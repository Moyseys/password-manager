import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { ToastService } from '../services/toast.service';
import { CookieService } from '../services/cookie.service';
import { environment } from '../../../environments/environment';
import type { ApiErrorResponse } from '../interfaces/api-error-response.interface';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const toastService = inject(ToastService);
  const router = inject(Router);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      let errorMessage = 'Ocorreu um erro inesperado. Tente novamente.';
      let apiError: Partial<ApiErrorResponse> | null = null;

      // Verifica se a resposta da API contém a estrutura ApiErrorResponse
      if (error.error && typeof error.error === 'object') {
        apiError = error.error as Partial<ApiErrorResponse>;

        // Usa o detail da API se disponível
        if (apiError.detail) {
          errorMessage = apiError.detail;
        } else if (typeof error.error.message === 'string') {
          // Fallback para message se detail não estiver disponível
          errorMessage = error.error.message;
        }
      }

      // Tratamento específico por status code
      switch (error.status) {
        case 0:
          // Erro de rede ou servidor não acessível
          errorMessage = 'Não foi possível conectar ao servidor. Verifique sua conexão.';
          toastService.error(errorMessage);
          break;

        case 400:
          // Bad Request - usa mensagem da API ou genérica
          toastService.error(errorMessage);
          break;

        case 401:
          // Unauthorized - Token expirado ou inválido
          if (errorMessage.includes('expired') || errorMessage.includes('expirou')) {
            // Remove o token e redireciona para login
            CookieService.deleteCookie(environment.cookies.token);
            toastService.warning('Sua sessão expirou. Faça login novamente.');
            router.navigate(['/login']);
          } else {
            toastService.error(errorMessage);
            // Opcional: redirecionar para login para outros erros 401
            router.navigate(['/login']);
          }
          break;

        case 403:
          // Forbidden
          errorMessage = apiError?.detail || 'Você não tem permissão para acessar este recurso.';
          toastService.error(errorMessage);
          break;

        case 404:
          // Not Found
          toastService.error(errorMessage);
          break;

        case 500:
        case 502:
        case 503:
        case 504:
          // Server errors
          errorMessage = apiError?.detail || 'Erro no servidor. Tente novamente mais tarde.';
          toastService.error(errorMessage);
          break;

        default:
          // Outros erros
          toastService.error(errorMessage);
          break;
      }

      // Propaga o erro para que componentes possam tratar se necessário
      return throwError(() => error);
    })
  );
};

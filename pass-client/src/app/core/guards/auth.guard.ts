import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { inject } from '@angular/core';

export const authGuard: CanActivateFn = (route, statte) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.isLogged) {
    return true;
  }

  return router.createUrlTree(['/login'], {
    queryParams: { redirect: statte.url },
  });
};

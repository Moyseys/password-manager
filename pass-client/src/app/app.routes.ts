import { Routes } from '@angular/router';
import { MainNavigator } from './core/components/main-navigator/main-navigator';
import { Dashboard } from './pages/dashboard/dashboard';
import { LoginComponent } from './pages/login/login';
import { Passwords } from './pages/passwords/passwords';
import { Settings } from './pages/settings/settings';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent,
  },
  {
    path: '',
    component: MainNavigator,
    canActivate: [authGuard],
    children: [
      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full',
      },
      {
        path: 'dashboard',
        component: Dashboard,
      },
      {
        path: 'passwords',
        component: Passwords,
      },
      {
        path: 'settings',
        component: Settings,
      },
    ],
  },
];

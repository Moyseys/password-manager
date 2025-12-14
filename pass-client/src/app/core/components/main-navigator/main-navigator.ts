import { Component, signal, computed, ChangeDetectionStrategy, inject } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive, Router } from '@angular/router';
import { CookieService } from '../../services/cookie.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-main-navigator',
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './main-navigator.html',
  styleUrl: './main-navigator.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MainNavigator {
  private readonly router = inject(Router);
  private readonly _isSidebarOpen = signal(false);
  private readonly _appTitle = signal('Pass Manager');

  protected readonly isSidebarOpen = computed(() => this._isSidebarOpen());
  protected readonly appTitle = computed(() => this._appTitle());
  protected readonly currentYear = computed(() => new Date().getFullYear());

  toggleSidebar(): void {
    this._isSidebarOpen.update((isOpen) => !isOpen);
  }

  closeSidebar(): void {
    this._isSidebarOpen.set(false);
  }

  openSidebar(): void {
    this._isSidebarOpen.set(true);
  }

  logout(): void {
    CookieService.deleteCookie(environment.cookies.token);
    this.router.navigate(['/login']);
  }
}

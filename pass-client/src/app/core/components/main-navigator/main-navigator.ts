import { Component, signal, computed, ChangeDetectionStrategy } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  selector: 'app-main-navigator',
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './main-navigator.html',
  styleUrl: './main-navigator.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MainNavigator {
  private readonly _isSidebarOpen = signal(false);
  private readonly _appTitle = signal('Pass Client');

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
}

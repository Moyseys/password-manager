import { Component, ChangeDetectionStrategy, signal, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SecretsApi } from '../../core/apis/Secrets.api';
import { SecretListInterface } from '../../core/interfaces/secretList.interface';
import { SecretInterface } from '../../core/interfaces/secret.interface';
import { PasswordDetailModal } from './password-detail-modal/password-detail-modal';
import { PasswordCreate } from './password-create/password-create';

@Component({
  selector: 'app-passwords',
  imports: [CommonModule, PasswordDetailModal, PasswordCreate],
  templateUrl: './passwords.html',
  styleUrl: './passwords.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [SecretsApi],
})
export class Passwords implements OnInit {
  private readonly secretsApi = inject(SecretsApi);

  secrets = signal<SecretListInterface[]>([]);
  isLoading = signal(false);
  error = signal<string | null>(null);

  selectedSecret = signal<SecretInterface | null>(null);
  isModalOpen = signal(false);
  isLoadingDetails = signal(false);
  isCreateFormOpen = signal(false);
  openCreateForm() {
    this.isCreateFormOpen.set(true);
  }

  ngOnInit() {
    this.loadSecrets();
  }

  loadSecrets() {
    this.isLoading.set(true);
    this.error.set(null);

    this.secretsApi.get(1, 100, 'title,asc').subscribe({
      next: (response) => {
        this.secrets.set(response.items);
        this.isLoading.set(false);
      },
      error: (err) => {
        this.error.set('Erro ao carregar senhas');
        this.isLoading.set(false);
        console.error('Error loading secrets:', err);
      },
    });
  }

  openSecretDetails(secretId: string) {
    this.isLoadingDetails.set(true);
    this.isModalOpen.set(true);

    this.secretsApi.getById(secretId).subscribe({
      next: (secret) => {
        this.selectedSecret.set(secret);
        this.isLoadingDetails.set(false);
      },
      error: (err) => {
        console.error('Error loading secret details:', err);
        this.isLoadingDetails.set(false);
        this.closeModal();
      },
    });
  }

  closeModal() {
    this.isModalOpen.set(false);
    this.selectedSecret.set(null);
  }
}

import { Component, ChangeDetectionStrategy, signal, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SecretsApi } from '../../core/apis/Secrets.api';
import { SecretListInterface } from '../../core/interfaces/secretList.interface';
import { SecretInterface } from '../../core/interfaces/secret.interface';
import { PasswordDetailModal } from './password-detail-modal/password-detail-modal';
import { PasswordCreate } from './password-create/password-create';
import { PaginationComponent } from 'src/app/core/components/pagination/pagination.component';
import { ReactiveFormsModule, ɵInternalFormsSharedModule } from '@angular/forms';
import { PaginationUtils } from 'src/app/core/utils/pagination.util';

@Component({
  selector: 'app-passwords',
  imports: [
    CommonModule,
    PasswordDetailModal,
    PasswordCreate,
    PaginationComponent,
    ɵInternalFormsSharedModule,
    ReactiveFormsModule,
  ],
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

  pagination = new PaginationUtils();

  selectedSecret = signal<SecretInterface | null>(null);
  selectedSecretMaster = signal<string | null>(null);
  isModalOpen = signal(false);
  isLoadingDetails = signal(false);
  isCreateFormOpen = signal(false);

  openCreateForm() {
    this.isCreateFormOpen.set(true);
  }

  ngOnInit() {
    this.pagination.form.valueChanges.subscribe((pagination) => {
      console.log(pagination);
      if (!pagination) return;
      this.loadSecrets();
    });
    this.loadSecrets();
  }

  loadSecrets() {
    this.isLoading.set(true);
    this.error.set(null);

    const { page, size } = this.pagination.getValue();
    this.secretsApi.get(page, size, 'title,asc').subscribe({
      next: (response) => {
        this.pagination.setPeagleableValue(response);
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

    const master = window.prompt('Digite sua Master Password para visualizar a senha:');
    if (master === null) {
      // User cancelled the prompt
      this.isLoadingDetails.set(false);
      this.isModalOpen.set(false);
      return;
    }

    this.selectedSecretMaster.set(master);

    this.secretsApi.getById(secretId, master).subscribe({
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
    this.selectedSecretMaster.set(null);
  }

  onSecretUpdated(secret: SecretInterface) {
    // Update local selection and refresh list
    this.selectedSecret.set(secret);
    this.loadSecrets();
  }
}

import {
  Component,
  ChangeDetectionStrategy,
  input,
  output,
  signal,
  inject,
  effect,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SecretInterface } from '../../../core/interfaces/secret.interface';
import { SecretsApi } from '../../../core/apis/Secrets.api';

@Component({
  selector: 'app-password-detail-modal',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './password-detail-modal.html',
  styleUrl: './password-detail-modal.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PasswordDetailModal {
  secret = input.required<SecretInterface>();
  masterPassword = input<string | null>();
  close = output<void>();
  updated = output<SecretInterface>();

  private readonly secretsApi = inject(SecretsApi);
  private readonly fb = inject(FormBuilder);

  editing = signal(false);
  isSaving = signal(false);
  copyMessage = signal<string | null>(null);

  showPassword = signal(false);

  form: FormGroup = this.fb.group({
    title: ['', Validators.required],
    userName: ['', Validators.required],
    password: ['', Validators.required],
  });

  get passwordInputType(): string {
    return this.showPassword() ? 'text' : 'password';
  }

  togglePasswordVisibility() {
    this.showPassword.set(!this.showPassword());
  }

  private _patchEffect = effect(() => {
    const s = this.secret?.();
    if (s) {
      this.form.patchValue({ title: s.title, userName: s.userName, password: s.password });
      this.form.disable();
      this.editing.set(false);
    }
  });

  onClose() {
    this.close.emit();
  }

  enterEdit() {
    this.form.enable();
    this.editing.set(true);
  }

  cancelEdit() {
    // revert to original secret values
    const s = this.secret();
    if (s) {
      this.form.patchValue({ title: s.title, userName: s.userName, password: s.password });
    }
    this.form.disable();
    this.editing.set(false);
  }

  save() {
    const s = this.secret();
    if (!s) return;

    let master = this.masterPassword?.() ?? null;
    if (!master) {
      master = window.prompt('Digite sua Master Password para salvar alterações:');
      if (master === null) {
        return; // user cancelled
      }
    }

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.isSaving.set(true);

    const payload = {
      title: this.form.get('title')!.value,
      username: this.form.get('userName')!.value,
      password: this.form.get('password')!.value,
    } as { title?: string; username?: string; password?: string };

    this.secretsApi.update(s.id, payload, master).subscribe({
      next: (updated) => {
        this.isSaving.set(false);
        this.form.disable();
        this.editing.set(false);
        this.updated.emit(updated);
      },
      error: (err) => {
        console.error('Error saving secret:', err);
        this.isSaving.set(false);
        this.copyMessage.set('Erro ao salvar');
        setTimeout(() => this.copyMessage.set(null), 3000);
      },
    });
  }

  async copyToClipboard(text: string) {
    if (!text) return;
    try {
      if (navigator.clipboard && navigator.clipboard.writeText) {
        await navigator.clipboard.writeText(text);
        this.copyMessage.set('Copiado');
        setTimeout(() => this.copyMessage.set(null), 1500);
        return;
      }
    } catch (err) {
      console.warn('Clipboard API failed, falling back', err);
    }

    // Fallback: temporary textarea + execCommand
    try {
      const textarea = document.createElement('textarea');
      textarea.value = text;
      textarea.style.position = 'fixed';
      textarea.style.left = '-9999px';
      document.body.appendChild(textarea);
      textarea.focus();
      textarea.select();
      const successful = document.execCommand('copy');
      document.body.removeChild(textarea);
      if (successful) {
        this.copyMessage.set('Copiado');
      } else {
        this.copyMessage.set('Falha ao copiar');
      }
      setTimeout(() => this.copyMessage.set(null), 1500);
    } catch (err) {
      console.error('Fallback copy failed', err);
      this.copyMessage.set('Falha ao copiar');
      setTimeout(() => this.copyMessage.set(null), 1500);
    }
  }
}

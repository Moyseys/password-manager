import { Component, ChangeDetectionStrategy, output, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { SecretsApi } from '../../../core/apis/Secrets.api';
import { ZardCardComponent } from '@shared/components/card/card.component';

@Component({
  selector: 'app-password-create',
  imports: [CommonModule, ReactiveFormsModule, ZardCardComponent],
  templateUrl: './password-create.html',
  styleUrls: ['./password-create.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PasswordCreate {
  onKeyDown(event: KeyboardEvent) {
    if (event.key === 'Escape') {
      this.close.emit();
    }
  }
  close = output<void>();
  created = output<void>();
  private readonly fb = inject(FormBuilder);
  private readonly secretsApi = inject(SecretsApi);

  form = this.fb.group({
    title: ['', Validators.required],
    username: ['', Validators.required],
    password: ['', Validators.required],
  });

  isLoading = false;
  error: string | null = null;
  success: boolean = false;

  submit() {
    if (this.form.invalid) return;
    this.isLoading = true;
    this.error = null;
    this.success = false;
    const { title, username, password } = this.form.value;
    const master = window.prompt('Digite sua Master Password para salvar o segredo:');
    if (master === null) return; // user cancelled prompt

    this.isLoading = true;

    this.secretsApi
      .create(
        {
          title: title ?? '',
          username: username ?? '',
          password: password ?? '',
        },
        master
      )
      .subscribe({
        next: () => {
          this.isLoading = false;
          this.success = true;
          this.form.reset();
          window.alert('Senha cadastrada com sucesso!');
          this.created.emit();
        },
        error: (err) => {
          this.isLoading = false;
          this.error = 'Erro ao cadastrar senha';
          console.error(err);
          window.alert('Erro ao cadastrar senha. Tente novamente.');
        },
      });
  }
}

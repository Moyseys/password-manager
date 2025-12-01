import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

import { ZardButtonComponent } from '@shared/components/button/button.component';
import { ZardCardComponent } from '@shared/components/card/card.component';
import { ZardInputDirective } from '@shared/components/input/input.directive';
import { PayloadCreateUser, UsersApi } from 'src/app/core/apis/Users.api';

@Component({
  selector: 'app-register',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    ReactiveFormsModule,
    ZardButtonComponent,
    ZardCardComponent,
    ZardInputDirective,
    RouterLink,
  ],
  templateUrl: './register.html',
  styleUrls: ['./register.scss'],
})
export class RegisterComponent {
  private readonly fb = inject(FormBuilder);
  private readonly router = inject(Router);
  private readonly usersApi = inject(UsersApi);

  readonly registerForm = this.fb.group({
    name: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]],
    masterPassword: ['', [Validators.required, Validators.minLength(6)]],
  });

  loading = false;
  errorMsg = '';

  get formValue(): PayloadCreateUser {
    return {
      name: this.registerForm.controls.name.value,
      email: this.registerForm.controls.email.value,
      password: this.registerForm.controls.password.value,
      masterPassword: this.registerForm.controls.masterPassword.value,
    } as PayloadCreateUser;
  }

  onSubmit(): void {
    if (this.registerForm.valid) {
      this.loading = true;
      this.errorMsg = '';
      this.usersApi.register(this.formValue).subscribe({
        next: () => {
          this.loading = false;
          this.router.navigate(['/login']);
        },
        error: (err: any) => {
          this.loading = false;
          this.errorMsg = err?.error?.message || 'Erro ao cadastrar';
        },
      });
    } else {
      this.registerForm.markAllAsTouched();
    }
  }

  get name() {
    return this.registerForm.get('name');
  }
  get email() {
    return this.registerForm.get('email');
  }
  get password() {
    return this.registerForm.get('password');
  }
  get masterPassword() {
    return this.registerForm.get('masterPassword');
  }
}

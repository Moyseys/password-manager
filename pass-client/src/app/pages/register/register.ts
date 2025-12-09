import { NgClass } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  ReactiveFormsModule,
  ValidationErrors,
  Validators,
} from '@angular/forms';
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
    NgClass,
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

  showPassword = false;
  showMasterPassword = false;
  showConfirmPassword = false;

  readonly registerForm = this.fb.group(
    {
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required]],
      masterPassword: ['', [Validators.required, Validators.minLength(6)]],
      confirmMasterPassword: ['', [Validators.required]],
    },
    {
      validators: [this.passwordMatchValidator, this.masterPasswordMatchValidator],
    }
  );

  loading = false;
  errorMsg = '';

  private passwordMatchValidator(control: AbstractControl): ValidationErrors | null {
    const password = control.get('password')?.value;
    const confirmPassword = control.get('confirmPassword')?.value;

    if (password && confirmPassword && password !== confirmPassword) {
      return { passwordMismatch: true };
    }
    return null;
  }

  private masterPasswordMatchValidator(control: AbstractControl): ValidationErrors | null {
    const masterPassword = control.get('masterPassword')?.value;
    const confirmMasterPassword = control.get('confirmMasterPassword')?.value;

    if (masterPassword && confirmMasterPassword && masterPassword !== confirmMasterPassword) {
      return { masterPasswordMismatch: true };
    }
    return null;
  }

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  toggleMasterPasswordVisibility(): void {
    this.showMasterPassword = !this.showMasterPassword;
  }

  toggleConfirmPasswordVisibility(): void {
    this.showConfirmPassword = !this.showConfirmPassword;
  }

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
  get confirmPassword() {
    return this.registerForm.get('confirmPassword');
  }
  get masterPassword() {
    return this.registerForm.get('masterPassword');
  }
  get confirmMasterPassword() {
    return this.registerForm.get('confirmMasterPassword');
  }

  get hasPasswordMismatch(): boolean {
    return (
      (this.registerForm.hasError('passwordMismatch') &&
        (this.confirmPassword?.dirty || this.confirmPassword?.touched)) ||
      false
    );
  }

  get hasMasterPasswordMismatch(): boolean {
    return (
      (this.registerForm.hasError('masterPasswordMismatch') &&
        (this.confirmMasterPassword?.dirty || this.confirmMasterPassword?.touched)) ||
      false
    );
  }
}

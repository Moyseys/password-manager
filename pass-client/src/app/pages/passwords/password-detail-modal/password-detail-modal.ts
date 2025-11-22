import { Component, ChangeDetectionStrategy, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SecretInterface } from '../../../core/interfaces/secret.interface';

@Component({
  selector: 'app-password-detail-modal',
  imports: [CommonModule],
  templateUrl: './password-detail-modal.html',
  styleUrl: './password-detail-modal.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PasswordDetailModal {
  secret = input.required<SecretInterface>();
  close = output<void>();

  onClose() {
    this.close.emit();
  }

  copyToClipboard(text: string) {
    navigator.clipboard.writeText(text);
  }
}

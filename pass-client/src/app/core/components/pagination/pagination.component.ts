import { CommonModule } from '@angular/common';
import { Component, forwardRef, input, signal } from '@angular/core';
import {
  ControlValueAccessor,
  NG_VALUE_ACCESSOR,
  ɵInternalFormsSharedModule,
} from '@angular/forms';

export interface IPagination {
  page: number;
  size: number;
  totalItems: number;
  totalPages: number;
  sort: string;
}

export const paginationInitialState: IPagination = {
  page: 1,
  size: 12,
  totalItems: 0,
  totalPages: 0,
  sort: '',
};

export const sizeOptions = [6, 12, 24, 32];

@Component({
  selector: 'app-pagination',
  imports: [ɵInternalFormsSharedModule, CommonModule],
  templateUrl: './pagination.html',
  styleUrl: './pagination.scss',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => PaginationComponent),
      multi: true,
    },
  ],
})
export class PaginationComponent implements ControlValueAccessor {
  paginationState = signal<IPagination>(paginationInitialState);
  sizeOptions = sizeOptions;

  _onChange!: (value: IPagination) => void;
  _onTouched!: (value: IPagination) => void;
  _disabled: boolean = false;

  writeValue(value: IPagination): void {
    if (!value) return;

    this.paginationState.set(value);
  }

  registerOnChange(fn: (value: IPagination) => void): void {
    this._onChange = fn;
  }

  registerOnTouched(fn: (value: IPagination) => void): void {
    this._onTouched = fn;
  }

  setDisabledState?(isDisabled: boolean): void {
    this._disabled = isDisabled;
  }

  set page(page: number) {
    const newValue = { ...this.paginationState(), page };
    this._onChange(newValue);
    this.writeValue(newValue);
  }

  get page() {
    return this.paginationState().page;
  }

  nextPage() {
    const page = this.page;
    if (!page || this._disabled || page >= this.paginationState().totalPages) return;

    this.page = page + 1;
  }

  previusPage() {
    const page = this.page;
    if (!page || this._disabled || page <= 1) return;

    this.page = page - 1;
  }

  changeSize(size: number) {
    const current = this.paginationState();
    if (this._disabled || !current) return;

    const newTotalPages = Math.ceil(current.totalItems / size);
    const newPage = Math.min(current.page, newTotalPages);
    const newValue = { ...current, size, totalPages: newTotalPages, page: newPage };
    this._onChange(newValue);
    this.writeValue(newValue);
  }

  goToPage(page: number) {
    const current = this.paginationState();
    if (this._disabled || !current) return;
    const newPage = Math.max(1, Math.min(page, current.totalPages || 1));
    const newValue = { ...current, page: newPage };
    this._onChange(newValue);
    this.writeValue(newValue);
  }
}

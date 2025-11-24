import { FormControl } from '@angular/forms';
import { IPagination, paginationInitialState } from '../components/pagination/pagination.component';
import { Pageable } from '../models/pageable.model';

export class PaginationUtils {
  form = new FormControl<IPagination>({
    page: paginationInitialState.page,
    size: paginationInitialState.size,
    totalItems: paginationInitialState.totalItems,
    totalPages: paginationInitialState.totalPages,
    sort: '',
  });

  setValue(value: IPagination, emitEvent: boolean = true) {
    this.form.setValue(value, { emitEvent: emitEvent });
  }

  setPeagleableValue<T>(value: Pageable<T>, emitEvent: boolean = false) {
    this.setValue(this.trasformPageableToPagination<T>(value), emitEvent);
  }

  getValue(): IPagination {
    const { page, size, totalItems, totalPages, sort } = this.form.value ?? paginationInitialState;
    return {
      page,
      size,
      totalItems,
      totalPages,
      sort: sort ?? '',
    };
  }

  trasformPageableToPagination<T = any>(value: Pageable<T>): IPagination {
    return {
      page: value.page,
      size: value.size,
      totalItems: value.totalItems,
      totalPages: value.totalPages,
      sort: '',
    };
  }
}

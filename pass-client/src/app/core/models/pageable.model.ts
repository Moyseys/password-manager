export interface Pageable<T> {
  items: T[];
  page: number;
  size: number;
  totalItems: number;
  totalPages: number;
}

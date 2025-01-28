export interface PagedResponseModel<T> {
  totalRecords: number;
  pageNumber: number;
  pageSize: number;
  data: T[]
}

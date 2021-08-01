export interface SuccessResponse<T> {
  data: T;
}

export interface ErrorModel {
  fieldName: string;
  message: string;
}

export interface ErrorResponse {
  errors: ErrorModel[];
}

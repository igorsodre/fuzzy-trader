export interface SignupRequest {
  name: string;
  email: string;
  password: string;
  confirmedPassword: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface UpdateUserRequest {
  newEmail: string;
  password: string;
  newPassword: string;
}

export interface ResetPasswordRequest {
  email: string;
  token: string;
  newPassword: string;
  confirmedPassword: string;
}

export interface ForgotPasswordRequest {
  email: string;
}

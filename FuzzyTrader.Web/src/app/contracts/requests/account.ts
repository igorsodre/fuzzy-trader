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
  oldPassword: string;
  newPassword: string;
  confirmedPassword: string;
  name: string;
}

export interface ForgotPasswordRequest {
  email: string;
}

export interface RecoverPasswordRequest {
  email: string;
  token: string;
  password: string;
  confirmedPassword: string;
}

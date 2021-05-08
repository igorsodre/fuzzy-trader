import { AppUser } from './../../models/user-interface';

export interface LoginResponse {
  accessToken: string;
  user: AppUser;
}

export interface RefreshTokenResponse {
  accessToken: string;
}

export interface SignupResponse {
  accessToken: string;
}

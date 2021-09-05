import { AppUser } from '../../models/app-user';

export interface LoginResponse {
  accessToken: string;
  user: AppUser;
}

export interface RefreshTokenResponse {
  accessToken: string;
  user: AppUser;
}

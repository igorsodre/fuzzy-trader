import { AppUser } from './../data/models/user-interface';
import { GET_BASE_URL } from './../data/constants';
import { useHttp, __ } from './base.http';
import { LoginResponse, RefreshTokenResponse } from '../data/contracts/responses/account';
import { SuccessResponse } from '../data/contracts/responses/default-responses';

export type AuthService = {
  errorText: Nullable<string>;
  isLoadding: boolean;
  clearError: () => void;
  refreshToken: () => Promise<RefreshTokenResponse>;
  register: (name: string, email: string, password: string) => Promise<string>;
  updateUser: (name: string, email: string, password: string, newpassword: string) => Promise<AppUser>;
  login: (
    email: string,
    password: string,
  ) => Promise<{
    accessToken: string;
    user: AppUser;
  }>;
  logout: () => Promise<string>;
  getLoggedUser: () => Promise<AppUser>;
};
export const useAuth = (): AuthService => {
  const { errorText, isLoadding, clearError, post, get } = useHttp();

  const register: AuthService['register'] = async (name, email, password) => {
    const endpoint = GET_BASE_URL() + '/api/account/signup';
    const body = {
      name,
      email,
      password,
    };
    return post<{ data: string }>(endpoint, body).then((res) => res.data);
  };

  const updateUser: AuthService['updateUser'] = async (name, email, password, newpassword) => {
    const endpoint = GET_BASE_URL() + '/api/account/update';
    const body = {
      name,
      email,
      password,
      newpassword,
    };
    return post<{ data: AppUser }>(endpoint, body).then((res) => res.data);
  };

  const login: AuthService['login'] = async (email, password) => {
    const endpoint = GET_BASE_URL() + '/api/account/login';
    const body = {
      email,
      password,
    };
    return post<SuccessResponse<LoginResponse>>(endpoint, body).then((res) => res.data);
  };

  const logout: AuthService['logout'] = async () => {
    const endpoint = GET_BASE_URL() + '/api/account/logout';

    return post<SuccessResponse<string>>(endpoint, {}).then((res) => res.data);
  };

  const refreshToken: AuthService['refreshToken'] = async () => {
    const endpoint = GET_BASE_URL() + '/api/account/refresh_token';
    return post<SuccessResponse<RefreshTokenResponse>>(endpoint, {}).then((res) => res.data);
  };

  const getLoggedUser: AuthService['getLoggedUser'] = async () => {
    const endpoint = GET_BASE_URL() + '/api/account';

    return get<{ data: AppUser }>(endpoint).then((res) => res.data);
  };

  return {
    errorText,
    isLoadding,
    register: __(register),
    updateUser: __(updateUser),
    login: __(login),
    clearError: __(clearError),
    getLoggedUser: __(getLoggedUser),
    refreshToken: __(refreshToken),
    logout: __(logout),
  };
};

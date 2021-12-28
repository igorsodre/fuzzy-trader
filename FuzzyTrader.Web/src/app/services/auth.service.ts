import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { environment } from 'src/environments/environment';
import {
  ForgotPasswordRequest,
  LoginRequest,
  RecoverPasswordRequest,
  SignupRequest,
  UpdateUserRequest,
} from '../contracts/requests/account';
import { LoginResponse, RefreshTokenResponse } from '../contracts/responses/account';
import { SuccessResponse } from '../contracts/responses/default-responses';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly urlPrefix = environment.baseUrl + '/api/account';

  constructor(private http: HttpClient) {}

  async login(requestBody: LoginRequest): Promise<LoginResponse> {
    const endpoint = this.urlPrefix + '/login';
    const result = await firstValueFrom(
      this.http.post<SuccessResponse<LoginResponse>>(endpoint, requestBody, { withCredentials: true }),
    );

    return result.data;
  }

  async logout() {
    const endpoint = this.urlPrefix + '/logout';
    const result = await firstValueFrom(
      this.http.post<SuccessResponse<string>>(endpoint, {}, { withCredentials: true }),
    );
    return result.data;
  }

  async register(requestBody: SignupRequest) {
    const endpoint = this.urlPrefix + '/signup';
    const result = await firstValueFrom(this.http.post<SuccessResponse<string>>(endpoint, requestBody));
    return result.data;
  }

  async updateUser(requestBody: UpdateUserRequest) {
    const endpoint = this.urlPrefix + '/update';
    const result = await firstValueFrom(this.http.post<SuccessResponse<string>>(endpoint, requestBody));
    return result.data;
  }

  async notifyPasswordForgoten(requestBody: ForgotPasswordRequest) {
    const endpoint = this.urlPrefix + '/forgot-password';
    const result = await firstValueFrom(this.http.post<SuccessResponse<string>>(endpoint, requestBody));
    return result.data;
  }

  async recoverPassword(requestBody: RecoverPasswordRequest) {
    const endpoint = this.urlPrefix + '/recover-password';
    const result = await firstValueFrom(this.http.post<SuccessResponse<string>>(endpoint, requestBody));
    return result.data;
  }

  async refreshToken(): Promise<RefreshTokenResponse | null> {
    try {
      const endpoint = this.urlPrefix + '/refresh-token';
      const response = await fetch(endpoint, { credentials: 'include', method: 'POST' });
      if (response.ok) {
        const result = await response.json();
        return result.data;
      }
      return null;
    } catch (err) {
      return null;
    }
  }
}

import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { firstValueFrom, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
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

  login(requestBody: LoginRequest) {
    const endpoint = this.urlPrefix + '/login';

    return firstValueFrom(
      this.http
        .post<SuccessResponse<LoginResponse>>(endpoint, requestBody, { withCredentials: true })
        .pipe(map((res) => res.data)),
    );
  }

  logout() {
    const endpoint = this.urlPrefix + '/logout';
    return firstValueFrom(
      this.http.post<SuccessResponse<string>>(endpoint, {}, { withCredentials: true }).pipe(map((res) => res.data)),
    );
  }

  register(requestBody: SignupRequest) {
    const endpoint = this.urlPrefix + '/signup';
    return firstValueFrom(this.http.post<SuccessResponse<string>>(endpoint, requestBody).pipe(map((res) => res.data)));
  }

  updateUser(requestBody: UpdateUserRequest) {
    const endpoint = this.urlPrefix + '/update';
    return firstValueFrom(this.http.post<SuccessResponse<string>>(endpoint, requestBody).pipe(map((res) => res.data)));
  }

  notifyPasswordForgoten(requestBody: ForgotPasswordRequest) {
    const endpoint = this.urlPrefix + '/forgot-password';
    return firstValueFrom(this.http.post<SuccessResponse<string>>(endpoint, requestBody).pipe(map((res) => res.data)));
  }

  async recoverPassword(requestBody: RecoverPasswordRequest) {
    const endpoint = this.urlPrefix + '/recover-password';
    return await firstValueFrom(
      this.http.post<SuccessResponse<string>>(endpoint, requestBody).pipe(map((res) => res.data)),
    );
  }

  refreshToken() {
    const endpoint = this.urlPrefix + '/refresh-token';

    return firstValueFrom(
      this.http.post<SuccessResponse<RefreshTokenResponse>>(endpoint, {}, { withCredentials: true }).pipe(
        map((res) => res.data),
        catchError(() => of(null)),
      ),
    );
  }
}

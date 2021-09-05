import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { LoginRequest, SignupRequest } from '../contracts/requests/account';
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
    const result = await this.http.post<SuccessResponse<LoginResponse>>(endpoint, requestBody).toPromise();
    return result.data;
  }

  async logout() {
    const endpoint = this.urlPrefix + '/logout';
    const result = await this.http.post<SuccessResponse<string>>(endpoint, {}).toPromise();
    return result.data;
  }

  async register(requestBody: SignupRequest) {
    const endpoint = this.urlPrefix + '/signup';
    const result = await this.http.post<SuccessResponse<string>>(endpoint, requestBody).toPromise();
    return result.data;
  }

  async updateUser() {
    const endpoint = this.urlPrefix + '/update';
  }

  async refreshToken(): Promise<RefreshTokenResponse> {
    const endpoint = this.urlPrefix + '/refresh-token';
    const result = await this.http
      .post<SuccessResponse<RefreshTokenResponse>>(endpoint, {}, { withCredentials: true })
      .toPromise();
    return result.data;
  }

  async getLoggedUser() {}
}
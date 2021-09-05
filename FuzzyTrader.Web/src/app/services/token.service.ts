import { RefreshTokenResponse } from './../../../old/src/data/contracts/responses/account';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import jwtDecode, { JwtPayload } from 'jwt-decode';
import { environment } from 'src/environments/environment';
import { SuccessResponse } from '../contracts/responses/default-responses';

@Injectable({
  providedIn: 'root',
})
export class TokenService {
  private accessToken: string;

  constructor(private http: HttpClient) {
    this.accessToken = '';
  }

  public setAccessToken(token: string) {
    this.accessToken = token;
  }

  public async getAccessToken(): Promise<string> {
    if (!this.isValidToken()) {
      this.setAccessToken(await this.fetchToken());
    }
    return this.accessToken;
  }

  private async fetchToken(): Promise<string> {
    try {
      const endpoint = environment.baseUrl + '/api/account/refresh-token';
      const result = await this.http
        .post<SuccessResponse<RefreshTokenResponse>>(endpoint, {}, { withCredentials: true })
        .toPromise();
      return result.data.accessToken;
    } catch (err) {
      return '';
    }
  }

  private isValidToken(): boolean {
    if (!this.accessToken) return false;
    const { exp } = jwtDecode<JwtPayload>(this.accessToken);
    if (!exp || Date.now() >= exp * 1000) return false;
    return true;
  }
}

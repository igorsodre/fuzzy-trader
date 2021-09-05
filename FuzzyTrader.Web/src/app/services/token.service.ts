import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import jwtDecode, { JwtPayload } from 'jwt-decode';
import { BehaviorSubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { RefreshTokenResponse } from '../contracts/responses/account';
import { SuccessResponse } from '../contracts/responses/default-responses';

@Injectable({
  providedIn: 'root',
})
export class TokenService {
  private readonly _accessTokenSource = new BehaviorSubject<string>('');
  readonly accessToken$ = this._accessTokenSource.asObservable();

  constructor(private http: HttpClient) {}

  setAccessToken(token: string) {
    this._accessTokenSource.next(token);
  }

  async getAccessToken(): Promise<string> {
    if (!this.isValidToken()) {
      this.setAccessToken(await this.fetchToken());
    }
    return this._accessTokenSource.getValue();
  }

  isAuthenticated(): boolean {
    return !!this._accessTokenSource.getValue();
  }

  private async fetchToken(): Promise<string> {
    try {
      const endpoint = environment.baseUrl + '/api/account/refresh-token';
      const response = await fetch(endpoint, { credentials: 'include', method: 'POST' });
      if (response.ok) {
        const result = await response.json();
        console.log('loggin result');
        return result.data;
      }
      return '';
    } catch (err) {
      return '';
    }
  }

  private isValidToken(): boolean {
    if (!this._accessTokenSource.getValue()) return false;
    const { exp } = jwtDecode<JwtPayload>(this._accessTokenSource.getValue());
    if (!exp || Date.now() >= exp * 1000) return false;
    return true;
  }
}

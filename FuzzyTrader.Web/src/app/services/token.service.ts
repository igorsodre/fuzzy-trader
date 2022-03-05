import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import jwtDecode, { JwtPayload } from 'jwt-decode';
import { BehaviorSubject, firstValueFrom } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
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

  resetToken() {
    this._accessTokenSource.next('');
  }

  async getAccessToken() {
    if (!this.isValidToken()) {
      this.setAccessToken(await this.fetchToken());
    }
    return this._accessTokenSource.getValue();
  }

  isAuthenticated() {
    return !!this._accessTokenSource.getValue();
  }

  private fetchToken() {
    const endpoint = environment.baseUrl + '/api/account/refresh-token';
    return firstValueFrom(
      this.http.post<SuccessResponse<string>>(endpoint, {}, { withCredentials: true }).pipe(
        map((response) => response.data),
        catchError((_) => ''),
      ),
    );
  }

  private isValidToken() {
    if (!this._accessTokenSource.getValue()) return false;
    const { exp } = jwtDecode<JwtPayload>(this._accessTokenSource.getValue());
    if (!exp || Date.now() >= exp * 1000) return false;
    return true;
  }
}

import { TokenService } from './token.service';
import { Injectable } from '@angular/core';
import { AuthStoreService } from '../stores/auth-store.service';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class InitializerService {
  constructor(
    private authStore: AuthStoreService,
    private authService: AuthService,
    private tokenService: TokenService,
  ) {}

  async startup() {
    console.log('===========================\nStarting up application\n=============================\n\n');
    try {
      const result = await this.authService.refreshToken();
      if (result && result.accessToken) {
        this.authStore.setUser(result.user);
        this.tokenService.setAccessToken(result.accessToken);
      }
    } catch {
      console.log('Error starting application');
    }
  }
}

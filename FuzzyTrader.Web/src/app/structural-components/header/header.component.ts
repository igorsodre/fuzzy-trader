import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { TokenService } from 'src/app/services/token.service';
import { AuthStoreService } from 'src/app/stores/auth-store.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
})
export class HeaderComponent implements OnInit {
  constructor(
    private authService: AuthService,
    private authStore: AuthStoreService,
    public tokenService: TokenService,
    private router: Router,
  ) {}

  ngOnInit(): void {}

  async logout() {
    try {
      await this.authService.logout();
      this.tokenService.resetToken();
      this.authStore.resetUser();
      this.router.navigate(['']);
    } catch (err) {
      console.log('Failed to logout?');
      console.log(err);
    }
  }
}

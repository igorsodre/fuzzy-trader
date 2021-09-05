import { AuthStoreService } from './stores/auth-store.service';
import { Component, OnInit } from '@angular/core';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  title = 'FuzzyTrader';
  constructor(authStore: AuthStoreService, authService: AuthService) {}
  public ngOnInit(): void {
    // throw new Error('Method not implemented.');
  }
}

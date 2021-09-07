import { Component, OnInit } from '@angular/core';
import { AuthStoreService } from './../../stores/auth-store.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  constructor(private authStore: AuthStoreService) {}

  ngOnInit(): void {}
}

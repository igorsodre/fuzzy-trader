import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/services/auth.service';
import { TokenService } from './../../services/token.service';
import { AuthStoreService } from './../../stores/auth-store.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss'],
})
export class ForgotPasswordComponent implements OnInit {
  constructor(
    private authService: AuthService,
    private authStore: AuthStoreService,
    private tokenService: TokenService,
    private router: Router,
    private toastr: ToastrService,
  ) {
    this.form = new FormGroup({
      email: new FormControl('dotnettest1@localhost.com', [Validators.required, Validators.email]),
    });
  }

  ngOnInit(): void {}

  form: FormGroup;

  get email() {
    return this.form.get('email');
  }

  async submit() {}
}

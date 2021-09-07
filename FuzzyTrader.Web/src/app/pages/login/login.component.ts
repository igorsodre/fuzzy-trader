import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../services/auth.service';
import { TokenService } from '../../services/token.service';
import { AuthStoreService } from '../../stores/auth-store.service';
import { ErrorModel } from './../../contracts/responses/default-responses';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  constructor(
    private authService: AuthService,
    private authStore: AuthStoreService,
    private tokenService: TokenService,
    private router: Router,
    private toastr: ToastrService,
  ) {
    this.form = new FormGroup({
      email: new FormControl('dotnettest1@localhost.com', [Validators.required, Validators.email]),
      password: new FormControl('Password!1', [Validators.required, Validators.minLength(6)]),
    });
  }

  ngOnInit(): void {}

  form: FormGroup;

  get email() {
    return this.form.get('email');
  }

  get password() {
    return this.form.get('password');
  }

  async submit() {
    if (this.form.invalid) return;
    try {
      const result = await this.authService.login({ email: this.email!.value, password: this.password!.value });
      if (!result) return;
      this.authStore.setUser(result.user);
      this.tokenService.setAccessToken(result.accessToken);
      this.toastr.success('Login Success');
      this.router.navigate(['/home']);
    } catch (err: any) {
      this.toastr.error('Failed to login');
      if (err?.error?.errors) {
        err?.error?.errors.forEach((entry: ErrorModel) => {
          this.toastr.error(entry.message);
        });
      }
    }
  }
}

import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { TokenService } from 'src/app/services/token.service';
import { AuthStoreService } from 'src/app/stores/auth-store.service';

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
  ) {
    this.form = new FormGroup({
      email: new FormControl('dotnettest1@localhost.com', [Validators.required, Validators.email]),
      password: new FormControl('Password!1', [Validators.required, Validators.minLength(6)]),
    });
  }

  ngOnInit(): void {}

  form: FormGroup = new FormGroup({});

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
      console.log('loging result');
      console.log(result);
      if (!result) return;
      this.authStore.setUser(result.user);
      this.tokenService.setAccessToken(result.accessToken);
      this.router.navigate(['/home']);
    } catch (err) {
      console.log('Error during login');
      console.log(err);
    }
  }
}

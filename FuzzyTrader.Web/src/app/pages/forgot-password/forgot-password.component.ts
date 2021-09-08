import { ErrorModel } from './../../contracts/responses/default-responses';
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

  async submit() {
    if (this.form.invalid) return;
    try {
      await this.authService.notifyPasswordForgoten({ email: this.email!.value });
      this.toastr.info('An an email was sent for you to recover your account');
      this.router.navigate(['']);
    } catch (err: any) {
      this.toastr.error('something went wrong');
      if (err?.error?.errors) {
        err?.error?.errors.forEach((entry: ErrorModel) => {
          this.toastr.error(entry.message);
        });
      }
    }
  }
}

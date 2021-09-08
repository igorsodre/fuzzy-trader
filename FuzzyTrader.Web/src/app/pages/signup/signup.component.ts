import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ErrorModel } from '../../contracts/responses/default-responses';
import { matchFields } from '../../custom-validators/match-fields';
import { AuthService } from '../../services/auth.service';
import { TokenService } from '../../services/token.service';
import { AuthStoreService } from '../../stores/auth-store.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.scss'],
})
export class SignupComponent implements OnInit {
  constructor(
    private authService: AuthService,
    private authStore: AuthStoreService,
    private tokenService: TokenService,
    private router: Router,
    private toastr: ToastrService,
  ) {
    this.form = new FormGroup(
      {
        email: new FormControl('dotnettest1@localhost.com', Validators.email),
        name: new FormControl('john Doe', [
          Validators.required,
          Validators.minLength(2),
          Validators.pattern(/^(\s+\S+\s*)*(?!\s).*$/),
        ]),
        password: new FormControl('Password!1', [Validators.required, Validators.minLength(6)]),
        confirmPassword: new FormControl('Password!1', [Validators.required, Validators.minLength(6)]),
      },
      { validators: matchFields('password', 'confirmPassword') },
    );
  }

  ngOnInit(): void {}

  form: FormGroup;

  get name() {
    return this.form.get('name');
  }

  get email() {
    return this.form.get('email');
  }

  get password() {
    return this.form.get('password');
  }

  get confirmPassword() {
    return this.form.get('confirmPassword');
  }

  async submit() {
    if (this.form.invalid) return;
    try {
      const result = await this.authService.register({
        email: this.email!.value,
        password: this.password!.value,
        confirmedPassword: this.confirmPassword!.value,
        name: this.name!.value,
      });
      this.toastr.info('Check you email to confirm the signup process');
      this.router.navigate(['']);
    } catch (err: any) {
      this.toastr.error('Failed to create new account');
      if (err?.error?.errors) {
        err?.error?.errors.forEach((entry: ErrorModel) => {
          this.toastr.error(entry.message);
        });
      }
    }
  }
}

import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ErrorModel } from '../../contracts/responses/default-responses';
import { matchFields } from '../../custom-validators/match-fields';
import { AuthService } from '../../services/auth.service';
import { TokenService } from '../../services/token.service';
import { AuthStoreService } from '../../stores/auth-store.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss'],
})
export class ResetPasswordComponent implements OnInit {
  private email = '';
  private token = '';
  constructor(
    private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute,
    private toastr: ToastrService,
  ) {
    this.form = new FormGroup(
      {
        password: new FormControl('Password!1', [Validators.required, Validators.minLength(6)]),
        confirmPassword: new FormControl('Password!1', [Validators.required, Validators.minLength(6)]),
      },
      { validators: matchFields('password', 'confirmPassword') },
    );
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      this.email = params['email'];
      this.token = params['token'];
    });
  }

  form: FormGroup;

  get password() {
    return this.form.get('password');
  }

  get confirmPassword() {
    return this.form.get('confirmPassword');
  }

  async submit() {
    if (this.form.invalid) return;

    try {
      await this.authService.recoverPassword({
        email: this.email,
        token: this.token,
        password: this.password!.value,
        confirmedPassword: this.confirmPassword!.value,
      });

      this.toastr.success('Password update successfully');
      this.router.navigate(['']);
    } catch (err: any) {
      this.toastr.error('Failed to reset password');

      if (err?.error?.errors) {
        err?.error?.errors.forEach((entry: ErrorModel) => {
          this.toastr.error(entry.message);
        });
      }
    }
  }
}

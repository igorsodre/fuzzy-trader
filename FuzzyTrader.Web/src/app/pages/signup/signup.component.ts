import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { matchFields } from 'src/app/custom-validators/match-fields';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.scss'],
})
export class SignupComponent implements OnInit {
  form: FormGroup = new FormGroup({});

  constructor() {
    this.form = new FormGroup(
      {
        email: new FormControl('dotnettest1@localhost.com', Validators.email),
        password: new FormControl('Password!1', [Validators.required, Validators.minLength(6)]),
        confirmPassword: new FormControl('', [Validators.required, Validators.minLength(6)]),
      },
      { validators: matchFields('password', 'confirmPassword') },
    );
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

  ngOnInit(): void {}

  get formErrors() {
    if (this.form.errors) {
      return Object.keys(this.form.errors).toString();
    }
    return '';
  }
}

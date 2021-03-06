import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from '../app-routing.module';
import { UiComponentsModule } from '../ui-components/ui-components.module';
import { StructuralComponentsModule } from './../structural-components/structural-components.module';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import { SignupComponent } from './signup/signup.component';
import { StartUpComponent } from './start-up/start-up.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { UserProfileComponent } from './user-profile/user-profile.component';
import { AvailableInvestmentsComponent } from './available-investments/available-investments.component';
import { UserWalletComponent } from './user-wallet/user-wallet.component';

@NgModule({
  declarations: [HomeComponent, SignupComponent, LoginComponent, StartUpComponent, ResetPasswordComponent, ForgotPasswordComponent, UserProfileComponent, AvailableInvestmentsComponent, UserWalletComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    UiComponentsModule,
    AppRoutingModule,
    StructuralComponentsModule,
  ],
  exports: [UiComponentsModule, StructuralComponentsModule],
})
export class PagesModule {}

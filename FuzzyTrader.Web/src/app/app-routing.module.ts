import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AuthGuardGuard } from './guards/auth-guard.guard';
import { AvailableInvestmentsComponent } from './pages/available-investments/available-investments.component';
import { ForgotPasswordComponent } from './pages/forgot-password/forgot-password.component';
import { HomeComponent } from './pages/home/home.component';
import { LoginComponent } from './pages/login/login.component';
import { ResetPasswordComponent } from './pages/reset-password/reset-password.component';
import { SignupComponent } from './pages/signup/signup.component';
import { StartUpComponent } from './pages/start-up/start-up.component';
import { UserProfileComponent } from './pages/user-profile/user-profile.component';
import { UserWalletComponent } from './pages/user-wallet/user-wallet.component';

const routes: Routes = [
  { path: '', component: StartUpComponent },
  { path: 'login', component: LoginComponent },
  { path: 'signup', component: SignupComponent },
  { path: 'reset-password', component: ResetPasswordComponent },
  { path: 'forgot-password', component: ForgotPasswordComponent },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuardGuard],
    children: [
      { path: 'home', component: HomeComponent },
      { path: 'user-profile', component: UserProfileComponent },
      { path: 'available-investments', component: AvailableInvestmentsComponent },
      { path: 'my-wallet', component: UserWalletComponent },
    ],
  },
  { path: '**', redirectTo: '' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}

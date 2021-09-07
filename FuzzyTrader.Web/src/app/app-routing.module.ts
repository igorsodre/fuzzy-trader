import { ResetPasswordComponent } from './pages/reset-password/reset-password.component';
import { StartUpComponent } from './pages/start-up/start-up.component';
import { SignupComponent } from './pages/signup/signup.component';
import { LoginComponent } from './pages/login/login.component';
import { HomeComponent } from './pages/home/home.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuardGuard } from './guards/auth-guard.guard';
import { ForgotPasswordComponent } from './pages/forgot-password/forgot-password.component';

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
    children: [{ path: 'home', component: HomeComponent }],
  },
  { path: '**', redirectTo: '' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}

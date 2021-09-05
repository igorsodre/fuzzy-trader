import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from '../app-routing.module';
import { UiComponentsModule } from '../ui-components/ui-components.module';
import { StructuralComponentsModule } from './../structural-components/structural-components.module';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import { SignupComponent } from './signup/signup.component';
import { StartUpComponent } from './start-up/start-up.component';

@NgModule({
  declarations: [HomeComponent, SignupComponent, LoginComponent, StartUpComponent, ResetPasswordComponent],
  imports: [CommonModule, UiComponentsModule, AppRoutingModule, StructuralComponentsModule],
  exports: [UiComponentsModule, StructuralComponentsModule],
})
export class PagesModule {}

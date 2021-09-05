import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './../app-routing.module';
import { UiComponentsModule } from './../ui-components/ui-components.module';
import { HeaderComponent } from './header/header.component';

@NgModule({
  declarations: [HeaderComponent],
  imports: [CommonModule, UiComponentsModule, AppRoutingModule],
})
export class StructuralComponentsModule {}

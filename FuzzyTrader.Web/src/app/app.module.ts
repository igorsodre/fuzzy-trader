import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ManageHttpInterceptor } from './insterceptors/manage-http.interceptor';

import { PagesModule } from './pages/pages.module';
import { HttpCancelService } from './services/http-cancel.service';

@NgModule({
  declarations: [AppComponent],
  imports: [BrowserModule, HttpClientModule, AppRoutingModule, BrowserAnimationsModule, PagesModule],
  providers: [HttpCancelService, { provide: HTTP_INTERCEPTORS, useClass: ManageHttpInterceptor, multi: true }],
  bootstrap: [AppComponent],
})
export class AppModule {}

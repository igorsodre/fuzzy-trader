import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { APP_INITIALIZER, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ToastrModule } from 'ngx-toastr';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BearerInterceptor } from './interceptors/bearer.interceptor';
import { LoadingInterceptor } from './interceptors/loading.interceptor';
import { ManageHttpInterceptor } from './interceptors/manage-http.interceptor';
import { PagesModule } from './pages/pages.module';
import { HttpCancelService } from './services/http-cancel.service';
import { InitializerService } from './services/initializer.service';

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    AppRoutingModule,
    ToastrModule.forRoot({ timeOut: 10000, preventDuplicates: true, positionClass: 'toast-bottom-right' }),
    PagesModule,
    NgxSpinnerModule,
  ],
  providers: [
    HttpCancelService,
    { provide: HTTP_INTERCEPTORS, useClass: ManageHttpInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: BearerInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true },
    {
      provide: APP_INITIALIZER,
      useFactory: (initializerService: InitializerService) => () => initializerService.startup(),
      deps: [InitializerService],
      multi: true,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}

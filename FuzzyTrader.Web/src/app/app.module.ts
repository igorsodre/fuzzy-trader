import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { APP_INITIALIZER, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BearerInterceptor } from './insterceptors/bearer.interceptor';
import { ManageHttpInterceptor } from './insterceptors/manage-http.interceptor';

import { PagesModule } from './pages/pages.module';
import { HttpCancelService } from './services/http-cancel.service';
import { InitializerService } from './services/initializer.service';
import { ToastrModule } from 'ngx-toastr';

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    AppRoutingModule,
    ToastrModule.forRoot({ timeOut: 10000, preventDuplicates: true, positionClass: 'toast-bottom-right' }),
    PagesModule,
  ],
  providers: [
    HttpCancelService,
    { provide: HTTP_INTERCEPTORS, useClass: ManageHttpInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: BearerInterceptor, multi: true },
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

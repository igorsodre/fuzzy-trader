import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { EMPTY, from, Observable, throwError } from 'rxjs';
import { TokenService } from '../services/token.service';
import { catchError } from 'rxjs/operators';

@Injectable()
export class BearerInterceptor implements HttpInterceptor {
  constructor(public tokenService: TokenService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return from(this.handle(request, next));
  }

  private async handle(request: HttpRequest<unknown>, next: HttpHandler) {
    console.log('Entering interceptor BearerInterceptor');
    let accessToken = '';
    try {
      accessToken = await this.tokenService.getAccessToken();
    } catch {
      console.log('Catch bearer after trying to get access token');
    }

    request.headers.append('Authorization', `Bearer ${accessToken}`);

    return next
      .handle(request)
      .pipe(
        catchError((error) => {
          console.log('catchError on bearer interceptor');
          console.log(error);
          return EMPTY;
        }),
      )
      .toPromise();
  }
}

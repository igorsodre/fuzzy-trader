import { HttpEvent, HttpHandler, HttpHeaders, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { from, Observable } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { TokenService } from '../services/token.service';

@Injectable()
export class BearerInterceptor implements HttpInterceptor {
  constructor(public tokenService: TokenService) {}

  private excludeUlrs = ['/api/account/login', '/api/account/signup', '/api/account/refresh-token'];

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return this.handle(request, next);
  }

  private handle(request: HttpRequest<unknown>, next: HttpHandler) {
    if (this.excludeUlrs.some((el) => request.url.includes(el))) {
      return next.handle(request);
    }

    return from(this.tokenService.getAccessToken()).pipe(
      switchMap((accessToken) => {
        request = request.clone({
          headers: new HttpHeaders({
            Authorization: `Bearer ${accessToken}`,
          }),
        });
        return next.handle(request);
      }),
    );
  }
}

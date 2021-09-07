import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { from, Observable } from 'rxjs';
import { TokenService } from '../services/token.service';

@Injectable()
export class BearerInterceptor implements HttpInterceptor {
  constructor(public tokenService: TokenService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return from(this.handle(request, next));
  }

  private async handle(request: HttpRequest<unknown>, next: HttpHandler) {
    let accessToken = '';
    try {
      accessToken = await this.tokenService.getAccessToken();
    } catch {}

    request.headers.append('Authorization', `Bearer ${accessToken}`);

    return next.handle(request).toPromise();
  }
}

import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ActivationEnd, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { HttpCancelService } from '../services/http-cancel.service';

@Injectable()
export class ManageHttpInterceptor implements HttpInterceptor {
  constructor(router: Router, private httpCancelService: HttpCancelService) {
    router.events.subscribe((event) => {
      // An event triggered at the end of the activation part of the Resolve phase of routing.
      if (event instanceof ActivationEnd) {
        // Cancel pending calls
        this.httpCancelService.cancelPendingRequests();
      }
    });
  }

  intercept<T>(req: HttpRequest<T>, next: HttpHandler): Observable<HttpEvent<T>> {
    return next.handle(req).pipe(takeUntil(this.httpCancelService.onCancelPendingRequests()));
  }
}

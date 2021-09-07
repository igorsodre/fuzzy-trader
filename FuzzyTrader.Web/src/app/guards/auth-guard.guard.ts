import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { TokenService } from './../services/token.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuardGuard implements CanActivate {
  constructor(private tokenService: TokenService, private router: Router) {}

  canActivate(): Observable<boolean> {
    return this.tokenService.accessToken$.pipe(
      map((token) => {
        if (token) return true;
        this.router.navigate(['/login']);
        return false;
      }),
    );
  }
}

import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { AppUser } from '../models/app-user';

@Injectable({
  providedIn: 'root',
})
export class AuthStoreService {
  private readonly _appUserSrouce = new BehaviorSubject<AppUser>(new AppUser(0, '', ''));
  constructor() {}

  public getUser(): AppUser {
    return this._appUserSrouce.getValue();
  }

  public setUser(appUser: AppUser) {
    this._appUserSrouce.next(appUser);
  }

  public resetUser() {
    this._appUserSrouce.next(new AppUser(0, '', ''));
  }
}

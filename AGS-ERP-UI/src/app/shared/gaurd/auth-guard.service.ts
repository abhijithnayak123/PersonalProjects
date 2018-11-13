import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { LocalStorageService } from '../wrappers/local-storage.service';
import { UserPrevilagesModel } from '../../user-management/models/user-previlages.model';

@Injectable()
export class AuthGuard implements CanActivate {
  constructor(private _localStorageService: LocalStorageService, private _router: Router) { }

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    return this.checkIfUserLoggedIn();
  }

  canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    return this.checkIfUserLoggedIn();
  }

  private checkIfUserLoggedIn(): boolean {
    const result = this._localStorageService.get('ags_erp_user_previlage');

    if (result) {
      let previlage: UserPrevilagesModel = new UserPrevilagesModel();
      previlage = JSON.parse(result);
      return true;
    } else {
      return false;
    }
  }
}

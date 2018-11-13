import { Component, OnInit } from '@angular/core';
import { LocalStorageService } from '../../../shared/wrappers/local-storage.service';
import { Router } from '@angular/router';
import { HeaderService } from '../../services/header.service';
import { ModuleModel } from '../../models/module.model';
import { Location } from '@angular/common';
import { Subscription } from 'rxjs/Subscription';
import { SessionStorageService } from '../../../shared/wrappers/session-storage.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
  // providers: [HeaderService]
})
export class HeaderComponent implements OnInit {
  
  public userName: string;
  selectedParentId: number;
  constructor(
    private _localStorageService: LocalStorageService,
    private _router: Router,
    private _headerService: HeaderService,
    private _location: Location,
    private _sessionStorageService: SessionStorageService
  ) { }
  ngOnInit() {
    const login = JSON.parse(
      this._localStorageService.get('ags_erp_user_previlage')
    );
    this.userName = login.FirstName;
    let sub = this._headerService.notifyObservable$.subscribe(res => setTimeout(() => {
      if (res.hasOwnProperty('key') && res.key === 'moduleId') {
        this.selectedParentId = res.value;
      }
    }, 0));
  }

  logout() {
    this._localStorageService.remove('ags_erp_user_previlage');
    this._sessionStorageService.remove('leftModuleId');
    this._router.navigate(['/user/login']);
    this._location.replaceState('/');
    this._localStorageService.remove('selectedheaderName');
  }

  goHome() {
    this.selectedParentId = 1;
  }

}

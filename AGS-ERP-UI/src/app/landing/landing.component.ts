import { Component, OnInit } from '@angular/core';
import { HeaderService } from '../common/services/header.service';
import { ModuleService } from '../common/services/module.service';
import { ModuleModel } from '../common/models/module.model';
import { LocalStorageService } from '../shared/wrappers/local-storage.service';
import { SessionStorageService } from '../shared/wrappers/session-storage.service';

@Component({
  selector: 'app-landing',
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.css']
})
export class LandingComponent implements OnInit {

  headerModules: ModuleModel[];

  constructor(
    public _headerService: HeaderService,
    public _moduleService: ModuleService,
    public _localStorageService: LocalStorageService,
    public _sessionStorageService: SessionStorageService
  ) { }

  ngOnInit() {
    let headers = this._moduleService.getHeaderMenu();
    this.headerModules = headers.splice(1);
  }

  navigate(parentId: number, childId: number){
    this._sessionStorageService.add('leftModuleId', parentId);
    this._headerService.notifyOther({
      key:'moduleId', value: parentId
    });
  }
  
}

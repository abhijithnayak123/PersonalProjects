import { Component, OnInit, OnDestroy, Output, EventEmitter } from '@angular/core';
import { ModuleService } from '../../services/module.service';
import { ModuleModel } from '../../models/module.model';
import { LocalStorageService } from '../../../shared/wrappers/local-storage.service';
import { HeaderService } from '../../services/header.service';
import { Subscription } from 'rxjs';
import { UserPrevilagesModel } from "../../../user-management/models/user-previlages.model";
import { SessionStorageService } from '../../../shared/wrappers/session-storage.service';

@Component({
  selector: 'app-left-menu',
  templateUrl: './left-menu.component.html',
  styleUrls: ['./left-menu.component.css']
})
export class LeftMenuComponent implements OnInit, OnDestroy {
  
  leftMenuList: Array<ModuleModel>;
  subscription : Subscription;
  showClass: string;
  @Output() onMenuExpand = new EventEmitter<Boolean>();
  childId: number;
  constructor(
    public moduleService: ModuleService,
    public _localStorageService: LocalStorageService,
    public _headerService: HeaderService,
    public _sessionStorageService: SessionStorageService
  ) { }
  
  ngOnInit() {
    let parentId = Number(this._sessionStorageService.get('leftModuleId'));
    this.leftMenuList = this.moduleService.getLeftMenu(parentId);
    this.showClass = 'show';
    this.subscription = this._headerService.notifyObservable$.subscribe(res => {
      if (res.hasOwnProperty('key') && res.key === 'moduleId') {
        let parentId = res.value;
        let a : UserPrevilagesModel
        a= JSON.parse(localStorage.getItem('ags_erp_user_previlage'));
        this.leftMenuList = this.moduleService.getLeftMenu(parentId);
      }
    });
    
  }

  getChildMenu(parentId: number): Array<ModuleModel> {
    let children : Array<ModuleModel> = [];
    let allLeftMenu : Array<ModuleModel> = this.moduleService.getLeftMenu(parentId);
    let userPrevillages : UserPrevilagesModel = JSON.parse(localStorage.getItem('ags_erp_user_previlage'));
    if(userPrevillages !== null){
      children= this.grantedModules(allLeftMenu, userPrevillages);
    }
    return children;
  }

  //Method check the Accessible modules
  grantedModules(allLeftMenu : Array<ModuleModel>,userPrevillages : UserPrevilagesModel):Array<ModuleModel>{
    let children : Array<ModuleModel> = allLeftMenu;
     for (var i = 0; i < allLeftMenu.length; i++) {
         for (var j = 0; j < userPrevillages.RolePermissions.length; j++) {
              if(allLeftMenu[i].Name == userPrevillages.RolePermissions[j].AllowedPermissions.DisplayName){;
                    if(userPrevillages.RolePermissions[j].AllowedFunctions.PrivilegeName == "Access Denied"){
                         children.splice(i,1);
                    }
               }
          }
     }
    return children;
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  onMenuAction(isExpanded: boolean){
    this.onMenuExpand.emit(isExpanded);
  }

  expandAll(isExpanded: boolean){
    if(isExpanded){
      this.showClass = 'show';
    }
    else{
      this.showClass = '';
    }
  }
}

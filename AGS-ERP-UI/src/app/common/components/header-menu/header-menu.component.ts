import { Component, OnInit, Input } from '@angular/core';
import { ModuleService } from '../../services/module.service';
import { ModuleModel } from '../../models/module.model';
import { HeaderService } from '../../services/header.service';
import { UserPrevilagesModel } from "../../../user-management/models/user-previlages.model";
import { LocalStorageService } from '../../../shared/wrappers/local-storage.service';
import { SessionStorageService } from '../../../shared/wrappers/session-storage.service';

@Component({
  selector: 'app-header-menu',
  templateUrl: './header-menu.component.html',
  styleUrls: ['./header-menu.component.css']
})
export class HeaderMenuComponent implements OnInit {
  HeaderMenu: Array<ModuleModel>;
  @Input() selectedParentId: number;
  
  constructor(
    public _moduleService: ModuleService,
    public _headerService: HeaderService,
    public _localStorageService: LocalStorageService,
    public _sessionStorage: SessionStorageService
  ) { }

  ngOnInit() {
    let headerMenu = this.getHeaderMenu();
    this.HeaderMenu =  this.accessibleMenu(headerMenu);
    this.selectedParentId = this._sessionStorage.get('leftModuleId');
    this._headerService.notifyOther({ key: 'moduleId', value: this.selectedParentId });
  }

  getHeaderMenu(): Array<ModuleModel> {
    return this._moduleService.getHeaderMenu();
  }

  accessibleMenu(headerMenu: Array<ModuleModel>){
    let userPrevillages : UserPrevilagesModel = JSON.parse(this._localStorageService.get('ags_erp_user_previlage'));
    let children= this.grantedModules(headerMenu,userPrevillages);
    return children;
  }
  grantedModules(headerMenu : Array<ModuleModel>,userPrevillages : UserPrevilagesModel):Array<ModuleModel>{
    let children : Array<ModuleModel> = headerMenu;
     for (var i = 0; i < headerMenu.length; i++) {
         for (var j = 0; j < userPrevillages.RolePermissions.length; j++) {
              if(headerMenu[i].Name == userPrevillages.RolePermissions[j].AllowedPermissions.DisplayName){
                    if(userPrevillages.RolePermissions[j].AllowedFunctions.PrivilegeName == "Access Denied"){
                      children.splice(i,1);
                    }
               }
          }
     }
    return children;
   }

   postData(headerComp: ModuleModel) {
    this.selectedParentId = headerComp.Id;
    this._sessionStorage.add('leftModuleId', this.selectedParentId);
    this._headerService.notifyOther({ key: 'moduleId', value: headerComp.Id });
  }
}

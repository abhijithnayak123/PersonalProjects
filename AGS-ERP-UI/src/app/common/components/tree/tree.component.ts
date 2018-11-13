import { Component, OnInit, Input } from '@angular/core';
import { ModuleModel } from '../../models/module.model';
import { ModuleService } from '../../services/module.service';
import { UserPrevilagesModel } from "../../../user-management/models/user-previlages.model";
import { LocalStorageService } from '../../../shared/wrappers/local-storage.service';

@Component({
  selector: 'app-tree',
  templateUrl: './tree.component.html',
  styleUrls: ['./tree.component.css']
})
export class TreeComponent implements OnInit {
  @Input() list: Array<ModuleModel>;
  activeClass  =  false;
  childId:  number;


  constructor(
    public _moduleService: ModuleService,
    public _localStorageService: LocalStorageService
  ) { }

  ngOnInit() {
    this.childId = Number(this._localStorageService.get('leftModuleChildId'));
  }

  hasChild(parentId: number): boolean {
    return this._moduleService.getLeftMenu(parentId).length > 0;
  }

  getChildMenu(parentId: number) {
    let allLeftMenu : Array<ModuleModel> = this._moduleService.getLeftMenu(parentId);
    let userPrevillages : UserPrevilagesModel = JSON.parse(localStorage.getItem('ags_erp_user_previlage'));
    let children= this.grantedModules(allLeftMenu,userPrevillages);
    this.list = children;
    return this.list;
  }

  grantedModules(allLeftMenu : Array<ModuleModel>,userPrevillages : UserPrevilagesModel):Array<ModuleModel>{
    let children : Array<ModuleModel> = allLeftMenu;
     for (var i = 0; i < allLeftMenu.length; i++) {
         for (var j = 0; j < userPrevillages.RolePermissions.length; j++) {
              if(allLeftMenu[i].Name == userPrevillages.RolePermissions[j].AllowedPermissions.DisplayName){
                    if(userPrevillages.RolePermissions[j].AllowedFunctions.PrivilegeName == "Access Denied"){
                         children.splice(i,1);
                    }
               }
          }
     }
    return children;
   }
  addActiveClass(childId) {
    this.childId = childId;
    this.activeClass  =  !this.activeClass;
  }
}

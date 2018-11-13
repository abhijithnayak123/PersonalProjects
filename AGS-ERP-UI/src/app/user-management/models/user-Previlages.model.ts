import { BaseModel } from "../../shared/models/base.model";

export class UserPrevilagesModel extends BaseModel {
  FirstName: string;
  LastName: string;
  FullName: string;
  UserName: string;
  ShortName: string;
  RolePermissions:any[] = new Array();
  Abbreviation:any[] = new Array();
   PrivilegeName: any[] = new Array();
}


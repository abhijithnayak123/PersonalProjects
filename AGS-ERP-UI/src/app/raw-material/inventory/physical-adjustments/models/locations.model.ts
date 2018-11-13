import { BaseModel } from "../../../../shared/models/base.model";
export class LocationsModel extends BaseModel {
  constructor(
          public Id: number,
          public Code: string,
          public Name: string) 
  {
    super();
  }
}

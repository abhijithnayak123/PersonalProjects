import { BaseModel } from "../../../../shared/models/base.model";
export class BinLocationsModel extends BaseModel{
    constructor(
        public Id:number,
        public AreaCode:string,
    )
    {
        super();
    }
}
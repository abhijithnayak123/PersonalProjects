import { BaseModel } from "../../../shared/models/base.model";

export class VendorItem extends BaseModel{
    constructor(
        public Id:number,
        public Description:string
    ){
        super()
    }
}
import { BaseModel } from "../../../shared/models/base.model";

export class VendorModel extends BaseModel {
    constructor(
        public Id: number, 
        public Description: string
    ){
        super();
    }
}
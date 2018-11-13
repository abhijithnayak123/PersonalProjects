import { BaseModel } from '../../../shared/models/base.model';

export class BomSearchVendor extends BaseModel {
    constructor(
        public VendorId : number,
        public VendorCode : string,
        public VendorName : string,
    ) {
        super();
    }
   
}
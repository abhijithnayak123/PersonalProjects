import { BaseModel } from '../../../shared/models/base.model';

export class SupplierVendorSite extends BaseModel {
    constructor(
        public Id : number,
        public Code : string,
        public Name : string,
        public VendorId: number
    ) {
        super();
    }
}

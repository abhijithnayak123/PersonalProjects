import { BaseModel } from '../../../shared/models/base.model';

export class SupplierVendor extends BaseModel {
    constructor(
        public Id : number,
        public Code : string,
        public Name : string,
        public SiteId: number,
        public SiteCode: string,
        public SiteName: string
    ) {
        super();
    }
}

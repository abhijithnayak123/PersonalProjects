import { BaseModel } from '../../../../shared/models/base.model';

export class Vendor extends BaseModel {
    constructor(
        public VendorId: number,
        public RMVendorCode: string,
        public RMVendorName: string
    ) {
        super();
    }
}

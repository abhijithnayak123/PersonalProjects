import { BaseModel } from '../../../shared/models/base.model';

export class Vendor extends BaseModel {
    constructor(
        public VendorId: number,
        public VendorName: string
    ) {
        super();
    }
}

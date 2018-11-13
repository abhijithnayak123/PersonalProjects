import { BaseModel } from '../../../shared/models/base.model';

export class AramarkItem extends BaseModel {
    constructor(
        public Id: number,
        public ItemNumber: string,
        public VendorId: number,
        public Description : string
    ) {
        super();
    }
}

import { BaseModel } from '../../../shared/models/base.model';

export class PurchaseOrder extends BaseModel {
    constructor(
        public Id: number,
        public PO: number
    ) {
        super();
    }
}

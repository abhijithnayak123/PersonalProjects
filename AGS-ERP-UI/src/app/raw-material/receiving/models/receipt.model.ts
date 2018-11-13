import { BaseModel } from '../../../shared/models/base.model';

export class Receipt extends BaseModel {
    constructor(
        public ReceiptNumber: number,
        public ReceiptId: number,
    ) {
        super();
    }
}

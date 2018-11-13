import { BaseModel } from '../../../../shared/models/base.model';

export class PurchaseOrder extends BaseModel {
    constructor(
        public POHdrId: number,
        public VendorId: number,
        public VendorSiteId: number,
        public ItemVendorId: number,
        public PONbr: string,
        public OrderDate: Date,
        public SchedShipDate: Date,
        public PoCreatedDate : Date
    ) {
        super();
    }
}

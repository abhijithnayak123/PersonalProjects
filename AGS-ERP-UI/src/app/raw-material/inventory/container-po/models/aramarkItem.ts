import { BaseModel } from '../../../../shared/models/base.model';

export class AramarkItem extends BaseModel {
    constructor(
        public VendorId : number,
        public VendorSiteId : number,
        public FGVendorId : number,
        public ItemVendorId : number,
        public ItemCode : string,
        public ItemName : string,
        public VendorItemNbr : string,
        public FiberContent : string,
        public CountryOfOrigin : string,
        public ColorCode : string,
        public ColorName : string,
        public POHdrId : number,
        public ItemID : number
    ) {
        super();
    }
}

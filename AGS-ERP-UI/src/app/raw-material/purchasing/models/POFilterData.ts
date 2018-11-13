import { BaseModel } from '../../../shared/models/base.model';
export class POFilterData extends BaseModel {
    constructor(
        public PO_NBR: number,
        public ORDER_DATE: Date ,
        public ItemNbr: string,
        public Vendor : string,
        public PickupLoc:string,
        public SCHED_SHIP_DATE: Date,
        public ShipTo : string,
        public Yards:number,
        public UNIT_PRICE   : number,
        public ExtendedValue: number,
        public PO_Status : string,
        public ApprovedBy:string,
        public VendorConfirmed:Date,
        public PO_HDR_ID : number,
        public VENDOR_ID : number,
        public SHIP_FROM_VENDOR_SITE_ID : number,
        public ITEM_ID : number,
        public PO_STATUS_ID : number,
        public Pickup_Location_ID : number,
    ) {    super();
    }
}

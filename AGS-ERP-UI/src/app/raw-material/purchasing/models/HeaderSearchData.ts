import { BaseModel } from '../../../shared/models/base.model';

export class HeaderSearchData extends BaseModel {
    constructor(       
        public Vendor: string ,
        public PickupLoc: string,
        public PONbr: number,
        public ItemNbr: string,
        public SchedShipDate: Date,
        public POStatus: string,
        public OrderDate: Date
    ) {
        super();
    }
}

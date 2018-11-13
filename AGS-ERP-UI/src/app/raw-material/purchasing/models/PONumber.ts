import { BaseModel } from '../../../shared/models/base.model';
export class PONumber extends BaseModel {
    constructor(
        public PONbr: number,
        public OrderDate: string,
        public Vendor:string,
        public Status: string,
        public POHdrId : number,
        public VendorId : number
    ) {
        super();
    }
}

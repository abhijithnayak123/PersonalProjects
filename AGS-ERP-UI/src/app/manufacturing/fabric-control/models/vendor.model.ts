import { BaseModel } from "../../../shared/models/base.model";
import { Lot } from "./lot.model";

export class Vendor extends BaseModel {
    constructor(
        public Id: number,
        public Name: string,
        public COO: string,
        public VendorSiteId: number,
        public VendorSite: string,
        public LotNumbers: Array<Lot>,
        public VendorItem: string,
    ){
        super();
    }
}
import { BaseModel } from "../../../../shared/models/base.model";

export class VendorDetails extends BaseModel {
    constructor(
        public Id: number,
        public VendorItemId: number,
        public ItemId: number,
        public Code: string,
        public Name: string,
        public CountryOfOrigin: string,
        public VendorDetail: string,
        public VendorItemNumber: string
    ) {
        super();
    }
}

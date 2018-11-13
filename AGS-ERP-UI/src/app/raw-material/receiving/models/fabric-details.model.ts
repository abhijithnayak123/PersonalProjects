import { BaseModel } from "../../../shared/models/base.model";

export class FabricDetails extends BaseModel {
    constructor(
        public Color: string,
        public Width: number,
        public FiberContent: string,
        public VendorItemCodes: Array<string>
    ){
        super();
    }
}
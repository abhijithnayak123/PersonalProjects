import { BaseModel } from "../../../shared/models/base.model";

export class AramarkItemModel extends BaseModel {
    constructor(
        public Id: number,
        public AramarkItemCode: string,
        public Color: string,
        public Width: number,
        public FiberContent: string,
        public VendorItem: string,
        public COO: string
    ){
        super();
    }
}
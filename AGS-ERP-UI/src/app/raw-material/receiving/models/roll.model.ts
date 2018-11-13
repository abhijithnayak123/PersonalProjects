import { BaseModel } from "../../../shared/models/base.model";

export class Roll extends BaseModel {
    constructor(
        public RollNumber: number,
        public PORelease: number,
        public PO: number,
        public BinLocation: string,
        public AramarkItemCode: string,
        public VendorItemCode: string,
        public Color: string,
        public Yards: number,
        public Width: number,
        public Lot: string,
        public FiberContent: string,
        public IsUsable: boolean,
        public IsSelected : boolean
    ){
        super();
    }
}
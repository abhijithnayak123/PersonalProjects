import { BaseModel } from "../../../shared/models/base.model";

export class AllocateResult extends BaseModel {
    constructor(
        public IsSelected: boolean,
        public CutHDRId: number,
        public CutItemId: number,
        public VendorSiteId: number,
        public ItemCode: string,
        public ItemId: number,
        public FiberContent: string,
        public VendorItem: string,
        public RollCaseId: number,
        public Width: number,
        public LotNumber: string,
        public AvailableYards: number,
        public AllocatedQty: number,
        public Consignment: boolean,
        public ReceiptDate: string,
        public BinLocation: string,
        public IsAddedToCurrentSelection: boolean,
        public ConsignmentField: string,
        public RMVendorName: string,
        public IsAllocatedRoll: boolean,
        public Status: string

    ){
        super();
    }
}


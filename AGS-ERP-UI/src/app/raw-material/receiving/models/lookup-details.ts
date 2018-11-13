import { BaseModel } from '../../../shared/models/base.model';

export class LookUpGridDetails extends BaseModel {
    constructor(
        public ReceiptNumber: number,
        public ReceivedDate: any,
        public PONumber: string,
        public RollCaseId: number,
        public Quantity: number,
        public ReceivedYards: number,
        public LotNumber: string,
        public BinLocation: string,
        public ItemCode: string,
        public Color: string,
        public Width: number,
        public FiberContent: string,
        public VendorItem: string,
        public COO: string,
        public Defective: number,
        public POComments: string,
        public VendorCode: string,
        public RollCaseNbr: string,
        public InventoryAreaId: number,
        public AramarkItemId: number,
        public POLineId: number,
        public POHdrId: number,
        public AramarkItem: string,
        public ContainerNumber: string,
        public PickupNumber: string,
        public CurrentReceivingRolls: number,
        public CurrentReceivingYards: number,
        public ReceivedRolls: number,
        public Comments: string
    ) {
        super();
    }
}

import { BaseModel } from '../../../shared/models/base.model';
import { AramarkItemCode } from "./aramark-item-code.model";

export class Receiving extends BaseModel {
    constructor(
        public IsSelected: boolean,
        public PurchaseOrder: any,
        public RollCaseId: string,
        public RollYards: number,
        public LotNumber: string,
        public BinLocation: any,
        public AramarkItemCode: AramarkItemCode,
        public Color: string,
        public Width: number,
        public FiberContent: string,
        public VendorItem: string,
        public COO: string,
        public Defective: number,
        public ReceiveDate: string,
        public ReceiptId :number
    ) {
        super();
    }
}

export class ReceivingHeader extends BaseModel {
    constructor(
        public Container: number,
        public PickupNumber: string,
        public Shipment: number,
        public VendorName: string,
        public ASN: string,
        public VendorCode: number,
        public BillOfLading: string,
        public PickupLocation: string,
        public TotalYards: number,
        public TotalRolls: number,
        public Comments: string,
        public ReceivedRolls:number,
        public ReceivedYards:number,
        public ActualReceivedRolls:number,
        public ActualReceivedYards:number

    ) {
        super();
    }
}

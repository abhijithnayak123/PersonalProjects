import { BaseModel } from '../../../shared/models/base.model';

export class LookHeaderDetails extends BaseModel {
    constructor(
        public ReceiptId: number,
        public ContainerNbr: string,
        public ContainerId: number,
        public PickupNumber: string,
        public ContainerStopId: number,
        public VendorId: number,
        public VendorCode: string,
        public VendorName: string,
        public PickupLocation: string,
        public POHdrId: number,
        public PONumber: string,
        public ReceiptNumber: string,
        public ReceiptDate: Date,
        public ReceiptFromDate: any,
        public ReceiptToDate: any,
        public Shipment: string,
        public ASN: string,
        public BillOfLading: string,
        public TotalRolls: number,
        public TotalYards: number,
        public ReceivedRolls: number,
        public ReceivedYards: number,
        public Comments: string,
        public ContainerVendor: string,
        public PoCreatedDate: Date
    ) {
        super();
    }
}


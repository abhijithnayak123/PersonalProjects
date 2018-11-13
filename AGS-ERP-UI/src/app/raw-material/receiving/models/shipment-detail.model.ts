import { BaseModel } from "../../../shared/models/base.model";
import { Roll } from "./roll.model";

export class ShipmentDetail extends BaseModel {
    constructor(
        public ReceiptId: number,
        public ReceivedDate: Date,
        public VendorId: number,
        public TotalRolls: number,
        public TotalYards: number,
        public BillOfLading: number,
        public Container: number,
        public Shipment: number,
        public ASN: number,
        public Comment: string,
        public Rolls: Array<Roll>
    ){
        super();
    }
}
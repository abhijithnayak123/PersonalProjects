import { BaseModel } from '../../../shared/models/base.model';

export class FabricContainerdetails extends BaseModel {
    constructor(
        public PickUpNumber: string,
        public VendorCode: string,
        public VendorName: string,
        public PickUpLocation: string,
        public Shipment: number,
        public ASN: string,
        public BillOfLading: string,
        public TotalRolls: number,
        public TotalYards: number
    ) {
        super();
    }
}

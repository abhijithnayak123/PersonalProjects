import { BaseModel } from '../../../../shared/models/base.model';

export class FabricContainer extends BaseModel {
    constructor(
        public BuildNbr: number,
        public ReceiverDcId: number,
        public ContainerId: number,
        public CarrierId: number,
        public CarrierCode: string,
        public CarrierName: string,
        public ReceivingDcCode: string,
        public ReceivingDcName: string,
        public ContainerType: string,
        public ContainerTypeDescription: string,
        public ExpectedPickupDate: Date,
        public ExpectedBorderDate: Date,
        public BatchGroupNumber : number,
        public ContainerNbr : string,
        public VendorNames : string,
        public Vendor : string,
    ) {
        super();
    }
}

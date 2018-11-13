import { BaseModel } from '../../../../shared/models/base.model';

export class ContainertrackingHeaderdetails extends BaseModel {
    constructor(
        public ContainerId: number,
        public VendorId: number,
        public ContainerNbr: string,
        public CurrEventId: number,
        public CurrEvent: string,
        public NextEventId: number,
        public NextEvent: string,
        public PrevEventId: number,
        public PrevEvent: string,
        public PrevEventExpectedDate: Date,
        public CurrEventExpectedDate: Date,
        public NextEventExpectedDate: Date,
        public ExpectedConfirmationDate: Date,
        public ActualConfirmationDate: Date,
        public ExpectedPickupDate: Date,
        public ActualPickupDate: Date,
        public ExpectedBorderDate: Date,
        public ExpectedAtFactoryDate: Date,
        public ExpectedDcDate: Date,
        public CarrierConfirmationDate: Date,
        public VendorConfirmationDate: Date,
        public ActualOriginDate: Date,
        public ActualBorderDate: Date,
        public ActualAtFactoryDate: Date,
        public ActualDcDate: Date,
        public Comments: string,
        public CarrierId: number,
        public CarrierCode: string,
        public CarrierName: string,
        public ReceivingDcCode: string,
        public ReceivingDcName: string,
        public ContainerType: string,
        public ContainerTypeDescription: string,
        public OGExpectedAtFactoryDate: Date,
        public OGExpectedPickupDate: Date,
        public OGExpectedBorderDate: Date,
        public OGExpectedDcDate: Date,
        public OGExpectedConfirmationDate: Date,
        public ContainerRefNumber: string,
        public CreatedDate: Date
    ) {
        super();
    }
}

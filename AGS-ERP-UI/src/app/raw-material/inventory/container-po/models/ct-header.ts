import { BaseModel } from '../../../../shared/models/base.model';

export class CTHeader extends BaseModel {
    constructor(
        public ContainerId: number,
        public VendorId: number,
        public CurrEventId: number,
        public CurrEventExpectedDate: any,
        public ContainerNbr: string,
        public ExpectedConfirmationDate: any,
        public ExpectedPickupDate: any,
        public ExpectedBorderDate: any,
        public ExpectedAtFactoryDate: any,
        public ExpectedDcDate: any,
        public Comments: string,
        public ContainerRefNumber: string,
    ) {
        super();
    }
}

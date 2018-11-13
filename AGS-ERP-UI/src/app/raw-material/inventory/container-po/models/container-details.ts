import { BaseModel } from '../../../../shared/models/base.model';

export class ContainerDetails extends BaseModel {
    constructor(
        public ContainerId: number,
        public ContainerNbr: string,
        public ContainerStopId: number,
        public PickupNbr: string,
        public StopNbr: number,
        public StopStatusId: number,
        public StopStatus: string,
        public VendorId: number,
        public VendorCode: string,
        public VendorName: string,
        public VendorSiteId: number,
        public VendorSiteCode: string,
        public VendorSiteName: string,
        public PickupLocnAddress: string,
        public ShipmentNbr: string,
        public AsnNbr: string,
        public BillOfLadingNbr: string,
        public TotalRolls: any,
        public TotalYards: any,
        public ShipmentHdrId: number,
        public VendorReportUrl: string,
        public RequestedYards: number
    ) {
        super();
    }
}

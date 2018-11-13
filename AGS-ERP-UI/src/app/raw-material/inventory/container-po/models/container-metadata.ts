import { BaseModel } from '../../../../shared/models/base.model';

export class ContainerMetaData extends BaseModel {
    constructor(
        public ContainerId: number,
        public ContainerNumber: string,
        public ContainerStopId: number,
        public PickupNumber: string,
        public POHdrId: number,
        public PONumber: string,
        public VendorId: number,
        public VendorName: string,
        public VendorItem: string,
        public AramarkItemId: number,
        public AramarkItemCode: string,
        public StatusId: number,
        public StatusCode: string,
        public StatusDescription: string,
        public LogicalStatusDescription: string,
        public TotalRolls: any,
        public TotalYards: any,
        public PickupLocation: string,
        public Color: string,
        public FiberContent: string,
        public COO: string,
        public CreatedDate: string,
        public AramarkDescription: string
    ) {
        super();
    }
}



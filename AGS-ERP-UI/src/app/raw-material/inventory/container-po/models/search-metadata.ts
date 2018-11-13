import { BaseModel } from '../../../../shared/models/base.model';

export class SearchMetaData extends BaseModel {
    constructor(       
        public ContainerNumber: string,
        public PickupNumber: string,
        public PONumber: string,
        public VendorName: string,
        public VendorItem: string,
        public AramarkItemCode: string,
        public FiberContent: string,
        public LogicalStatusDescription: string
    ) {
        super();
    }
}



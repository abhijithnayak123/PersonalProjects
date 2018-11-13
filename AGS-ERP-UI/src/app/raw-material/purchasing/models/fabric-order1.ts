import { BaseModel } from '../../../shared/models/base.model';

export class FabricOrder1 extends BaseModel{
    
     constructor(
        public ItemID: number,
        public VendorID: number,
        public RequestedDate: Date,
        public DeliveryDate: Date,
        public ItemNumber: string,
        public Description: string,
        public VendorItem: string,
        public FiberContent: string,
        public COO: string,
        public LeadTime: number,
        public OnHand: number,
        public AvgWeeklyDemand: number,
        public MinWOS: number,
        public SafetyStock: number,
        public POPrice: number,
        public SystemYds: number,
        public SystemCost: number,
        public OrderYds: string,
        public OrderCost: number,
        public IsSelected: boolean,
        public ActionType: number,
        public AdditionalDescription: string,
        public Comment : string,
    ) {
        super();
    }

}

import { BaseModel } from "../../../../shared/models/base.model";

export class MaintainRolls extends BaseModel{
    constructor(
        public IsSelected:boolean,
        public RollCaseId: string,
        public AramarkItemCode:string,
        public Color:string,
        public FiberContent:string,
        public VendorItem:string,
        public Vendor:string,
        public Lot:string,
        public COO :string,        
        public RollWidth:string,
        public OnHand:number,
        public Allocated:number,
        public Available:number,
        public Defective:number,
        public ReasonCode:string
    ){
        super();
    }
}

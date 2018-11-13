import { BaseModel } from '../../../shared/models/base.model';

export class ItemDetail extends BaseModel {
    constructor() {
        super();
    }
        public Id : number;
        public Code : string;
        public Description : string;
        public StatusId: number;
        public LastStatusId: number;
        public FabricItem: boolean;
        public FiberContent: string;
        public TypeId: number;
        public ProductCategoryId: number;
        public CostUOMId: number;
        public StockUOMId: number;
        public PackedAsRoll: boolean;
        public StdCost:number;
        public ProjCost: number;
        public Weight: number;
        public ColorId: number;
        public FlameResistant: boolean;
        public Finish: string;
        public CuttableWidth: number;
        public GreigeGood: string;
        public GreigeItemId: number;
        public Pattern: string;
        public LastStatus: string;
        public Comments: string;
        public CreatedON: Date;
        public CreatedBy: string;
        public LastModifiedON: Date;
        public LastModifiedBy: string;      
}

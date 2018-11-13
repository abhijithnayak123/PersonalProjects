import { BaseModel } from '../../../shared/models/base.model';

export class BomFilteredGridData extends BaseModel {
    constructor() {
        super();
    }
    public Id : number;
    public TypeId : number;
    public TypeName : string;
    public ItemId : number;
    public ItemCode : string;
    public Description : string;
    public ItemStatus : string;
    public Color : string;
    public UOM : string;
    public VendorId : number;
    public Vendor : string;
    public AverageQuantity: string;
    public WasteFactor: string;
    public IsInBOMCompGrid : boolean;
    public IsAdded : boolean;
    public IsRemoved : boolean;
    public IsModified : boolean;

    public BOMHdrId : number;
    public StatusId : number;
    public AverageQuantityWithWaste : number;
    
}
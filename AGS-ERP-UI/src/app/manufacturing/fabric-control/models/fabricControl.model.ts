import { BaseModel } from "../../../shared/models/base.model";
import { AllocateModel } from "./allocate.model";

export class    FabricControlModel extends BaseModel {
    constructor(
        public CutOrder : string,
        public SewPlant : string,
        public CutPlant : number,
        public Style : string,
        public Color : string,
        public MPSCutQuantity : number,
        public FiscalWeekNumber : number,
        public CutDate : string,
        public TotalBOMYield : number,
        public MarkerYield : number,
        public Comment :string,
        public Allocated:string,
        public ActualCutQty: number,
        public ActualCutQtyDisabled: number,
        public FabricRollCases : Array<AllocateModel>,
        public StyleDescription:string,
    ){
        super();
    }
}

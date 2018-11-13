import { BaseModel } from "../../../../shared/models/base.model";
import { RollCasesModel } from "./roll-case.model";

export class InventoryControl extends BaseModel{

    public LocationId : number;
    public SourceBinId : number;
    public DestinationBinId : number;
    public RollCases : Array<RollCasesModel>;
}
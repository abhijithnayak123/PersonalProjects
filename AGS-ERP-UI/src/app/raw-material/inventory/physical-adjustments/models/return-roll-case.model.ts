import { BaseModel } from "../../../../shared/models/base.model";

export class ReturnRollCase extends BaseModel{
    constructor(
        RollCaseId:string,
        CutOrder:string,
        Vendor:string,
        Style:string,
        Color:number,
        AramarkItem:string,
        LotNumber:string,
        Width:number,
        DateConsumed:Date,
        BinLocation:string,
        OnHandYards:number,
        TableNumber:number,
        IsSelected:boolean
        ){
        super();
    }  

}

import { BaseModel } from "../../../shared/models/base.model";


export class Allocation extends BaseModel{
    constructor(
        public Id:number,
        public Type:string
    ){
        super()
    }
}
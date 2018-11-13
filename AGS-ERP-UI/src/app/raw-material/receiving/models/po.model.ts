import { BaseModel } from "../../../shared/models/base.model";

export class POModel extends BaseModel {
    constructor(
        public PONumber: string,
        public POLineId:number,
        public POHdrId:number
    ){
        super();
    }
}
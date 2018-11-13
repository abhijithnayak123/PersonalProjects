import { BaseModel } from "../../../shared/models/base.model";

export class BomVersion extends BaseModel{
    constructor(
        public BomHdrId: number,
        public Version: number
    ){
        super();
    }
}
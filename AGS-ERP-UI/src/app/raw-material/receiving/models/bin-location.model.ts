import { BaseModel } from "../../../shared/models/base.model";

export class BinLocation extends BaseModel {
    constructor(
        public Id: number,
        public AreaCode: string
    ){
        super();
    }
}
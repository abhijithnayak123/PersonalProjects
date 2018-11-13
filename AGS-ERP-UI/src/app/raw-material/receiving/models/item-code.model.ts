import { BaseModel } from "../../../shared/models/base.model";

export class ItemCode extends BaseModel {
    constructor(
        public POReleaseNumber: number,
        public ItemCode: string
    ){
        super();
    }
}
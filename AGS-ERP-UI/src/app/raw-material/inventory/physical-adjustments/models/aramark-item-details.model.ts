import { BaseModel } from "../../../../shared/models/base.model";

export class AramarkItemDetails extends BaseModel{
    constructor(
        public Id: number,
        public ItemCode: string,
        public ItemName: string,
        public Width: number,
        public FiberContent: string
    ){
        super();
    }
}
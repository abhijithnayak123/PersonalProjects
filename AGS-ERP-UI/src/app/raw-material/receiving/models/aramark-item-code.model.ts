import { BaseModel } from "../../../shared/models/base.model";

export class AramarkItemCode extends BaseModel {
    constructor(
        public Id: number,
        public ItemCode: string
    ) {
        super();
    }
}
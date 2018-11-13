import { BaseModel } from "../../../shared/models/base.model";

export class PORelease extends BaseModel {
    constructor(
        public PORelaseNumber: string
    ){
        super();
    }
}
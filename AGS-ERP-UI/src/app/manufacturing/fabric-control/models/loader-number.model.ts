import { BaseModel } from "../../../shared/models/base.model";

export class LoaderNumber extends BaseModel {
    constructor(
        public Id: number,
        public LoaderNumber: number
    ){
        super();
    }
}
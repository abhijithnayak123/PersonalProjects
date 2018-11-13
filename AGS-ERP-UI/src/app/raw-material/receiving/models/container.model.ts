import { BaseModel } from "../../../shared/models/base.model";

export class ContainerModel extends BaseModel {

    constructor(
        public Id: number,
        public Number: number,
        public Vendor: string
    ){
        super();
    }
}
import { BaseModel } from "../../../shared/models/base.model";

export class PickupModel extends BaseModel {
    constructor(
        public PickupNumber: string,
        public ContainerId:number,
        public Vendor:string,
        public Location:string
    ){
        super();
    }
}
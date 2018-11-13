import { BaseModel } from '../../../shared/models/base.model';

export class BomDescription extends BaseModel {
    constructor(
        public ItemId : number,
        public ItemCode : string,
        public ItemDescription : string,
    ) {
        super();
    }
    

}
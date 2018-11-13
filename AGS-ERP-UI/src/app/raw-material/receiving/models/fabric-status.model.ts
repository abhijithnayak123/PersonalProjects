import { BaseModel } from '../../../shared/models/base.model';


export class FabricStatus extends BaseModel {
    constructor(
        public Id: number,
        public Status: string,
        public Description: string,      
    ) {
        super();
    }
}

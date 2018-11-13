import { BaseModel } from '../../../shared/models/base.model';
export class POHdr extends BaseModel {
    constructor(
        public POHdrId : number,
        public Status: string
    ) {
        super();
    }
}

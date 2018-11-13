import { BaseModel } from '../../../shared/models/base.model';

export class Asn extends BaseModel {
    constructor(
        public Id : number,
        public Code : string,
        public Description : string
    ) {
        super();
    }
}

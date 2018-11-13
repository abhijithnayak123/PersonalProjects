import { BaseModel } from '../../../shared/models/base.model';

export class GreigeItem extends BaseModel {
    constructor(
        public Id : number,
        public Code : string,
        public Name : string
    ) {
        super();
    }
}

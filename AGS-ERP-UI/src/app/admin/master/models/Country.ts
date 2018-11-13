import { BaseModel } from '../../../shared/models/base.model';

export class Country extends BaseModel {
    constructor(
        public Id : number,
        public Code : string,
        public Name : string
    ) {
        super();
    }
}

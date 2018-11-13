import { BaseModel } from '../../../shared/models/base.model';

export class ItemColor extends BaseModel {
    constructor(
        public Id : number,
        public Code : string,
        public Name : string
    ) {
        super();
    }
}

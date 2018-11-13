import { BaseModel } from '../../../shared/models/base.model';

export class Type extends BaseModel {
    constructor(
        public Id : number,
        public Code : string,
        public Description : string
    ) {
        super();
    }
}

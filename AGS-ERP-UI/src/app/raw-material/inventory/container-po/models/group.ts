import { BaseModel } from '../../../../shared/models/base.model';

export class Group extends BaseModel {
    constructor(
        public Id: number,
        public Code: string,
        public Weight : number
    ) {
        super();
    }
}

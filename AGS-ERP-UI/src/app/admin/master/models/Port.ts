import { BaseModel } from '../../../shared/models/base.model';

export class Port extends BaseModel {
    constructor(
        public Id : number,
        public Code : string,
        public CityCode: string,
        public Description : string
    ) {
        super();
    }
}

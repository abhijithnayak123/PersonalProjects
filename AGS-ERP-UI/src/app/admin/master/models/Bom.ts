import { BaseModel } from '../../../shared/models/base.model';

export class Bom extends BaseModel {
    constructor(
        public Id : number,
        public Style : string,
        public Color : string,
        public Vendor: String,
        public YDS: number
    ) {
        super();
    }
}

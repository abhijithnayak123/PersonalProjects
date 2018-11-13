import { BaseModel } from '../../../shared/models/base.model';

export class PickUpNumber extends BaseModel {
    constructor(
        public Id: number,
        public Number: number
    ) {
        super();
    }
}

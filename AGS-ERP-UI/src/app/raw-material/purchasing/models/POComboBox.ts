import { BaseModel } from '../../../shared/models/base.model';
export class POComboBox extends BaseModel {
    constructor(
        public PONbr: number,
        public OrderDate: Date
    ) {
        super();
    }
}

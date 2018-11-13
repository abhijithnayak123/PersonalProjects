import { BaseModel } from '../../../../shared/models/base.model';

export class PoCombo extends BaseModel {
    constructor(
        public PoNbr: string,
        public CreatedDate: string
    ) {
        super();
    }
}

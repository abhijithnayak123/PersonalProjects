import { BaseModel } from '../../../../shared/models/base.model';

export class AramarkCombo extends BaseModel {
    constructor(
        public AramarkItemCode: string,
        public AramarkDescription: string
    ) {
        super();
    }
}

import { BaseModel } from '../../../shared/models/base.model';
export class POItemNumber extends BaseModel {
    constructor(
        public ItemId: number,
        public ItemCode: string
    ) {
        super();
    }
}

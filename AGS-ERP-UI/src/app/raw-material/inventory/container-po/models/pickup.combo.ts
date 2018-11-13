import { BaseModel } from '../../../../shared/models/base.model';

export class PickupCombo extends BaseModel {
    constructor(
        public PickupNbr: string,
        public VendorName: string,
        public PickupLocation: string
    ) {
        super();
    }
}

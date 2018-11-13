import { BaseModel } from '../../../../shared/models/base.model';

export class PickupLocation extends BaseModel {
    constructor(
        public VendorId: number,
        public PickupLocationId: number,
        public PickupLocation: string,
   	    public VendorSiteId: number,
        public PickupLocnCode: string,
        public PickupLocnName: string,    ) {
        super();

    }
}

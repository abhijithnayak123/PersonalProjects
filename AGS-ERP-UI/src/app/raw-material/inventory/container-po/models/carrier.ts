import { BaseModel } from '../../../../shared/models/base.model';

export class Carrier extends BaseModel {
    constructor(
        public CarrierId: number,
        public CarrierCode: string,
        public CarrierName: string,
    ) {
        super();
    }
}

import { BaseModel } from '../../../../shared/models/base.model';

export class Container extends BaseModel {
    constructor(
        public ContainerId: number,
        public VendorId: number,
        public ContainerNbr: string,
        public VendorNames: string
    ) {
        super();
    }
}

import { BaseModel } from '../../../../shared/models/base.model';

export class ContainerCombo extends BaseModel {
    constructor(
        public ContainerId: number,
        public ContainerNbr: string,
        public VendorName: string
    ) {
        super();
    }
}

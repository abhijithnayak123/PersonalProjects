import { BaseModel } from '../../../shared/models/base.model';

export class FabricContainer extends BaseModel {
    constructor(
        public ContainerNbr: string,
        public ContainerId: number
    ) {
        super();
    }
}

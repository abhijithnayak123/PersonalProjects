import { BaseModel } from '../../../../shared/models/base.model';

export class Build extends BaseModel {
    constructor(
        public BuildNbr: number,
        public ContainerCount: number,
        public CreatedOn: Date,
        public CreatedBy: string,
        public BatchStatus: string
    ) {
        super();
    }
}

import { BaseModel } from '../../../../shared/models/base.model';

export class Note extends BaseModel {
    constructor(
        public ContainerId: number,
        public ContainerNbr: string,
        public NotesId: number,
        public Notes: string,
        public CreatedByLogin: string,
        public CreatedOn: any,
        public ModifiedOn: any
    ) {
        super();
    }
}

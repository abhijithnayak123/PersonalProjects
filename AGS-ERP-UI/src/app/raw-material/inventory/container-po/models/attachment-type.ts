import { BaseModel } from '../../../../shared/models/base.model';

export class AttachmentType extends BaseModel {
    constructor(
        public AttachmentTypeId: number,
        public AttachmentFor: string,
        public Code: string,
        public Description: string
    ) {
        super();
    }
}

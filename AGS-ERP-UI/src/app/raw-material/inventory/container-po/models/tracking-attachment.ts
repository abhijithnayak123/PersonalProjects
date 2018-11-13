import { BaseModel } from '../../../../shared/models/base.model';

export class TrackingAttachment extends BaseModel {
    constructor(
        public Action: number,
        public ContainerId: number,
        public ContainerNbr: string,
        public AttachmentId: number,
        public AttachmentTypeId: number,
        public AttachmentTypeCode: string,
        public AttachmentTypeDesc: string,
        public FileName: string,
        public DisplayName: string,
        public DownloadFileName: string,
        public CreatedByLogin: string,
        public ModifiedByLogin: string,
        public CreatedOn: Date,
        public ModifiedOn: Date
    ) {
        super();
    }
}

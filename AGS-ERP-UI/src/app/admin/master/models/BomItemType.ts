import { BaseModel } from '../../../shared/models/base.model';

export class BomItemType extends BaseModel {
    constructor(
        public ItemTypeId : number,
        public ItemTypeCode : string,
        public ItemTypeName : string,
    ) {
        super();
    }
}

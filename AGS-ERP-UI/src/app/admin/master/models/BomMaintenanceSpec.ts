import { BaseModel } from '../../../shared/models/base.model';

export class BomMaintenanceSpec extends BaseModel {
    constructor(
        public Id : number,
        public RevisionNumber : number,
        public FileName : string,
        public CreatedDate : Date
    ) {
        super();
    }
   
}
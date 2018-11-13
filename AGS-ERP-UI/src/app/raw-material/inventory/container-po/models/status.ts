import { BaseModel } from '../../../../shared/models/base.model';

export class Status extends BaseModel {
    constructor(
        public StatusCode : string,
        public StatusName : string,
        
    ) {
        super();
    }
}

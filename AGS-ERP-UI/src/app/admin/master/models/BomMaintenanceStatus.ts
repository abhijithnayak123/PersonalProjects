import { BaseModel } from '../../../shared/models/base.model';

export class BomMaintenanceStatus extends BaseModel {
    constructor(
       public Id : number,
       public Code : string,
       public Description : string,
    ) {
        super();
    }
}
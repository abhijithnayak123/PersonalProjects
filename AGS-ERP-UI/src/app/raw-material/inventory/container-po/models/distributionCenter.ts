import { BaseModel } from '../../../../shared/models/base.model';

export class DistributionCenter extends BaseModel {
    constructor() {
        super();
    }
    public Id : number;
    public Key : string;
    public Name : string;
}

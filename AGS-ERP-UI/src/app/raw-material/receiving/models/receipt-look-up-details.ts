import { BaseModel } from '../../../shared/models/base.model';
import { LookHeaderDetails } from './look-header-details';
import { LookUpGridDetails } from './lookup-details';

export class ReceiptLookUpDetails extends BaseModel {
    constructor() {
        super();
    }
    public LoopUpHeader: LookHeaderDetails;
    public Details: LookUpGridDetails[];
}

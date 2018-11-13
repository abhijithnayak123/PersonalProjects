import { BaseModel } from '../../../shared/models/base.model';
export class POStatus extends BaseModel {
    constructor(
        public POStatusId: number,
        public StatusCode: string,
        public Status:string
    ) {
        super();
    }
}

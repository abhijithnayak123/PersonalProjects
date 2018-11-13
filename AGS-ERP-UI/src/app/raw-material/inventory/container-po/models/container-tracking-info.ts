import { BaseModel } from '../../../../shared/models/base.model';
import { ContainerDetails } from './container-details';
import { CTHeader } from './ct-header';

export class ContainerTrackingInfo extends BaseModel {
    constructor(
        public HeaderDetails: CTHeader,
        public TrackingDetails: Array<ContainerDetails>,
        public Action: number
    ) {
        super();
    }
}

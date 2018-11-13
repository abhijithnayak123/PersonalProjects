import { BaseModel } from '../../../shared/models/base.model';

export class FutureWatchList extends BaseModel {
    constructor(
        public ItemId: string,
        public ItemNumber: string,
        public Description: string,
        public OnHand: number,
        public AvgWeeklyDemand: number,
        public MinWOS: number,
        public SafetyStock: number,
        public WEValues: Array<string>,
        public WELabels: Array<string>,
        public CSSList: Array<string>
    ) {
        super();
    }
}

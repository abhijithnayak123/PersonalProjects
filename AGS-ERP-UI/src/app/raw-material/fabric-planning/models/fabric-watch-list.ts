import { BaseModel } from '../../../shared/models/base.model';

export class FabricWatchList extends BaseModel {
    constructor(
        public ItemId: number,
        public ItemNumber: string,
        public Description: string,
        public FiberContent: string,
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

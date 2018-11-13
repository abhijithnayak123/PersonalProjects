import { BaseModel } from '../../../shared/models/base.model';

export class PlanningReport extends BaseModel {
    constructor(
        public ItemCode: string,
        public OnHand: number,
        public AvgWeeklyDemand: string,
        public Description: string,
        public MinWOS: number,
        public SafetyStock: number,
        public FiberContent: string,
        public WEDateLabel: Array<string>,
        public SchedRecpts: Array<number>,
        public UnallocCutReq: Array<number>,
        public FcstReq: Array<number>,
        public FcstReqEndInvt: Array<number>,
        public FcstReqWOS: Array<number>,
        public HistAvgReq: Array<number>,
        public HistReqEndInvt: Array<number>,
        public HistReqWOS: Array<number>,
        public ReportRequirementList: Array<string>,
        public FcstReqWOSCSS: Array<string>,
        public HistReqWOSCSS: Array<string>,
    ) {
        super();

    }
}

import { BaseModel } from '../../../shared/models/base.model';
import { Allocation } from './allocation.model';

export class ReturnRollcase extends BaseModel {
    constructor(
        public IsSelected: boolean,
        public ItemCode: string,
        public FiberContent: string,
        public RMVendorName: string,
        public RMVendorItemNumber: string,
        public Width: number,
        public BinLocation: any,
        public PulledQty: number,
        public CuttingVendorSiteId: number,
        public AllocationBatchVersionStamp: any[],
        public CutHDRId: number,
        public AllocationBatchId: number,
        public AllocationHDRVersionStamp: any[],
        public CutItemId: number,
        public MarkerYeild: number,
        public LoaderNumber: any,
        public MarkerQty: number,
        public COO: string,
        public RMVendorId: number,
        public RMVendorSiteId: number,
        public MillId: number,
        public LotNumber: string,
        public AllocationItemId: number,
        public AllocationItemVersionStamp: any[],
        public InventoryHDRId: number,
        public RollCaseId: number,
        public AllocationQty: number,
        public ConsumedQty: number,
        public ReturnedQty: number,
        public ReturnedToInventoryArea: string,
        public AdjustedQty: number,
        public AdjustedResonCodeId: number,
        public AllocationRollId: number,
        public AllocationRollVesrsionStamp: any[],
        public InventoryDTLId: number,
        public AllocationHDRId: number,
        public ActionType: Allocation,
        public ReturnedBy: string,
        public ReturnedDate: string,
        public RollCaseNumber: string
    ) {
        super();
    }
}

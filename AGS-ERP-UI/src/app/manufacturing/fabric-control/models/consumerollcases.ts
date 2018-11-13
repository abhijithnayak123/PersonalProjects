import { BaseModel } from '../../../shared/models/base.model';
import { LoaderNumber } from './loader-number.model';

export class ConsumeRollCase extends BaseModel {
    constructor(
        public Id: number,
        public ItemCode: string,
        public FiberContent: string,
        public Vendor: string,
        public RMVendorName: string,
        public RMVendorItemNumber: string,
        public RollCaseId: number,
        public Width: number,
        public BinLocation: string,
        public LotNumber: string,
        public AllocationQty: number,
        public PulledQty: number,
        public ConsumedQty: number,
        public ConsumedBy: string,
        public ConsumedDate: string,
        public RollCaseNumber: string,
        public IsSelected: boolean,
        public CutHDRId: number,
        public VendorItem: string,
        public AllocationItemId: number,
        public AllocationRollId: number,
        public CutItemId: number,
        public InventoryDTLId: number,
        public InventoryHDRId: number,
        public LoadNumber: LoaderNumber
    ) {
        super();
    }
}

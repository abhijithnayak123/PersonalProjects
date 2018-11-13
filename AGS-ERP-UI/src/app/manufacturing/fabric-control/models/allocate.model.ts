import { LoaderNumber } from "./loader-number.model";
import { Consumption } from "./consumption.model";
import { BaseModel } from "../../../shared/models/base.model";
import { Allocation } from "./allocation.model";

export class AllocateModel extends BaseModel {
    constructor(

        public Id: number,
        public RollCaseId: number,
        public BinLocationId: string,
        public AramarkItem: string,
        public Color: string,
        public FiberContent: string,
        public Yards: number,
        public Width: number,
        public ShadeLotNbr: string,
        public Vendor: string,
        public InventoryInDate: string,
        public AllocatonType: Allocation,
        public Allocated: number,
        public AllocatedBy: string,
        public AllocatedDate: string,
        public LoaderNumber: any,
        public ConsumptionType: Consumption,
        public UnUsedYards: number,
        public YardsConsumed: number,
        public DifferenceYards: number,
        public ConsignmentRoll: boolean,
        public IsSelected: boolean,
        public PulledBy: string,
        public PullTimeStamp: string,
        public VendorItem: string,
        public ReturnYards: number,
        public PulledQty: number
    ) {
        super();
    }
}

import { BaseModel } from "../../../../shared/models/base.model";
import { VendorDetails } from "./vendor-details.model";
import { AramarkItemDetails } from "./aramark-item-details.model";
import { ColorDetails } from "./color-details.model";
export class RollCasesModel extends BaseModel {
        public Id: number;
        public RollCaseId: number;
        public DCId: number;
        public BinId: number;
        public AramarkItemList: Array<AramarkItemDetails>;
        public AramarkItemCode: any;
        public ItemId: number;
        public ColorCode: any;
        public FiberContent: string;
        public VendorItem: string;
        public VendorList: Array<VendorDetails>;
        public Vendor: any;
        public VendorId: number;
        public LotNumber: string;
        public CountryOfOrigin: string;
        public Width: number;
        public OnHand: string;
        public Allocated: number;
        public Available: number;
        public Defective: string;
        public ReasonCodeId: any;
        public IsSelected: boolean;
        public IsModified: boolean;
        public IsAdded: boolean;
        public IsValid: boolean;
        public ColorDetailsList:Array<ColorDetails>;
}

import { BaseModel } from "../../../shared/models/base.model";

import { Vendor } from "./vendor.model"
import { Lot } from "./lot.model";
export class AllocateFilterRollCase extends BaseModel {
    constructor(
        public IsSelected: boolean,
        public CutHbrId: number,
        public CutItemId: number,
        public ItemId: number,
        public AramarkItem: string,
        public FiberContent: string,
        public Color: string,
        public LotNumber: string,
        public Available: number,
        public BOMYards: number,
        public MarkerYards: number,
        public Allocated: number,
        public BOMDiff: number,
        public MarkerDiff: number,
        public VendorSiteId: number,
        public VendorSite: string,
        public Vendors: Array<Vendor>,
        public LotNumbers: Array<Lot>,
        public CountryOfOrigin: string,
        public AvailableYards: number,
        public BOM: number,
        public Marker: number,
        public AllocatedQty: number,
        public Vendor: Vendor,
        public Lot: Lot,
        public DefaultLot: Lot,
        public SelectedLots: Array<string>,        
        public IsMarkerEnabled:boolean,
    ){
        super();
    }
}
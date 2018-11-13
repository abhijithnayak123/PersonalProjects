import { BaseModel } from '../../../shared/models/base.model';
import { BomItemType } from "./BomItemType";
import { BomAramarkItem } from "./BomAramarkItem";
import { BomDescription } from "./BomDescription";
import { BomSearchVendor } from "./BomSearchVendor";

export class BomMaintenanceSearch extends BaseModel {
    constructor() {
        super();
    }
    public ItemTypeList : Array<BomItemType>;
    public FilteredItemTypes : Array<BomItemType>;
    public SelectedItemType : BomItemType;
    public AramarkItemList : Array<BomAramarkItem>;
    public FilteredAramarkItems : Array<BomAramarkItem>;
    public SelectedAramarkItem : BomAramarkItem;
    public DescriptionList : Array<BomDescription>;
    public FilteredDescriptions : Array<BomDescription>;
    public SelectedDescription : BomDescription;
    public VendorList :  Array<BomSearchVendor>;
    public FilteredVendors :  Array<BomSearchVendor>;
    public SelectedVendor :  BomSearchVendor;
}
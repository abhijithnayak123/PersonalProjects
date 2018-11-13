import { BaseModel } from '../../../shared/models/base.model';
import { BomMaintenanceStyle } from "./BomMaintenanceStyle";
import { BomMaintenanceColor } from "./BomMaintenanceColor";
import { BomMaintenanceMfgVendor } from "./BomMaintenanceMfgVendor";
import { BomMaintenanceSpec } from "./BomMaintenanceSpec";
import { BomMaintenanceStatus } from "./BomMaintenanceStatus";
import { BomVersion } from './BomVersion';

export class BomMaintenanceHeader extends BaseModel {
    constructor(
       
    ) {
        super();
    }
    public Id : number;
    public StyleId: number;
    public Styles : Array<BomMaintenanceStyle>
    public FilteredStyles : Array<BomMaintenanceStyle>
    public SelectedStyle : BomMaintenanceStyle
    public ColorId: number;
    public Colors : Array<BomMaintenanceColor>
    public FilteredColors : Array<BomMaintenanceColor>
    public SelectedColor : BomMaintenanceColor
    public VendorId: number;
    public Vendors : Array<BomMaintenanceMfgVendor>
    public FilteredVendors : Array<BomMaintenanceMfgVendor>
    public SelectedVendor : BomMaintenanceMfgVendor
    public Versions : BomVersion[];
    public filteredVersions : Array<BomVersion>
    public selectedVersion : BomVersion
    public SpecId: number;
    public Specs : BomMaintenanceSpec[]
    public FilteredSpecs : BomMaintenanceSpec[]
    public SelectedSpec : BomMaintenanceSpec
    public StatusId: number;
    public Status : Array<BomMaintenanceStatus> 
    public MainStatusArray : Array<BomMaintenanceStatus> 
    public SelectedStatus : BomMaintenanceStatus
    public IsSpecChanged : boolean
    public IsModelChanged : boolean
    public IsStatusChanged : boolean
    public IsComponentsChanged : boolean
    public Model : string
    public IsInCreateState : boolean
    public EditBom : boolean
    public IsOldVersionSelected : boolean
}
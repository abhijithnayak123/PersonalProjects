import { BaseModel } from '../../../shared/models/base.model';
import { BomMaintenanceStyle } from "./BomMaintenanceStyle";
import { BomMaintenanceColor } from "./BomMaintenanceColor";
import { BomMaintenanceMfgVendor } from "./BomMaintenanceMfgVendor";

export class BomMaintenanceCopyBom extends BaseModel {
    constructor() {
        super();
    }
    public FromBom = new FROMBOM();
    public ToBom = new TOBOM();

}

class FROMBOM{
    public Styles : Array<BomMaintenanceStyle>
    public FilteredStyles : Array<BomMaintenanceStyle>
    public SelectedStyle : BomMaintenanceStyle
    public Colors : Array<BomMaintenanceColor>
    public FilteredColors : Array<BomMaintenanceColor>
    public SelectedColor : BomMaintenanceColor
    public MfgVendors : Array<BomMaintenanceMfgVendor>
    public FilteredMfgVendors : Array<BomMaintenanceMfgVendor>
    public SelectedMfgVendor : BomMaintenanceMfgVendor
    
}

class TOBOM{
    public FilteredStyles : Array<BomMaintenanceStyle>
    public Styles : Array<BomMaintenanceStyle> 
    public SelectedStyle : BomMaintenanceStyle
    public FilteredColors : Array<BomMaintenanceColor>
    public Colors : Array<BomMaintenanceColor>
    public SelectedColor : BomMaintenanceColor
    public MfgVendors : Array<BomMaintenanceMfgVendor>
    public FilteredMfgVendors : Array<BomMaintenanceMfgVendor>
    public SelectedMfgVendor : BomMaintenanceMfgVendor
    
}
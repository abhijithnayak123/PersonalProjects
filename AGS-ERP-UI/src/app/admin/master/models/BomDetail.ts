import { BaseModel } from '../../../shared/models/base.model';
import { BomFilteredGridData } from "./BomFilteredData";
import { BomMaintenanceHeader } from "./BomMaintenanceHeader";
import { BomVersion } from "./BomVersion";

export class BomDetail extends BaseModel {
    constructor(
       
    ) {
        super();
    }
    public BOMHeader = new BOmHeader();
    public BomComponents : Array<BomFilteredGridData>;

}

class BOmHeader {
    public Id : number
    public StyleId : number
    public ColorId : number
    public VendorId : number
    public SpecId : number
    public Model : string
    public StatusId : number
    public Versions : Array<BomVersion>
}
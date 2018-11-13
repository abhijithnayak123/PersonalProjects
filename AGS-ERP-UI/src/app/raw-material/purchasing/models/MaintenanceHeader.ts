import { BaseModel } from '../../../shared/models/base.model';
import { Data } from '@angular/router/src/config';
export class MaintenanceHeader extends BaseModel {
    constructor(
      
    ) { 
        super();
    } 
     public PONbr: number;
    public PODate: Date;
    public RequestedDate: Date
    public Vendor:string;
    public PickupLoc:string;
    public ShipTo:string;
    public POStatus:string;
    public POSentToVendor:string;
    public OrigSchedCreateDate: Date;
    public SchedCreateDate: Date;
    public ActualCreateDate: Date;
    public OrigSchedConfirmDate :Date;
    public SchedConfirmDate: Date;
    public ActualConfirmDate: Date;
    public OrigSchedShipDate: Date;
    public SchedShipDate: Date;
    public ActualShipDate: Date;
    public OrigSchedReceiptDate: Date;
    public SchedReceiptDate: Date;
    public ActualReceiptDate: Date;
    public POHdrID:number;
    public VendorId: number;
    public ShipFromVendorSiteId:number;
    public ItemId:number;
    public POStatusId:number;
    public OB : number;
    public IsPOCommunicated: boolean;
}

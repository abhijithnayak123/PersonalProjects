import { BaseModel } from "../../../shared/models/base.model";
import { Data } from "@angular/router/src/config";
export class POSearchHeader extends BaseModel {
  constructor() {
    super();
  }
  public POHdrId: number;
  public PONbr: number;
  public OrderDate: Date;
  public ItemNbr: string;
  public Vendor: string ;
  public PickupLoc: string;
  public SchedShipDate: Date    ;
  public ShipTo: string;
  public Yards: string;
  public UnitPrice: string;
  public ExtendedValue  : string;
  public POStatus: string;
  public ApprovedBy: string;
  public VendorId: number;
  public ShipFromVendorSiteId: number;
  public ItemId: number;
  public POStatusId: number;
  public ItemDescription: string;
}

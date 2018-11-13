import { BaseModel } from "../../../shared/models/base.model";
import { Data } from "@angular/router/src/config";
export class POMaintenance extends BaseModel {
  constructor() {
    super();
  }
  public POHDRId: number;
  public POStatusId: number;
  public SchedShipDate: Date;
  public SchedReceiptDate: Date;
  public ActualConfirmDate: Date;
  public ActualShipDate: Date;
  public ActualReceiptDate: Date;
  public POLineID: number;
  public POLineStatusID: number;
  public POPrice: number;
  public ConfirmYds: number;
  public IsSelected: boolean;
  public LineNbr: number;
  public ItemId: number;
  public VendorItemNbr: string;
  public QtyConfirmed: number;
  public QtyOrdered: number;
  public Amount: number;
  public UOM: string;
  public IsPOCommunicated: boolean;
}

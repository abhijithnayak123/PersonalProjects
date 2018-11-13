import { BaseModel } from '../../../shared/models/base.model';
import { Data } from '@angular/router/src/config';
export class MaintenanceGrid extends BaseModel {
    constructor(
      
    ) { 
        super();
    } 
    public ItemId:number;
    public POStatusId:number;
    public OB : number;
    public ItemCode:string;
    public ItemDesc:string;
    public VendorItemNbr:string;
    public Color:string;
    public FiberContent:string;
    public COO:string;
    public UOM:string;
    public UnitPrice:string;
    public QtyOrdered:number;
    public QtyConfirmed:string;
    public QtyShipped:number;
    public QtyReceived:number;
    public QtyOB : number;
    public Amount : number;
    public POLineId: number;
    public ColorId: number;
    public DeliveryDate:Date;
    public RequestedDate:Date;
    public VendorID:number;
    public LineNbr:number;
    public COMMENTS : string;
    public IsSelected:boolean = false;
}

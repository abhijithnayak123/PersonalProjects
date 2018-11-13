import { BaseModel } from "../../../shared/models/base.model";

export class PODetailModel extends BaseModel {
    public PONumber: string;
    public POHdrId: number;
    public AramarkItemId: number;
    public AramarkItemCode: string;
    public Color: string;
    public FiberContent: string;
    public Width: number;
    public VendorItem: string;
    public COO: string;
    public POLineId:number;
}
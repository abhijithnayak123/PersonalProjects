import { BaseModel } from '../../../shared/models/base.model';

export class BomMaintenanceSearchDB extends BaseModel {
    constructor() {
        super();
    }
    public ItemTypeId : number;
    public ItemTypeCode : string;
    public ItemTypeName : string;
    public ItemId : number;
    public ItemCode : string;
    public ItemDescritpion : string;
    public VendorId : number;
    public VendorCode : string;
    public VendorName : string;
    public ItemStatus : string;

}
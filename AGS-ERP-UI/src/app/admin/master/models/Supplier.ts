import { BaseModel } from '../../../shared/models/base.model';
import { COO } from './COO';
import { UoM } from './UoM';
import { SupplierVendor } from './SupplilerVendor';
import { SupplierVendorSite } from './SupplierVendorSite';

export class Supplier extends BaseModel {
        public Id: number;
        public ItemId : number;
        public ItemCode: string;
        public VendorId : number;
        public Vendor: SupplierVendor;
        public VendorName : string;
        public VendorList: SupplierVendor[];
        public VendorSiteId: number;
        public VendorSite: SupplierVendorSite;
        public VendorSiteList: SupplierVendorSite[];
        public VendorItem: string;
        public VendorStyle: string;
        public VendorColor: string;
        public VendorStatus: string[];
        public Width: number;
        public UOMId: number;
        public UnitPrice: number;
        public LeadTime: number;
        public COO: COO;
        public COOList: COO[];
        public UOMList: UoM[];
        public UOM: string;
        public Status: string;
        public AllocationPercentage: number;
        public Consignment: boolean;
        public StageId: number;
        public IsSelected: boolean;
        public IsAdded: boolean;
        public IsCreated: boolean;
        public IsAbsolete: boolean;
}

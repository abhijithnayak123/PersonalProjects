import { BaseModel } from '../../../shared/models/base.model';
import { Vendor } from './Vendor';
import { VendorStatus } from './VendorStatus';
import { Country } from './Country';
import {SupplierSiteDetails} from './SupplierSiteDetails';

export class VendorModel extends BaseModel {
    constructor() {
        super();
    }
        public Id:number;
        public Code:string;
        public Name: string;
        public ShortDescription: string;
        public Vendor: Vendor;
        public StatusId: number;
        public Status: string;
        public VendorStatus: VendorStatus;
        public Consignment: boolean;
        public ConsignmentTakeDays: number;
        public AddressLine1: string;
        public AddressLine2: string;
        public City: string;
        public State: string;
        public Country: string;
        public CountryId: number;
        public VendorCountry: Country;
        public Zip: number;

        public VendorList: Array<Vendor>;
        public StatusList: Array<VendorStatus>;
        public Countries: Array<Country>;
        public SupplierSiteDetails: Array<SupplierSiteDetails>;
        
        public isAddMode: boolean;
        public isEditMode: boolean;
}

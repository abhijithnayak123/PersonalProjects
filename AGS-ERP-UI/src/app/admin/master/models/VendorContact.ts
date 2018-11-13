import { BaseModel } from "../../../shared/models/base.model";
import { Site } from "./site";

export class VendorContact extends BaseModel {
    constructor() {
        super();
    }
        public Id: number;
        public SiteList: Site[];
        public Site: Site;
        public SiteName: string;
        public ContactName: string;
        public SiteId: number;
        public Email: string;
        public Role: string;
        public Phone: string;
        public Mobile: string;
        public Fax: string;
        public PrimaryContact: Boolean;
        public POContact: Boolean;
        public FinanceContact: Boolean;
        public VendorId: number;
        public IsSelected: Boolean;
        public IsAdded: Boolean;
}
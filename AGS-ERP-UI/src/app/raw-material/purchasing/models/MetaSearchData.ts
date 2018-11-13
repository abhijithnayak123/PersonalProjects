import { BaseModel } from '../../../shared/models/base.model';
import { POComboBox } from './POComboBox';
import { AramarkCombo } from '../../inventory/container-po/models/aramark.combo';
import { POSearchHeader } from "../models/POMaintenanceHeader";

export class MetaSearchData extends BaseModel {
    constructor(       
        public Vendor: string ,
        public PickupLoc: string,
        public PONbr: POComboBox,
        public ItemNbr: AramarkCombo,
        public PofromDate: Date,
        public PoToDate: Date,
        public POStatus: string,
        public vendors: Array<string>,
        public pickups: Array<string>,
        public pos:Array<POComboBox>,
        public aramarks:Array<AramarkCombo>,
        public status:Array<string>,
        public resultData: Array<POSearchHeader>
    ) {
        super();
    }
}

import { BaseModel } from "../../../shared/models/base.model";
import { PickUpNumber } from "./pickup-number.model";
import { VendorModel } from "./vendor.model";
import { AramarkItemCode } from "./aramark-item-code.model";
import { FiberContent } from "./fiber-content.model";
import { VendorItem } from "./vendor-item.model";
import { FabricContainer } from "./fabric-container.model";
import { PurchaseOrder } from "./purchase-order.model";

export class Search extends BaseModel{
    constructor(
        public ContainerNumber:FabricContainer,
        public PickUpNumber:PickUpNumber,
        public PONumber:PurchaseOrder,
        public TotalYards:number,
        public TotalRolls:number,
        public PickUpLocation:string,
        public VendorName:VendorModel,
        public AramarkItemCode:AramarkItemCode,
        public Color:string,
        public FiberContent:FiberContent,
        public VendorItem:VendorItem,
        public COO:string,
        public ContainerStatus: string
    ){
        super()
    }
}
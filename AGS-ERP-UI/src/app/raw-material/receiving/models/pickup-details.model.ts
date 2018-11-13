import { BaseModel } from "../../../shared/models/base.model";
import { ContainerModel } from "./container.model";
import { PickupModel } from "./pick-up.model";

export class PickupDetails extends BaseModel {
    public ContainerList: ContainerModel[] = [];
    public Containers: ContainerModel[] = [];
    public Container: ContainerModel;
    public PickupList: PickupModel[] = [];
    public Pickups: PickupModel[] = [];
    public Pickup: PickupModel;
    public VendorCode: string;
    public Shipment: string;
    public Location: string;
    public VendorName: string;
    public ASN: string;
    public BillOfLading: string;
    public TotalRolls: number;
    public ReceivedRolls: number;
    public CurrentReceivingRolls: number;
    public TotalYards: number;
    public ReceivedYards: number;
    public CurrentReceivingYards: number;
    public Comments: string;
    public ReceiptDate: Date;
    public PickupNumber: string;
    public ContainerStopId: number;
    public ShipmentHdrId: number;
    public ReceiptNumber: number;
    public ContainerNumber: string;
    public ContainerId:number;
}

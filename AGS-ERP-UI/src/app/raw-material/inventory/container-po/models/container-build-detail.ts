import { BaseModel } from '../../../../shared/models/base.model';
import { Group } from "./group";

export class ContainerBuildDetail extends BaseModel {
    constructor(
        public POLineId: number,
        public POHdrId: number,
        public VendorId: number,
        public VendorSiteId: number,
        public FGVendorId: number,
        public ReceiverDcId: number,
        public ItemVendorId: number,
        public POTypeId: number,
        public POStatusId: number,
        public POLineStatusId: number,
        public UOMId: number,
        public RMVendorCode: number,
        public RMVendorName: string,
        public VendorSite: string,
        public VendorSiteName: string,
        public PONbr: string,
        public LineNbr: number,
        public OrderDate: Date,
        public SchedShipDate: Date,
        public AvailableQty: string,
        public UnitPrice: number,
        public WeightPerYard: number,
        public ExtendedWeight: number,
        public ExtendedValue: number,
        public ItemCode: string,
        public ItemName: string,
        public UOMCode: string,
        public VendorItemNbr: string,
        public FiberContent: string,
        public CountryOfOrigin: string,
        public ColorCode: string,
        public ColorName: string,
        public CuttableWidth: number,
        public FGVendor: string,
        public FGVendorName: string,
        public ReceivingDcCode: string,
        public ReceivingDcName: string,
        public LeadTime : number,
        public POPrice : number,
        
        //added for purpose of approve
        public ContainerTypeId : number,
        public CarrierId : number,
        public ReceiverDCId : number,
        public ExpectedOriginDate  : Date,
        public ExpectedBorderDate : Date,
        public ContainerId  : number,
        public ContainerVersionStamp  : number[],
        public BatchGroupNumber  : number,
        public ContainerDTLId : number,
        public RequestedQty  : number,
        public ContainerDTLVersionStamp : number[],
        public BuildNbr :number,
        public containerStopId : number,
        public ContainerNbr:number,
        public ContainerBatchVersionStamp : number[],

        //params which are not in DB
        public PickupLocation: string,
        public IsSelected: boolean,
        public Weight: number,
        public DbQty : number,
        public IsInCurrentSelection : boolean,
        public Group : Group,
        public IsAdded : boolean,
        public IsRemoved : boolean,
        public IsModified : boolean,
        public ActionType : number
        

    ) {
        super();
    }
}

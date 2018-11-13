import { BaseModel } from '../../../shared/models/base.model';
import { Type} from './Type';
import { VendorStatus} from './VendorStatus';
import { Inco } from "./Inco";
import { Port } from "./Port";
import { PoCommunication } from "./PoCommunication";
import { DeliveryEvent } from "./DeliveryEvent";
import { Asn } from "./Asn";
import { Country } from "./Country";

export class SupplierSiteDetails extends BaseModel {
    constructor() {
        super();
    }

    public Id: number;
    public VendorId: number;
    public VendorName: string;
    public Code: string;
    public Name: string;
    public Description: string;
    public StatusId: number;
    public TypeId: number;
    public Type: Type;
    public TypesList : Type[];
    public FabricItem: boolean;
    public Trim: boolean;
    public SendForecast: boolean;
    public OrderToPickup: number;
    public Status: string;
    public VendorStatus: VendorStatus;
    public StatusList : VendorStatus[];
    public AddressLine1: string;
    public AddressLine2: string;
    public City: string;
    public State: string;
    public CountryId: number;
    public Country: Country;
    public Zip: number;
    public INCOId: number;
    public INCO: string;
    public DeliveryEventId: number;
    public DeliveryEvent: string;
    public IncoSite : Inco;
    public DeliveryEventSite: DeliveryEvent;
    public PortId: number;
    public Port: string;
    public PortSite: Port;
    public TPNumber: number;
    public POCommunicationId: number;
    public POCommunication: string;
    public POCommunicationSite: PoCommunication;
    public OTS: boolean;
    public ASNId: number;
    public ASN: string;
    public AsnSite: Asn;

    public IncoList: Inco[];
    public AsnList: Asn[];
    public DeliveryList: DeliveryEvent[];
    public CountryList: Country[];
    public Ports: Port[];
    public Communications: PoCommunication[];

    public isAdded: boolean;
    public isSelected: boolean;
    public recordNumber : number;

    public isAddMode: boolean = false;
    public isDisabled: boolean;

}

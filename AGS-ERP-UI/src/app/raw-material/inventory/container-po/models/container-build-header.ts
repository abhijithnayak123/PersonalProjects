import { BaseModel } from '../../../../shared/models/base.model';
import { Status } from "./status";
import { Build } from "./build";
import { FabricContainer } from "./fabric-container";
import { Carrier } from "./carrier";
import { ContainerType } from "./containertype";

export class ContainerBuildHeader extends BaseModel {
    constructor() {
        super();
    }

    public Status: Array<Status>;
    public SelectedStatus : Status;
    public Builds : Array<Build>;
    public FilteredBuilds : Array<Build>;
    public SelectedBuild : Build;
    public FabricContainers : Array<FabricContainer>;
    public FilteredFabricContainers : Array<FabricContainer>;
    public SelectedFabricContainer : FabricContainer;
    public Carriers : Array<Carrier>;
    public FilteredCarriers : Array<Carrier>;
    public SelectedCarrier : Carrier;
    public SelectedPickDate : Date;
    public ExpBorderDate : Date;
    public DestinationDc : string;
    public ContainerTypes : Array<ContainerType>;
    public FilteredContainerTypes : Array<ContainerType>;
    public SelectedContainerType  : ContainerType;
}

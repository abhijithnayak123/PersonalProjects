import { Component, OnInit, group } from '@angular/core';
import { ContainerBuildDetail } from '../../models/container-build-detail';
import { Group } from '../../models/group';
import { VendorDetails } from '../../../physical-adjustments/models/vendor-details.model';
import { ContainerType } from '../../models/containertype';
import { Container } from '@angular/compiler/src/i18n/i18n_ast';
import { PurchaseOrder } from '../../models/purchase-order';
import { PickupLocation } from '../../models/pickup-location';
import { process, GroupDescriptor, State, aggregateBy, SortDescriptor, orderBy } from '@progress/kendo-data-query';
import { ContainerPoService } from "../../container-po.service";
import { HttpErrorResponse } from "@angular/common/http";
import { SuccessService } from "../../../../../shared/services/success.service";
import { ErrorService } from "../../../../../shared/services/error.service";
import { ConfirmationService } from "../../../../../shared/services/confirmation.service";
import { AlertService } from "../../../../../shared/services/alert.service";
import { ToastService } from "../../../../../shared/services/toast.service";
import { Vendor } from "../../models/vendor";
import { GridDataResult } from "@progress/kendo-angular-grid/dist/es/data/data.collection";
import { Status } from "../../models/status";
import { Build } from "../../models/build";
import { FabricContainer } from "../../models/fabric-container";
import { Carrier } from "../../models/carrier";
import { AramarkItem } from "../../models/aramarkItem";
import { DistributionCenter } from "../../models/distributionCenter";
import { VendorOfContainerPipe } from "../../../../../shared/pipes/vendor-of-container.pipe";

@Component({
  selector: 'app-container-build',
  templateUrl: './container-build.component.html',
  styleUrls: ['./container-build.component.css'],
  providers: [ContainerPoService]
})

export class ContainerBuildComponent implements OnInit {
  vendors: Array<Vendor>;
  filteredVendors : Array<Vendor>;
  selectedVendor: Vendor;
  pickupLocations: Array<PickupLocation>;
  filteredPickupLocn : Array<PickupLocation>;
  selectedPickupLocation: PickupLocation;
  selectedReqDate : Date ;
  purchaseOrders: Array<PurchaseOrder>;
  filteredPos: Array<PurchaseOrder>;
  filterGridData: GridDataResult = {data: [],total : 0}
  currentSelectionGridData: GridDataResult = {data: [],total : 0}
  removedHoldRecords : Array<ContainerBuildDetail>;
  removedApprovedRecords : Array<ContainerBuildDetail>;
  status: Array<Status> = [new Status('N','New'),new Status('H','Hold'),new Status('A','Approved')]
  selectedStatus : Status;
  selectedBuild : Build;
  builds : Array<Build>;
  filteredBuilds : Array<Build>;
  fabricContainers : Array<FabricContainer>;
  filteredFabricContainers : Array<FabricContainer>;
  selectedContainer : FabricContainer;
  carriers : Array<Carrier>;
  filteredCarriers : Array<Carrier>;
  selectedCarrier : Carrier;
  selectedPickDate : Date;
  expBorderDate : Date;
  destinationDc : string = '';
  groups: Array<Group> = [];
  selectedGroup: Group;
  show: boolean;
  filterSort: SortDescriptor[] = [];
  currentSort : SortDescriptor[] = [];
  destinationDCs : Array<DistributionCenter>
  selectedDc : DistributionCenter

  containerDetails: Array<ContainerBuildDetail> = [];
  items: Array<AramarkItem>;
  filteredItems: Array<AramarkItem>;
  selectedItem: AramarkItem;
  containerTypes: Array<ContainerType>;
  filteredContainerTypes: Array<ContainerType>;
  selectedContainerType: ContainerType;
  selectedPurchaseOrder: PurchaseOrder;
  IsAllSelected: boolean;
  value: Date = new Date();
  totalquantity: number = 0;
  totalextendedvalue: number = 0;
  totalweight: number = 0;
  dbCurrentSelectionLength : number;
  successBuild : Build;

  constructor(
    private _containerPoService: ContainerPoService,
    private _successService: SuccessService,
    private _errorService: ErrorService,
    private _confirmationService: ConfirmationService,
    private _alertService: AlertService,
    private _toastService: ToastService,
    private _contVendorName : VendorOfContainerPipe
  ) { }

  ngOnInit() {
    this.getVendors();
  }
  
  /**
   * Format the date to mm-dd-yyyy
   * @param date to be formated
   */
  formatTheDate(dateToFormat : Date){
    let date = dateToFormat.getDate();
    let month = dateToFormat.getMonth()+1;
    let year = dateToFormat.getFullYear();
    let formatedDate = month+'-'+date+'-'+year;
    return formatedDate;
  }
  /**
   * Get the Vendors
   */
  getVendors() {
    this.show = true;
    this._containerPoService.containerBuildGetVendors().subscribe(
      data =>{
        this.vendors = this.filteredVendors = data;
        this.show = false;
        if(this.vendors.length === 1){
          this.selectedVendor = this.vendors[0];
          this.onVendorChange(this.selectedVendor);
        }
      },
      (err: HttpErrorResponse) => {
        this.show = false;
        this._errorService.error(err);
      }
    );
  }

  /**
   *
   * @param selectedVendor
   */
  onVendorChange(selectedVendor){
    if(selectedVendor){
    this.selectedPickupLocation = null;
    this.purchaseOrders = [];
    this.items = this.filteredItems =[];
    this.selectedPurchaseOrder = null;
    this.selectedItem = null;
    this.selectedPurchaseOrder = null;
    this.selectedReqDate = null;
    this.getPickupLocations(selectedVendor);
    }
  }

  /**
   * 
   * @param selectedVendor Get Pickup Locations for the selected vendor
   */
  getPickupLocations(selectedVendor : Vendor){
    this.show = true;
    if(selectedVendor){
    this._containerPoService.containerBuildGetPickupLocations(selectedVendor).subscribe(
      data =>{
        this.pickupLocations = this.filteredPickupLocn = data;
        this.show = false;
        if(this.pickupLocations.length === 1){
          this.selectedPickupLocation = this.pickupLocations[0];
          this.onPickupLocnChange();
        }
      },
      (err: HttpErrorResponse) => {
        this.show = false;
        this._errorService.error(err);
      }
    )
    }
   this.show = false;
  }

  /**
   * On change of Pickup Location get the PO data
   */
  onPickupLocnChange(){
    if(this.selectedPickupLocation){
      this.getPOdata();
    }
  }

  /**
   * On change of Requested Date get the PO data
   * @param event 
   */
  onRequestedDateChange(event){
    this.getPOdata();
  }

  /**
   * Get Purchase Orders(PO)
   */
  getPOdata(){
    this.show = true;
    if(this.selectedVendor && this.selectedPickupLocation && this.selectedReqDate){
      let selectedDate = this.formatTheDate(this.selectedReqDate);
      this._containerPoService.containerBuildGetPOs(this.selectedPickupLocation,selectedDate).subscribe(
        data =>{
          this.purchaseOrders= this.filteredPos = data;
          if(this.purchaseOrders.length === 1){
            this.selectedPurchaseOrder = this.purchaseOrders[0];
            this.getItems();
            this.show = false;
          }
        },
        (err: HttpErrorResponse) => {
        this.show = false;
        this._errorService.error(err);
      }
      )
    }
    else{
      this.show = false;
    }
    this.show = false;
  }

  /**
   * On change of PO Validate Search form before getting PO lines
   */
  onPOChange(){
    if(this.filterGridData.data.length > 0){
      this.validateSearchForm();
    }
  }

  /**
   * get Aramark Items
   */
  getItems(){
    this.show = true;
    if(this.selectedPurchaseOrder){
      let selectedDate = this.formatTheDate(this.selectedReqDate);
      this._containerPoService.containerBuildGetItems(this.selectedPurchaseOrder).subscribe(
        data =>{
         this.items = this.filteredItems = data;
          this.show = false;
          if(this.items.length === 1){
            this.selectedItem =  this.items[0];
          }
        },
        (err: HttpErrorResponse) => {
          this.show = false;
          this._errorService.error(err);
        }
      )
    }
    this.show = false;
  }
 
  /**
   * Validation of search form before getting filter grid data
   */
  validateSearchForm(){
    let errorInfo: Array<string> = [];
    let rollError: string;
    if (!this.selectedVendor) {
      errorInfo.push("Vendor");
    }
    if (!this.selectedPickupLocation) {
      errorInfo.push("Pickup Location");
    }
    if (!this.selectedReqDate) {
      errorInfo.push("Req Ship Date");
    }
    rollError = errorInfo.join(", ");
    if (rollError) {
      this._toastService.error("Kindly enter the required field(s):  " + rollError);
    }
    else {
      this.filterGridData.data = [];
      this.GetFilterData();
    }
  }

  /**
   * 
   */
  clearSearchForm(){
    this.selectedVendor = null;
    this.filteredVendors = this.vendors;
    this.selectedPickupLocation = null;
    this.filteredPickupLocn = this.pickupLocations = [];
    this.filteredPos = this.purchaseOrders = [];
    this.selectedPurchaseOrder = null;
    this.items = this.filteredItems = [];
    this.selectedItem = null;
    this.selectedReqDate = null;
  }

  updateExtValAndWeight(row : ContainerBuildDetail){
    if(Number(row.AvailableQty.replace(/,/g,'')) == 0){
      this._toastService.error("Quantity field  can't be Zero");
    }
    else{
      row.AvailableQty = Number(row.AvailableQty.replace(/,/g,'')).toLocaleString();
      row.Weight = Math.floor(row.WeightPerYard * Number(row.AvailableQty.replace(/,/g,'')));
      row.ExtendedValue = Math.floor(row.POPrice * Number(row.AvailableQty.replace(/,/g,'')));
    }
  }

  GetFilterData(){
    this.show = true;
    let reqdate = this.formatTheDate(this.selectedReqDate);
    let itemVendorId = 0;
    let poHdrId = 0;
    if(this.selectedPurchaseOrder){
      poHdrId = this.selectedPurchaseOrder.POHdrId;
    }
    if(this.selectedItem){
      itemVendorId = this.selectedItem.ItemVendorId;
    }
    this._containerPoService.containerBuildGetPOLines(this.selectedPickupLocation.VendorSiteId,reqdate,itemVendorId,poHdrId).subscribe(
      data => {
        data.forEach(element => {
          element.PickupLocation = this.selectedPickupLocation.PickupLocnName;
          element.Weight = Math.floor(Number(element.AvailableQty) * element.WeightPerYard);
          element.AvailableQty = Number(element.AvailableQty).toLocaleString();
          element.DbQty = Number(element.AvailableQty.replace(/,/g,''));
          if(this.currentSelectionGridData.data.find(p => p.POLineId === element.POLineId)){
            element.IsInCurrentSelection = true;
          }
          if((this.selectedStatus.StatusCode === 'N' || this.selectedStatus.StatusCode === 'H') && element.IsInCurrentSelection === true){
            element.IsSelected = true;
          }
          this.filterGridData.data.push(element);
        });
        this.show = false;
      },
       (err: HttpErrorResponse) => {
        this.show = false;
        this._errorService.error(err);
      }
    )
  }

  populateCurrentSelectionGrid(filterGridRow : ContainerBuildDetail){
    if(Number(filterGridRow.AvailableQty) == 0){
      this._toastService.error("Quantity field  can't be Zero");
    }
    else{
      if(this.selectedContainer && this.selectedStatus.StatusCode === 'A'){
        let groupsWeight = Number(this.getWeightTotal().replace(/,/g,''));
        if(groupsWeight + filterGridRow.Weight > 42500){
          this._toastService.error("Container will be overloded. Can't add this Weight");
          return false;
        }
      }
      filterGridRow.IsInCurrentSelection = true;
      filterGridRow.IsSelected = true;
      let row = this.currentSelectionGridData.data.find(r => r.POLineId === filterGridRow.POLineId);
      if(row){
        row.AvailableQty = (Number(row.AvailableQty.replace(/,/g,'')) + Number(filterGridRow.AvailableQty.replace(/,/g,''))).toLocaleString();
        row.Weight = Math.floor(row.Weight + filterGridRow.Weight);
        row.ExtendedValue = Math.floor(Number(row.AvailableQty.replace(/,/g,'')) * row.POPrice);
        row.Group = null;
        row.IsModified = true;
        filterGridRow.IsModified = true;
      }
      else{
      filterGridRow.IsAdded = true;
      this.currentSelectionGridData.data.push(filterGridRow);
      }
      this.getGroups();
      if(this.groups.length === 1){
        this.loadWeight(this.groups[0],this.currentSelectionGridData.data[this.currentSelectionGridData.data.length-1]);
      }
    }
  }

  confirmRemovalFromCurrentSelGrid(currentSelectionRow : ContainerBuildDetail){
     this._confirmationService.confirm({
      key: 'message',
      value: {
        message: 'Are you sure you want to remove the record from Current Selection?',
        continueCallBackFunction: () => this.removeFromCurrentSelectionGrid(currentSelectionRow)
      }
    });
  }

  removeFromCurrentSelectionGrid(currentSelectionRow : ContainerBuildDetail){
    var containerId = 0;
    if(currentSelectionRow.Group){
      let grp = this.groups.find(g => g.Id === currentSelectionRow.Group.Id);
      grp.Weight = Math.floor(grp.Weight - currentSelectionRow.Weight);
    }
    let index = this.currentSelectionGridData.data.indexOf(currentSelectionRow);
    currentSelectionRow.Group = null;
    if(this.currentSelectionGridData.data.length === 1){
      containerId = currentSelectionRow.ContainerId;
    }
    this.currentSelectionGridData.data.splice(index, 1);
    if(!currentSelectionRow.IsAdded && this.selectedStatus.StatusCode === 'H'){
      this.removedHoldRecords.push(currentSelectionRow);
    }
    if(!currentSelectionRow.IsAdded && this.selectedStatus.StatusCode === 'A'){
      this.removedApprovedRecords.push(currentSelectionRow);
    }
    let ratio = this.getRatio();
    if(this.groups.length > Math.ceil(ratio)){
      let numOfGrpsToRemove = this.groups.length - Math.ceil(ratio);
      for(var i = 0 ; i < numOfGrpsToRemove ; i++){
       let lastGroup = this.groups[this.groups.length-1];
       let rowWithThisGroup = this.currentSelectionGridData.data.filter(r => r.Group == lastGroup);
       if(rowWithThisGroup.length > 0){
          rowWithThisGroup.forEach(row => {
            row.Group = null;
          });
       }
       this.groups.pop();
      }
    }
    if(this.currentSelectionGridData.data.length === 0){
      this.groups = [];
    }
    const filterRow = this.filterGridData.data.find(r => r.POLineId == currentSelectionRow.POLineId);
    if (filterRow) {
      filterRow.IsInCurrentSelection = false;
      filterRow.IsRemoved = true;
      filterRow.IsModified = false;
      if(this.selectedStatus.StatusCode === 'N' || this.selectedStatus.StatusCode === 'H'){
        filterRow.AvailableQty = filterRow.DbQty.toLocaleString();
        filterRow.Weight = Math.floor(filterRow.DbQty * filterRow.WeightPerYard);
      }
      else{
        if(filterRow.IsAdded){
          filterRow.AvailableQty = filterRow.DbQty.toLocaleString();
          filterRow.Weight = Math.floor(filterRow.DbQty * filterRow.WeightPerYard);
        }
        else{
        filterRow.AvailableQty = (Number(filterRow.AvailableQty.replace(/,/g,'')) + Number(currentSelectionRow.AvailableQty.replace(/,/g,''))).toLocaleString();
        filterRow.Weight = Math.floor(Number(filterRow.AvailableQty.replace(/,/g,'')) * filterRow.WeightPerYard);
        }
      }
      filterRow.ExtendedValue = Math.floor(Number(filterRow.AvailableQty.replace(/,/g,'')) * filterRow.POPrice);
      filterRow.IsSelected = false;
    }
  }

  getQtyTotal(){
    return this.currentSelectionGridData.data.map(el =>Number(el.AvailableQty.replace(/,/g,''))).reduce((sum,qty) => sum+qty , 0).toLocaleString();
  }

  getExtendedTotal(){
    return Math.floor(this.currentSelectionGridData.data.map(el => el.ExtendedValue).reduce((sum,ext) => sum+ext,0)).toLocaleString();
  }

  getWeightTotal(){
    return Math.floor(this.currentSelectionGridData.data.map(el => el.Weight).reduce((sum,weight)=>sum+weight,0)).toLocaleString();
  }

  getRatio(){
    return Number(this.getWeightTotal().replace(/,/g,''))/42500
  }

  onStatusChange(currentVal : Status, prevVal : Status, isHoldSuccess : boolean){
    if(this.currentSelectionGridData.data.length > 0){
      this._confirmationService.confirm({
          key: 'message',
          value: {
            message: 'This Page contain unsaved data. Would you like to perform navigation?<br> Press "OK" to continue or "Cancel" to abort the navigation',
            continueCallBackFunction: () => this.resetFieldsForHoldAndApprove(currentVal,isHoldSuccess),
            cancelCallBackFunction: () => this.preventStatusChange(prevVal)
          }
        });
    }
    else{
      this.selectedStatus = currentVal;
      if(currentVal.StatusCode === 'N'){
          this.getCarrierdata();
          this.getContainerTypes();
          this.getDcs();
          this.builds = this.filteredBuilds = [];
          this.fabricContainers = this.filteredFabricContainers = [];
          this.selectedBuild = null;
          this.selectedContainerType = null;
          this.selectedVendor = null;
          this.pickupLocations = [];
          this.selectedPickupLocation = null;
          this.purchaseOrders = [];
          this.selectedPurchaseOrder = null;
          this.selectedReqDate = null;
          this.items = this.filteredItems = [];
          this.selectedItem = null;
          this.filterGridData.data = [];
          this.currentSelectionGridData.data = [];
          this.destinationDc = '';
          this.selectedPickDate = null;
          this.expBorderDate = null;
      }
      else if(currentVal.StatusCode === 'H' || currentVal.StatusCode === 'A'){
          this.resetFieldsForHoldAndApprove(currentVal,isHoldSuccess);
       }
    }
  }

  preventStatusChange(prevVal : Status){
    this.selectedStatus = new Status(prevVal.StatusCode,prevVal.StatusName);
  }

  resetFieldsForHoldAndApprove(selectedStaus , isHoldSuccess : boolean){
      this.fabricContainers = this.filteredFabricContainers = [];
      this.selectedContainer = null;
      this.selectedContainerType = null;
      this.selectedCarrier = null;
      this.selectedDc = null;
      this.selectedPickDate = null;
      this.expBorderDate = null;
      this.selectedBuild = null;
      this.destinationDc = '';
      this.getCarrierdata();
      this.getContainerTypes();
      this.getDcs();
      this.getVendors();
      if(this.selectedStatus){
      this.selectedStatus = selectedStaus;
      if(selectedStaus.StatusCode === 'H' || selectedStaus.StatusCode === 'A'){
        this.getBuildData(selectedStaus,isHoldSuccess);
      }
      }
      this.selectedVendor = null;
      this.pickupLocations = [];
      this.filteredPickupLocn = [];
      this.selectedPickupLocation = null;
      this.purchaseOrders = [];
      this.filteredPos = [];
      this.selectedPurchaseOrder = null;
      this.items = this.filteredItems = [];
      this.selectedItem = null;
      this.selectedReqDate = null;
      this.filterGridData.data = [];
      this.currentSelectionGridData.data = [];
      this.groups = [];
  }

  getBuildData(selectedStaus : Status , isHoldSuccess : boolean){
    this.show = true;
    this.selectedStatus = selectedStaus;
    this._containerPoService.containerBuildGetBuildData(selectedStaus.StatusCode).subscribe(
        data =>{
          this.builds = this.filteredBuilds =  data;
          if(isHoldSuccess){
            if(this.successBuild){
              this.selectedBuild = this.successBuild;
            }
            else{
              this.selectedBuild = this.builds[this.builds.length - 1];
            }
            this.successBuild = null;
            this.onBuildChange(this.selectedBuild);
          }
          else if(this.builds.length === 1){
            this.selectedBuild = this.builds[0];
            this.onBuildChange(this.selectedBuild);
          }
          this.show = false;
        },
        (err: HttpErrorResponse) => {
          this.show = false;
          this._errorService.error(err);
        }
    )
  }

  onBuildChangeValidation(currentBuild : Build, prevBuild : Build){
    if(currentBuild){
      if((this.currentSelectionGridData.data.length > 0 && this.selectedStatus.StatusCode !== 'A') || (this.selectedStatus.StatusCode === 'A' && !this.disableButton())){
        this._confirmationService.confirm({
          key: 'message',
          value: {
            message: 'This Page contain unsaved data. Would you like to perform navigation?<br> Press "OK" to continue or "Cancel" to abort the navigation',
            continueCallBackFunction: () => this.onBuildChange(currentBuild),
            cancelCallBackFunction: () => this.preventBuildChange(prevBuild)
          }
        });
      }
      else{
        this.onBuildChange(currentBuild);
      } 
    }
  }

  onBuildChange(selectedBuild : Build){
    this.selectedBuild = selectedBuild;
    this.selectedCarrier = null;
    this.selectedContainerType = null;
    this.selectedDc = null;
    this.selectedPickDate = null;
    this.expBorderDate = null;
    this.currentSelectionGridData.data = [];
    this.groups = [];
    this.filterGridData.data = [];
    this.selectedVendor = null;
    this.pickupLocations = this.filteredPickupLocn = [];
    this.selectedPickupLocation = null;
    this.purchaseOrders = this.filteredPos = [];
    this.selectedPurchaseOrder = null;
    this.items = this.filteredItems = [];
    this.selectedItem = null;
    if(selectedBuild.BatchStatus === 'A'){
      this.getFabricContainerData(selectedBuild);
    }
    if(selectedBuild.BatchStatus === 'H'){
      this.getCurrentSelectionGrid();
    }
  }

  preventBuildChange(prevBuild : Build){
    this.selectedBuild = new Build(prevBuild.BuildNbr,prevBuild.ContainerCount,prevBuild.CreatedOn,prevBuild.CreatedBy,prevBuild.BatchStatus);
  }

  getDcs(){
    this.destinationDCs =  this._containerPoService.containerBuildGetDCs();
    this.selectedDc = this.destinationDCs[0];
  }
  setHeaderValues(){
    let rowInCurrentGrid = this.currentSelectionGridData.data[0];
    this.getCarrierdata();
    this.getContainerTypes();
    this.getDcs();
    this.selectedCarrier =  this.carriers.find(c => c.CarrierId === rowInCurrentGrid.CarrierId);
    this.selectedContainerType = this.containerTypes.find( c => c.ContainerTypeId === rowInCurrentGrid.ContainerTypeId);
    this.selectedDc = this.destinationDCs[0];
    let dbPickDate = new Date(rowInCurrentGrid.ExpectedOriginDate);
    let dbBorderDate = new Date(rowInCurrentGrid.ExpectedBorderDate);
    this.selectedPickDate = dbPickDate;
    this.expBorderDate = dbBorderDate;
  }

  getCurrentSelectionGrid(){
    this.show = true;
    this.groups = [];
    this.removedApprovedRecords = [];
    this.removedHoldRecords = [];
    this._containerPoService.containerBuildGetCurrentSelectionData(this.selectedBuild,this.selectedContainer).subscribe(
      data => {
        let dataToGrid = [];
        let grp : Group;
        data.forEach(element => {
          element.AvailableQty = element.RequestedQty.toLocaleString();
          element.Weight = Math.floor(element.RequestedQty * element.WeightPerYard);
          grp = new Group(element.BatchGroupNumber,""+element.BatchGroupNumber+"",element.Weight);
          element.Group = grp;
          if(!this.groups.find(g => g.Id == grp.Id)){
              this.groups.push(grp);
            }
          else{
            let gr= this.groups.find(g => g.Id == grp.Id);
            let a = gr.Weight + grp.Weight;
            gr.Weight = Math.floor(a);
          }
          dataToGrid.push(element);
        });
        this.currentSelectionGridData.data = dataToGrid;
        this.setHeaderValues();
        this.getGroups();
        this.show = false;
      },
      (err: HttpErrorResponse) => {
        this.show = false;
        this._errorService.error(err);
      }
    )
  }

  getFabricContainerData(selectedBuild : Build){
    this.show = true;
    this.selectedContainer = null;
    this._containerPoService.containerBuildGetFabricContainers(selectedBuild.BuildNbr).subscribe(
      data => {
        this.fabricContainers= this.filteredFabricContainers = data;
        if(this.fabricContainers.length === 1){
          this.selectedContainer = this.fabricContainers[0];
          this.onFabricContainerChange(this.fabricContainers[0]);
        }
        this.show = false;
      },
      (err: HttpErrorResponse) => {
        this.show = false;
        this._errorService.error(err);
      }
    )
  }

  onFabricContainerChangeValidation(currentVal,prevVal){
    if(currentVal){
      if(this.currentSelectionGridData.data.length > 0){
        this._confirmationService.confirm({
          key: 'message',
          value: {
            message: 'This Page contain unsaved data. Would you like to perform navigation?<br> Press "OK" to continue or "Cancel" to abort the navigation',
            continueCallBackFunction: () => this.onFabricContainerChange(currentVal),
            cancelCallBackFunction: () => this.preventContainerChange(prevVal)
          }
        });
      }
      else{
        this.onFabricContainerChange(currentVal);
      }
    }
  }

  onFabricContainerChange(selectedContainer){
    this.selectedContainer = selectedContainer;
    this.getCurrentSelectionGrid();
  }

  preventContainerChange(prevContainer : FabricContainer){
    this.selectedContainer = new FabricContainer(
      prevContainer.BuildNbr,
      prevContainer.ReceiverDcId,
      prevContainer.ContainerId,
      prevContainer.CarrierId,
      prevContainer.CarrierCode,
      prevContainer.CarrierName,
      prevContainer.ReceivingDcCode,
      prevContainer.ReceivingDcName,
      prevContainer.ContainerType,
      prevContainer.ContainerTypeDescription,
      prevContainer.ExpectedPickupDate,
      prevContainer.ExpectedBorderDate,
      prevContainer.BatchGroupNumber,
      prevContainer.ContainerNbr,
      prevContainer.VendorNames,
      prevContainer.Vendor,
    )
  }

  generateTrucksForHoldAndApprove(){
    let groups : Array<Group> = [];
    let newGroups : Array<Group> = [];
    for(var i = 0; i < this.currentSelectionGridData.data.length ; i ++){
      let currentObj = this.currentSelectionGridData.data[i];
      currentObj.Group.Id = i;
      currentObj.Group.Code = i;
      currentObj.Group.Weight = Math.floor(currentObj.WeightPerYard * currentObj.AvailableQty);
      groups.push(currentObj);
    }
    groups.forEach(element => {
      let grp = newGroups.find( g => g.Id === element.Id);
      if(grp){
        let a = grp.Weight + element.Weight
        grp.Weight = Math.floor(a);
      }
      else{
        newGroups.push(element);
      }
    });
    this.groups = newGroups;
  }

  getCarrierdata(){
    this.show = true;
    this._containerPoService.containerBuildGetCarrierData().subscribe(
      data => {
        this.carriers =this.filteredCarriers =  data;
        if((this.selectedStatus.StatusCode != 'A') && (this.selectedStatus.StatusCode != 'H')){
           if(this.carriers.length === 1){
           this.selectedCarrier = this.carriers[0];
         }
        }
        this.show = false;
      },
       (err: HttpErrorResponse) => {
        this.show = false;
        this._errorService.error(err);
      }
    )
  }

  getContainerTypes(){
    this.show = true;
    this._containerPoService.containerBuildGetContainerTypes().subscribe(
      data => {
        this.containerTypes = this.filteredContainerTypes= data;
        if((this.selectedStatus.StatusCode != 'A') && (this.selectedStatus.StatusCode != 'H')){
          if(this.containerTypes.length === 1){
            this.selectedContainerType = this.containerTypes[0];
          }
        }
        this.show = false;
      },
      (err: HttpErrorResponse) => {
        this.show = false;
        this._errorService.error(err);
      }
    )
  }

  getGroups() {
    let num : number = Number(this.getWeightTotal().replace(/,/g,''))/42500;
    let grps = this.createGroups(Math.ceil(num));
    grps.forEach(element => {
      this.groups.push(element);
    });
  }

  createGroups(noOfGroupsToCreate : number){
    let groups : Array<Group> = [];
    if(noOfGroupsToCreate <= 5){
      let currentGrpCount = this.groups.length;
      for(var i = (currentGrpCount+1); i <= noOfGroupsToCreate ; i++){
        let g = new Group(i,""+i+"",0);
        if(this.groups.filter(gr => gr.Id == g.Id).length === 0){
          groups.push(g);
        }
      }
    }
    else{
      this._toastService.error("One Build can't contain more than 5 containers");
      let addedRow = this.currentSelectionGridData.data[this.currentSelectionGridData.data.length-1];
      this.filterGridData.data.find(r => r.POLineId === addedRow.POLineId).IsInCurrentSelection = false;
      this.currentSelectionGridData.data.pop();
    }
    return groups;
  }

  loadWeight(event : Group,row : ContainerBuildDetail){
    if(row.Group){
      let a = this.groups.find(g => g.Id === row.Group.Id);
      if((a.Weight - row.Weight) <= 0){
        a.Weight = 0;
      }
      else{
        a.Weight = Math.floor(a.Weight - row.Weight);
      }
    }
    row.Group = event;
    let g = this.groups.find(g => g.Id === event.Id);
    g.Weight = Math.floor(g.Weight + row.Weight);
  }

  getClass(weight){
    if(weight === 0){
      return 'font-bold truck-estimation-item mb-0';
    }
    else if(weight > 0 && weight <= 42500){
      return 'font-bold truck-estimation-item truck-estimation-item-occupied mb-0';
    }
    else if(weight > 42500){
      return 'font-bold truck-estimation-item truck-estimation-item-overweight mb-0';
    }
    else{
      return '';
    }
  }

  onApprove(){
    let errorInfo: Array<string> = [];
    let rollError: string;
    if (!this.selectedCarrier) {
      errorInfo.push("Carrier");
    }
    if(!this.selectedContainerType){
      errorInfo.push("Container Type")
    }
    if (!this.selectedPickDate) {
      errorInfo.push("Pickup Date");
    }
    if (!this.expBorderDate) {
      errorInfo.push("Expected Border Date");
    }
    let a =this.currentSelectionGridData.data.filter(r => !r.Group);
    if(a.length > 0){
      errorInfo.push("Container for all records");
    }

    rollError = errorInfo.join(", ");
    if (rollError) {
      this._toastService.error("Kindly enter missing fields: " + rollError);
    }
    else {
     if(this.groups.filter(g => g.Weight > 42500).length > 0){
       this._toastService.error("Container(s) your trying to create exceeds Container Capacity.  Please re-assign groups and try again.");
     }
     else{
         this._confirmationService.confirm({
          key: 'message',
          value: {
          message: 'Are you sure you want to Create the Container(s)?',
          continueCallBackFunction: () => this.createContainer()
          }
        });
      }
    }

  }

  createContainer(){
    this.show = true;
    this.currentSelectionGridData.data.forEach(row => {
      if(this.selectedBuild){
        row.BuildNbr = this.selectedBuild.BuildNbr;
      }
      if(this.selectedContainer){
        row.ContainerId = this.selectedContainer.ContainerId;
      }
      row.ContainerTypeId = this.selectedContainerType.ContainerTypeId;
      row.CarrierId = this.selectedCarrier.CarrierId;
      row.ExpectedOriginDate = this.formatTheDate(this.selectedPickDate);
      row.ExpectedBorderDate = this.formatTheDate(this.expBorderDate);
      row.RequestedQty = Number(row.AvailableQty.replace(/,/g,''));
      row.BatchGroupNumber = row.Group.Id;
      if(this.selectedStatus.StatusCode === 'N' && !this.selectedBuild){
        row.ActionType = 20;
      }
      if(this.selectedStatus.StatusCode === 'H' && this.selectedBuild){
        row.ActionType = 31;
      }
      if(this.selectedStatus.StatusCode === 'A' && this.selectedBuild && this.selectedContainer){
        row.ActionType = 35;
      }
    });
     this._containerPoService.containerBuildCreateContainers(this.currentSelectionGridData.data).subscribe(
      data => {
        if(data){
          this._toastService.success("Container(s) created Successfully");
          this.selectedStatus = null;
          this.resetFieldsForHoldAndApprove(this.selectedStatus,false);
        }
        else{
          this._toastService.error("Sorry.. Something went wrong");
        }
        this.show = false;
      },
      (err: HttpErrorResponse) => {
        this.show = false;
        this._errorService.error(err);
      }
    )
  }

  onHold(){
    let errorInfo: Array<string> = [];
    let rollError: string;
    if (!this.selectedCarrier) {
      errorInfo.push("Carrier");
    }
    if(!this.selectedContainerType){
      errorInfo.push("Container Type")
    }
    if (!this.selectedPickDate) {
      errorInfo.push("Pickup Date");
    }
    if (!this.expBorderDate) {
      errorInfo.push("Expected Border Date");
    }
    let a =this.currentSelectionGridData.data.filter(r => !r.Group);
    if(a.length > 0){
      errorInfo.push("Container for all records");
    }

    rollError = errorInfo.join(", ");
    if (rollError) {
      this._toastService.error("Kindly enter missing fields: " + rollError);
    }
    else{
    this._confirmationService.confirm({
      key: 'message',
      value: {
        message: 'Are you sure you want to assign the Container(s) to Hold?',
        continueCallBackFunction: () => this.holdContainers()
      }
    });
    }
  }

  holdContainers(){
    var actionType : number;
    this.currentSelectionGridData.data.forEach(row => {
      row.ContainerTypeId = this.selectedContainerType.ContainerTypeId;
      row.CarrierId = this.selectedCarrier.CarrierId;
      row.ExpectedOriginDate = this.formatTheDate(this.selectedPickDate);
      row.ExpectedBorderDate = this.formatTheDate(this.expBorderDate);
      row.RequestedQty = Number(row.AvailableQty.replace(/,/g,''));
      row.BatchGroupNumber = row.Group.Id;
      if(this.selectedBuild){
        row.BuildNbr = this.selectedBuild.BuildNbr;
      }
      if(this.selectedStatus.StatusCode === 'N' && !this.selectedBuild){
        row.ActionType = 10;
      }
      if(this.selectedStatus.StatusCode === 'H' && this.selectedBuild){
        row.ActionType = 30;
      }
    });
    this._containerPoService.containerBuildHoldContainers(this.currentSelectionGridData.data).subscribe(
      data => {
      if(data){
          this.successBuild = this.selectedBuild;
          this._toastService.success("Container(s) have been assigned to Hold successfully");
          this.currentSelectionGridData.data = [];
          this.onStatusChange(new Status('H','Hold'),this.selectedStatus, true);
        }
        else{
          this._toastService.error("Sorry.. Something went wrong");
        }
        this.show = false;
      },
      (err: HttpErrorResponse) => {
        this.show = false;
        this._errorService.error(err);
      }
    )

  }

  onSelectChangeQty(poLine) {
    if (!poLine.IsSelected) {
      poLine.AvailableQty = poLine.DbQty.toLocaleString();
      poLine.ExtendedValue = Math.floor(poLine.POPrice * poLine.DbQty);
      poLine.Weight = Math.floor(poLine.WeightPerYard * poLine.DbQty);
    }
  }

  disableCheckBox(poLine){
    let disable = false;
    if(this.selectedStatus){
      if((this.selectedStatus.StatusCode === 'N' && poLine.IsInCurrentSelection === true)){
        disable = true;
      }
      if((this.selectedStatus.StatusCode === 'A') && (poLine.IsAdded || poLine.IsModified)){
        disable = true;
      }
      if(this.selectedStatus.StatusCode === 'H' && (poLine.IsInCurrentSelection)){
        disable = true;
      }
    }
  return disable;
  }

  disableQty(poLine){    
    if(this.disableCheckBox(poLine)){
      return true;
    }
    return !poLine.IsSelected;
  }

  showHoldButton(){
    if(this.selectedStatus && this.selectedStatus.StatusCode === 'A'){
      return false;
    }
    return true;
  }

  filterSortChange(sort: SortDescriptor[]): void {
    this.filterSort = sort;
    this.sortFilterDataRolls();
  }
  sortFilterDataRolls() {
    this.filterGridData = {
      data: orderBy(this.filterGridData.data, this.filterSort),
      total: this.filterGridData.data.length
    };
  }
  currentSortChange(sort: SortDescriptor[]): void {
    this.currentSort = sort;
    this.sortCurrentDataRolls();
  }
  sortCurrentDataRolls() {
    this.currentSelectionGridData = {
      data: orderBy(this.currentSelectionGridData.data, this.currentSort),
      total: this.currentSelectionGridData.data.length
    };
  }

  handleVendorFilter(value: string) {
    this.filteredVendors = this.vendors.filter((s) => (s.RMVendorName.toUpperCase()).startsWith(value.toUpperCase()));
  }

  handlePickupLocationFilter(value : string){
    this.filteredPickupLocn = this.pickupLocations.filter((s) => (s.PickupLocnName.toUpperCase()).startsWith(value.toUpperCase()));
  }

  handlePOFilter(value : string){
    this.filteredPos = this.purchaseOrders.filter((s) => (s.PONbr.toUpperCase()).startsWith(value.toUpperCase()));
  }

  handleItemFilter(value : string){
    this.filteredItems = this.items.filter((s) => (s.ItemName.toUpperCase()).startsWith(value.toUpperCase()));
  }
  handleBuildFilter(value : string){
    this.filteredBuilds = this.builds.filter((s) => (s.BuildNbr.toString().startsWith(value)));
  }
  handleContainerFilter(value : string){
    this.filteredFabricContainers = this.fabricContainers.filter((s) => (s.ContainerNbr.toUpperCase()).startsWith(value.toUpperCase()));
  }
  handleContainerTypeFilter(value : string){
    this.filteredContainerTypes = this.containerTypes.filter((s) => (s.ContainerTypeDesc.toUpperCase()).startsWith(value.toUpperCase()));
  }
  handleCarrierFilter(value : string){
    this.filteredCarriers = this.carriers.filter((s) => (s.CarrierName.toUpperCase()).startsWith(value.toUpperCase()));
  }
  disableButton(){
    let disable : boolean = true;
    if((this.currentSelectionGridData.data.length > 0) && this.currentSelectionGridData.data.filter(r => r.IsModified === true || r.IsAdded === true || this.removedHoldRecords.length > 0 || this.removedApprovedRecords.length > 0).length > 0){
      disable = false;
    }
    if((this.currentSelectionGridData.data.length > 0) && this.selectedStatus.StatusCode === 'H'){
      disable = false;
    }
    return disable;
  }

  disableSearchForm(){
    if(this.selectedStatus){
      if(this.selectedStatus.StatusCode === 'H' && !this.selectedBuild){
        return true;
      }
      if(this.selectedStatus.StatusCode === 'A' && !this.selectedContainer){
        return true;
      }
    }
    else{
      return true;
    }
  }

  validateExpBorderDate(){
    if(this.expBorderDate < this.selectedPickDate && this.selectedPickDate != null ){
      this._toastService.error("'Exp Border Date' can't be less than 'Pickup Date'")
      this.expBorderDate = null;
    }
  }
  validatePickupDate(){
    if(this.selectedPickDate > this.expBorderDate && this.expBorderDate !== null){
      this._toastService.error("'Pickup Date' can't be greater than 'Exp Border Date'")
      this.selectedPickDate = null;
    }
  }

}
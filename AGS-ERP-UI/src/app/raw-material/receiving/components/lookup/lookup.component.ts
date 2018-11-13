import { Component, OnInit } from '@angular/core';
import * as moment from 'moment';
import { Receiving, ReceivingHeader } from '../../models/receiving.model';
import { AramarkItemCode } from '../../models/aramark-item-code.model';
import { LookUpGridDetails } from '../../models/lookup-details';
import { GridDataResult, GridComponent } from '@progress/kendo-angular-grid';
import { SortDescriptor } from '@progress/kendo-data-query/dist/es/sort-descriptor';
import { orderBy } from '@progress/kendo-data-query/dist/es/array.operators';
import { Receipt } from '../../models/receipt.model';
import { PickUpNumber } from '../../models/pickup-number.model';
import { ReceivingService } from '../../receiving.service';
import { HttpErrorResponse } from '@angular/common/http';
import { ErrorService } from '../../../../shared/services/error.service';
import { PickupDetails } from '../../models/pickup-details.model';
import { LookHeaderDetails } from '../../models/look-header-details';
import { CommonService } from '../../../../shared/services/common.service';
import { SortPipe } from '../../../../shared/pipes/sort.pipe';
import { SessionStorageService } from "../../../../shared/wrappers/session-storage.service";
import { ToastService } from "../../../../shared/services/toast.service";

@Component({
  selector: 'app-lookup',
  templateUrl: './lookup.component.html',
  styleUrls: ['./lookup.component.css'],
  providers: [ReceivingService]
})
export class LookupComponent implements OnInit {
  lookUpHeaderList: Array<LookHeaderDetails>;
  containers: Array<LookHeaderDetails>;
  containerList: Array<LookHeaderDetails>;
  selectedContainer: LookHeaderDetails;
  pickupNumbers: Array<LookHeaderDetails>;
  pickupNumberList: Array<LookHeaderDetails>;
  selectedpickUp: LookHeaderDetails;
  receipts: Array<LookHeaderDetails>;
  receiptsList: Array<LookHeaderDetails>;
  selectedReceipt: LookHeaderDetails;
  poNumbers: Array<LookHeaderDetails>;
  poNumberList: Array<LookHeaderDetails>;
  selectedPONumber: LookHeaderDetails;
  vendorNames: Array<LookHeaderDetails>;
  vendorsList: Array<LookHeaderDetails>;
  selectedVendorName: LookHeaderDetails;
  pickupLocations: Array<LookHeaderDetails>;
  pickupLocationList: Array<LookHeaderDetails>;
  selectedPickUpLocation: LookHeaderDetails;
  gridData: GridDataResult = { data: [], total: 0 };
  sort: SortDescriptor[] = [];
  lookUpGrid: Array<LookUpGridDetails>;
  lookUpGridData: Array<LookUpGridDetails>;
  receiptToDate: Date;
  receiptFromDate: Date;
  yards: number;
  receivedYards: number;
  show: boolean;

  constructor(
    private _recevingService: ReceivingService,
    private _errorService: ErrorService,
    private _commonService: CommonService,
    private _sortPipe: SortPipe,
    private _toastService: ToastService,
    private _sessionStorage : SessionStorageService
  ) { }

  ngOnInit() {
    if(this._sessionStorage.get('lookup_load')){
      let headerDetail = this._sessionStorage.get('lookup_header_data');
      this.containerList = this.containers = headerDetail.containerList;
      this.selectedContainer = headerDetail.selectedContainer;
      this.receiptsList = this.receipts = headerDetail.receiptsList;
      this.selectedReceipt = headerDetail.selectedReceipt;
      this.poNumberList = this.poNumbers = headerDetail.poNumberList;
      this.selectedPONumber = headerDetail.selectedPONumber;
      this.pickupNumberList = this.pickupNumbers = headerDetail.pickupNumberList;
      this.selectedpickUp = headerDetail.selectedpickUp;
      if(headerDetail.receiptFromDate){
        this.receiptFromDate = new Date(headerDetail.receiptFromDate);
      }
      if(headerDetail.receiptFromDate){
         this.receiptToDate = new Date(headerDetail.receiptToDate);
      } 
      this.vendorsList = this.vendorNames = headerDetail.vendorsList;
      this.selectedVendorName = headerDetail.selectedVendorName; 
      this.pickupLocationList = this.pickupLocations = headerDetail.pickupLocationList;
      this.selectedPickUpLocation = headerDetail.selectedPickUpLocation;
      this.lookUpHeaderList = headerDetail.lookUpHeaderList;
      this.lookUpGridData = this.lookUpGrid = this._sessionStorage.get('lookup_grid_data');
      this.loadGrid();
      this.yards = headerDetail.yards;
      this.receivedYards = headerDetail.receivedYards;
      this._sessionStorage.removeAll(['lookup_load','lookup_header_data']);
    }
    else{
      this.show = false;
      this.receiptToDate = this.receiptFromDate = null;
      this.lookUpGrid = [];
      this.yards = this.receivedYards = 0;
      this.getLookupHeaderDetailsList();
    }
  }


  onchangetoDate(receiptFromDate,receiptToDate){
    if(receiptFromDate > receiptToDate){
       this._toastService.error(
        "Receipt To Date can not be less than From Date"
      );
      this.receiptToDate = null;
    }
  }
  onchangefromDate(receiptFromDate,receiptToDate){
    if(receiptToDate == null){
    }
    else{
    if(receiptFromDate > receiptToDate){
       this._toastService.error(
        "Receipt To Date can not be less than From Date"
      );
      this.receiptToDate = null;
      }
    }
  }
  sortChange(sort: SortDescriptor[]): void {
    this.sort = sort;
    this.loadGrid();
  }
  loadGrid() {
    this.gridData = {
      data: orderBy(this.lookUpGrid, this.sort),
      total: this.lookUpGrid.length
    };
  }

  getLookupHeaderDetailsList() {
    this.show = true;
    this._recevingService.GetLookUpContainers().subscribe(
      data => {
        this.lookUpHeaderList = this.containers = this.poNumbers = this.pickupNumbers = this.receipts =
          this.pickupLocationList = this.containerList = this.pickupNumberList = this.receiptsList = this.pickupLocationList =
          this.vendorNames = this.vendorsList = this.poNumberList = this.vendorsList = this.pickupLocations = data;
        this.show = false;
      },
      (err: HttpErrorResponse) => {
        this.show = false;
        this._errorService.error(err);
      }
    );
  }

  onContainerChange() {
    this.show = true;
    if (this.selectedContainer) {
      const containerId = this.selectedContainer.ContainerId;
      this.pickupNumbers = this.lookUpHeaderList.filter(x => x.ContainerId === containerId);
      this.receipts = this.lookUpHeaderList.filter(x => x.ContainerId === containerId);
      this.poNumbers = this.lookUpHeaderList.filter(x => x.ContainerId === containerId);
      this.vendorNames = this.lookUpHeaderList.filter(x => x.ContainerId === containerId);
      this.pickupLocations = this.lookUpHeaderList.filter(x => x.ContainerId === containerId);

      // region filtering
      this.pickupNumbers = this.pickupLocationList = this.filterDuplicates(this.pickupNumbers, 'PickupNumber');
      this.receipts = this.receiptsList = this.filterDuplicates(this.receipts, 'ReceiptNumber');
      this.poNumbers = this.poNumberList = this.filterDuplicates(this.poNumbers, 'PONumber');
      this.vendorNames = this.vendorsList = this.filterDuplicates(this.vendorNames, 'VendorName');
      this.pickupLocations = this.pickupLocationList = this.filterDuplicates(this.pickupLocations, 'PickupLocation');
      // endregion

      // region setting default
      if (this.pickupNumbers.length === 1) {
        this.selectedpickUp = this.pickupNumbers[0];
      }
      if (this.receipts.length === 1) {
        this.selectedReceipt = this.receipts[0];
      }
      if (this.poNumbers.length === 1) {
        this.selectedPONumber = this.poNumbers[0];
      }
      if (this.vendorNames.length === 1) {
        this.selectedVendorName = this.vendorNames[0];
      }
      if (this.pickupLocations.length === 1) {
        this.selectedPickUpLocation = this.pickupLocations[0];
      }
    }
    this.show = false;
  }

  onPickUpChange() {
    this.show = true;
    if (this.selectedpickUp) {
      const pickupNumber = this.selectedpickUp.PickupNumber;
      this.containers = this.lookUpHeaderList.filter(x => x.PickupNumber === pickupNumber);
      this.receipts = this.lookUpHeaderList.filter(x => x.PickupNumber === pickupNumber);
      this.poNumbers = this.lookUpHeaderList.filter(x => x.PickupNumber === pickupNumber);
      this.vendorNames = this.lookUpHeaderList.filter(x => x.PickupNumber === pickupNumber);
      this.pickupLocations = this.lookUpHeaderList.filter(x => x.PickupNumber === pickupNumber);

      // region filtering
      this.containers = this.containerList = this.filterDuplicates(this.containers, 'ContainerNbr');
      this.receipts = this.receiptsList = this.filterDuplicates(this.receipts, 'ReceiptNumber');
      this.poNumbers = this.poNumberList = this.filterDuplicates(this.poNumbers, 'PONumber');
      this.vendorNames = this.vendorsList = this.filterDuplicates(this.vendorNames, 'VendorName');
      this.pickupLocations = this.pickupLocationList = this.filterDuplicates(this.pickupLocations, 'PickupLocation');
      // endregion

      // region setting default
      if (this.containers.length === 1) {
        this.selectedContainer = this.containers[0];
      }
      if (this.receipts.length === 1) {
        this.selectedReceipt = this.receipts[0];
      }
      if (this.poNumbers.length === 1) {
        this.selectedPONumber = this.poNumbers[0];
      }
      if (this.vendorNames.length === 1) {
        this.selectedVendorName = this.vendorNames[0];
      }
      if (this.pickupLocations.length === 1) {
        this.selectedPickUpLocation = this.pickupLocations[0];
      }
      // endregion

    }
    this.show = false;
  }

  onReceiptChange() {
    this.show = true;
    if (this.selectedReceipt) {
      const receiptId = this.selectedReceipt.ReceiptId;
      this.containers = this.lookUpHeaderList.filter(x => x.ReceiptId === receiptId);
      this.pickupNumbers = this.lookUpHeaderList.filter(x => x.ReceiptId === receiptId);
      this.poNumbers = this.lookUpHeaderList.filter(x => x.ReceiptId === receiptId);
      this.vendorNames = this.lookUpHeaderList.filter(x => x.ReceiptId === receiptId);
      this.pickupLocations = this.lookUpHeaderList.filter(x => x.ReceiptId === receiptId);

      // region filtering
      this.containers = this.containerList = this.filterDuplicates(this.containers, 'ContainerNbr');
      this.pickupNumbers = this.pickupNumberList = this.filterDuplicates(this.pickupNumbers, 'PickupNumber');
      this.poNumbers = this.poNumberList = this.filterDuplicates(this.poNumbers, 'PONumber');
      this.vendorNames = this.vendorsList = this.filterDuplicates(this.vendorNames, 'VendorName');
      this.pickupLocations = this.pickupLocationList = this.filterDuplicates(this.pickupLocations, 'PickupLocation');
      // endregion

      // region setting default
      if (this.containers.length === 1) {
        this.selectedContainer = this.containers[0];
      }
      if (this.pickupNumbers.length === 1) {
        this.selectedpickUp = this.pickupNumbers[0];
      }
      if (this.poNumbers.length === 1) {
        this.selectedPONumber = this.poNumbers[0];
      }
      if (this.vendorNames.length === 1) {
        this.selectedVendorName = this.vendorNames[0];
      }
      if (this.pickupLocations.length === 1) {
        this.selectedPickUpLocation = this.pickupLocations[0];
      }
      // endregion
    }
    this.show = false;
  }

  onPOChange() {
    this.show = true;
    if (this.selectedPONumber) {
      const poNumber = this.selectedPONumber.PONumber;
      this.containers = this.lookUpHeaderList.filter(x => x.PONumber === poNumber);
      this.pickupNumbers = this.lookUpHeaderList.filter(x => x.PONumber === poNumber);
      this.receipts = this.lookUpHeaderList.filter(x => x.PONumber === poNumber);
      this.vendorNames = this.lookUpHeaderList.filter(x => x.PONumber === poNumber);
      this.pickupLocations = this.lookUpHeaderList.filter(x => x.PONumber === poNumber);

      // region filtering
      this.containers = this.containerList = this.filterDuplicates(this.containers, 'ContainerNbr');
      this.receipts = this.receiptsList = this.filterDuplicates(this.receipts, 'ReceiptNumber');
      this.pickupNumbers = this.pickupLocationList = this.filterDuplicates(this.pickupNumbers, 'PickupNumber');
      this.vendorNames = this.vendorsList = this.filterDuplicates(this.vendorNames, 'VendorName');
      this.pickupLocations = this.pickupLocationList = this.filterDuplicates(this.pickupLocations, 'PickupLocation');
      // endregion

      // region setting default
      if (this.containers.length === 1) {
        this.selectedContainer = this.containers[0];
      }
      if (this.receipts.length === 1) {
        this.selectedReceipt = this.receipts[0];
      }
      if (this.pickupNumbers.length === 1) {
        this.selectedpickUp = this.pickupNumbers[0];
      }
      if (this.vendorNames.length === 1) {
        this.selectedVendorName = this.vendorNames[0];
      }
      if (this.pickupLocations.length === 1) {
        this.selectedPickUpLocation = this.pickupLocations[0];
      }
      // endregion
    }
    this.show = false;
  }

  onVendorChange() {
    this.show = true;
    if (this.selectedVendorName) {
      const poNumber = this.selectedVendorName.PONumber;
      this.containers = this.lookUpHeaderList.filter(x => x.PONumber === poNumber);
      this.receipts = this.lookUpHeaderList.filter(x => x.PONumber === poNumber);
      this.pickupNumbers = this.lookUpHeaderList.filter(x => x.PONumber === poNumber);
      this.poNumbers = this.lookUpHeaderList.filter(x => x.PONumber === poNumber);
      this.pickupLocations = this.lookUpHeaderList.filter(x => x.PONumber === poNumber);

      // region filtering
      this.containers = this.containerList = this.filterDuplicates(this.containers, 'ContainerNbr');
      this.receipts = this.receiptsList = this.filterDuplicates(this.receipts, 'ReceiptNumber');
      this.pickupNumbers = this.pickupLocationList = this.filterDuplicates(this.pickupNumbers, 'PickupNumber');
      this.poNumbers = this.poNumberList = this.filterDuplicates(this.poNumbers, 'PONumber');
      this.pickupLocations = this.pickupLocationList = this.filterDuplicates(this.pickupLocations, 'PickupLocation');
      // endregion

      // region setting default
      if (this.containers.length === 1) {
        this.selectedContainer = this.containers[0];
      }
      if (this.receipts.length === 1) {
        this.selectedReceipt = this.receipts[0];
      }
      if (this.pickupNumbers.length === 1) {
        this.selectedpickUp = this.pickupNumbers[0];
      }
      if (this.poNumbers.length === 1) {
        this.selectedPONumber = this.poNumbers[0];
      }
      if (this.pickupLocations.length === 1) {
        this.selectedPickUpLocation = this.pickupLocations[0];
      }
      // endregion
    }
    this.show = false;
  }

  onPickupLocationChange() {
    this.show = true;
    if (this.selectedPickUpLocation) {
      const pickupLocation = this.selectedPickUpLocation.PickupLocation;
      this.containers = this.lookUpHeaderList.filter(x => x.PickupLocation === pickupLocation);
      this.receipts = this.lookUpHeaderList.filter(x => x.PickupLocation === pickupLocation);
      this.pickupNumbers = this.lookUpHeaderList.filter(x => x.PickupLocation === pickupLocation);
      this.poNumbers = this.lookUpHeaderList.filter(x => x.PickupLocation === pickupLocation);
      this.vendorNames = this.lookUpHeaderList.filter(x => x.PickupLocation === pickupLocation);

      // region filtering
      this.containers = this.containerList = this.filterDuplicates(this.containers, 'ContainerNbr');
      this.receipts = this.receiptsList = this.filterDuplicates(this.receipts, 'ReceiptNumber');
      this.pickupNumbers = this.pickupLocationList = this.filterDuplicates(this.pickupNumbers, 'PickupNumber');
      this.poNumbers = this.poNumberList = this.filterDuplicates(this.poNumbers, 'PONumber');
      this.vendorNames = this.vendorsList = this.filterDuplicates(this.vendorNames, 'VendorName');
      // endregion

      // region setting default
      if (this.containers.length === 1) {
        this.selectedContainer = this.containers[0];
      }
      if (this.receipts.length === 1) {
        this.selectedReceipt = this.receipts[0];
      }
      if (this.pickupNumbers.length === 1) {
        this.selectedpickUp = this.pickupNumbers[0];
      }
      if (this.poNumbers.length === 1) {
        this.selectedPONumber = this.poNumbers[0];
      }
      if (this.vendorNames.length === 1) {
        this.selectedVendorName = this.vendorNames[0];
      }
      // endregion
    }
    this.show = false;
  }
  getDate(date): string {
    return moment(date).format('MM/DD/YYYY');
  }

  handleFilter(value) {
    this.containers = this.containerList.filter((s) => (s.ContainerNbr.toString().toUpperCase()).startsWith(value.toString().toUpperCase()));
  }
  handlePickUpNbrFilter(value) {
    this.pickupNumbers = this.pickupNumberList.filter((s) => (s.PickupNumber.toUpperCase()).startsWith(value.toUpperCase()));
  }
  handleReceiptFilter(value) {
    this.receipts =
      this.receiptsList.filter(x => (x.ReceiptNumber.toString().toUpperCase()).startsWith(value.toString().toUpperCase()));
  }
  handlePOFilter(value) {
    this.poNumbers = this.poNumberList.filter(x => x.PONumber.toString().toUpperCase().startsWith(value.toString().toUpperCase()));
  }
  handleVendorNameFilter(value) {
    this.vendorNames = this.vendorsList.filter(x => x.VendorName.toString().toUpperCase().startsWith(value.toString().toUpperCase()));
  }
  handlePickupLocationFilter(value) {
    this.pickupLocations = this.pickupLocationList
      .filter(x => x.PickupLocation.toString().toUpperCase().startsWith(value.toString().toUpperCase()));
  }
  ExportToPdf(grid: GridComponent) {
    grid.saveAsPDF();
  }

  ExportToExcel(grid: GridComponent) {
    grid.saveAsExcel();
  }

  ClearHeader() {
    this.receiptToDate = this.receiptFromDate = this.selectedpickUp = this.selectedPONumber = null;
    this.selectedContainer = this.selectedPickUpLocation = this.selectedReceipt = this.selectedVendorName = null;
    this.pickupLocations = this.pickupLocationList = this.pickupNumbers = this.pickupNumberList = this.vendorNames = this.containers =
      this.receipts = this.receiptsList = this.poNumbers = this.poNumberList = this.vendorsList = this.containerList
      = this.lookUpHeaderList;
    this.receiptFromDate = this.receiptToDate = null;
  }

  Search() {
    this.show = true;
    this.yards = this.receivedYards = 0;
    const toDate = this.receiptToDate == null ? null :
      this.getDate(new Date(this.receiptToDate.getFullYear(), this.receiptToDate.getMonth(), this.receiptToDate.getDate()));
    const fromDate = this.receiptFromDate == null ? null :
      this.getDate(new Date(this.receiptFromDate.getFullYear(), this.receiptFromDate.getMonth(), this.receiptFromDate.getDate()));
    const pickNumber = (this.selectedpickUp === undefined || this.selectedpickUp === null) ? '' : this.selectedpickUp.PickupNumber;
    const containerId = (this.selectedContainer === undefined || this.selectedContainer === null) ? 0 : this.selectedContainer.ContainerId;
    const vendorId = (this.selectedVendorName === undefined || this.selectedVendorName === null) ? 0 : this.selectedVendorName.VendorId;
    const pickupLocation = (this.selectedPickUpLocation === undefined || this.selectedPickUpLocation === null) ?
      '' : this.selectedPickUpLocation.PickupLocation;
    const PONumber = (this.selectedPONumber === undefined || this.selectedPONumber === null) ? '' : this.selectedPONumber.PONumber;
    const receiptNumber = (this.selectedReceipt === undefined || this.selectedReceipt === null) ? '' : this.selectedReceipt.ReceiptNumber;

    const searchData = new LookHeaderDetails(0, '', containerId, pickNumber, 0, vendorId, '', '', pickupLocation, 0, PONumber,
      receiptNumber, new Date(), fromDate, toDate, '', '', '', 0, 0, 0, 0, '', '', null);

    this._recevingService.GetLookupGridDetails(searchData).subscribe(
      data => {
        this.lookUpGridData = this.lookUpGrid = data;
        this.lookUpGrid.forEach(x => {
          x.ReceivedDate = this.getDate(x.ReceivedDate);
          this.yards += Number(x.Quantity);
          this.receivedYards += Number(x.ReceivedYards);
        });
        this.loadGrid();
        this.show = false;
      },
      (err: HttpErrorResponse) => {
        this.show = false;
        this._errorService.error(err);
      }
    );
  }

  EnableSearch(): boolean {
    if (this.selectedContainer !== undefined || this.selectedContainer === null) {
      return true;
    }
    return false;
  }
  post(receiptNumber, poNumber) {
    this.show = true;
    this._commonService.Notify({
      key: 'PlanningReport',
      value: { 'ReceiptNumber': receiptNumber, 'PONumber': poNumber }
    });
    setTimeout(() => {
      this._commonService.Notify({
        key: 'PlanningReport',
        value: { 'ReceiptNumber': receiptNumber, 'PONumber': poNumber }
      });
      this.show = false;
    }, 1);
    let a = new LookupHeader();
    a.containerList = this.containerList;
    a.selectedContainer = this.selectedContainer;
    a.receiptsList = this.receiptsList;
    a.selectedReceipt = this.selectedReceipt;
    a.pickupNumberList = this.pickupNumberList;
    a.selectedpickUp = this.selectedpickUp;
    a.poNumberList = this.poNumberList;
    a.selectedPONumber = this.selectedPONumber;
    a.receiptFromDate = this.receiptFromDate;
    a.receiptToDate = this.receiptToDate;
    a.vendorsList = this.vendorsList;
    a.selectedVendorName = this.selectedVendorName;
    a.pickupLocationList = this.pickupLocationList;
    a.selectedPickUpLocation = this.selectedPickUpLocation;
    a.lookUpHeaderList = this.lookUpHeaderList;
    a.yards = this.yards;
    a.receivedYards = this.receivedYards;
    this._sessionStorage.add('lookup_load',true);
    this._sessionStorage.add('lookup_header_data',a);
    this._sessionStorage.add('lookup_grid_data',this.gridData.data)
  }

  filterDuplicates(list, value) {
    return this._sortPipe.removeDuplicates(list, value);
  }
  canDisableSearchAndClear() {
    return (!this.selectedContainer && (!this.selectedpickUp) && (!this.selectedPickUpLocation) &&
      (!this.selectedPONumber) && (!this.selectedReceipt) && (!this.selectedVendorName) && ((!this.receiptToDate)
        || (!this.receiptFromDate)));
  }
}

export class LookupHeader{
  public containers: Array<LookHeaderDetails>;
  public containerList: Array<LookHeaderDetails>;
  public selectedContainer: LookHeaderDetails;
  public pickupNumbers: Array<LookHeaderDetails>;
  public pickupNumberList: Array<LookHeaderDetails>;
  public selectedpickUp: LookHeaderDetails;
  public receipts: Array<LookHeaderDetails>;
  public receiptsList: Array<LookHeaderDetails>;
  public selectedReceipt: LookHeaderDetails;
  public poNumbers: Array<LookHeaderDetails>;
  public poNumberList: Array<LookHeaderDetails>;
  public selectedPONumber: LookHeaderDetails;
  public vendorNames: Array<LookHeaderDetails>;
  public vendorsList: Array<LookHeaderDetails>;
  public selectedVendorName: LookHeaderDetails;
  public pickupLocations: Array<LookHeaderDetails>;
  public pickupLocationList: Array<LookHeaderDetails>;
  public selectedPickUpLocation: LookHeaderDetails;
  public receiptToDate: Date;
  public receiptFromDate: Date;
  public lookUpHeaderList:  Array<LookHeaderDetails>;
  public yards: number;
  public receivedYards: number;
}

import { Component, OnInit } from '@angular/core';
import * as moment from 'moment';
import { BinLocationsModel } from '../../../inventory/physical-adjustments/models/bin-locations.model';
import { Receiving, ReceivingHeader } from '../../models/receiving.model';
import { FabricContainer } from '../../models/fabric-container.model';
import { PurchaseOrder } from '../../models/purchase-order.model';
import { AramarkItemCode } from '../../models/aramark-item-code.model';
import { ContainerModel } from '../../models/container.model';
import { POLineModel } from '../../models/po-line.model';
import { PickupModel } from '../../models/pick-up.model';
import { POModel } from '../../models/po.model';
import { AramarkItemModel } from '../../models/aramark-item.model';
import { ConfirmationService } from '../../../../shared/services/confirmation.service';
import { AlertService } from '../../../../shared/services/alert.service';
import { GridComponent, GridDataResult } from '@progress/kendo-angular-grid';
import { PickupDetails } from '../../models/pickup-details.model';
import { ReceivingService } from '../../receiving.service';
import { ErrorService } from '../../../../shared/services/error.service';
import { PODetailModel } from '../../models/po-detail.model';
import { PhysicalAdjustmentsService } from '../../../inventory/physical-adjustments/physical-adjustments.service';
import { PONumber } from '../../../purchasing/models/poNumber';
import { AramarkItem } from '../../../fabric-planning/models/aramark-item';
import { BinLocation } from '../../models/bin-location.model';
import { SortDescriptor } from '@progress/kendo-data-query/dist/es/sort-descriptor';
import { orderBy } from '@progress/kendo-data-query/dist/es/array.operators';
import { ToastService } from '../../../../shared/services/toast.service';
import { HttpErrorResponse } from '@angular/common/http';
import { SessionStorageService } from "../../../../shared/wrappers/session-storage.service";
@Component({
  selector: 'app-receiving-old',
  templateUrl: './receipts.component.html',
  styleUrls: ['./receipts.component.css'],
  providers: [ReceivingService, PhysicalAdjustmentsService]
})
export class ReceiptsComponent implements OnInit {

  ReceiptHeader: PickupDetails;
  PickupDetailsList: PickupDetails[];
  PODetailsList: PODetailModel[];
  ReceiptsGridData: POLineModel[];
  BinLocationList: BinLocationsModel[];
  TotalRolls: number;
  TotalYards: number;
  gridView: GridDataResult = { data: [], total: 0 };
  show: boolean = false;
  public sort: SortDescriptor[] = [];
  public dropdownlist: any;

  constructor(
    private _confirmationService: ConfirmationService,
    private _alertService: AlertService,
    private _receivingService: ReceivingService,
    private _errorService: ErrorService,
    private _physicalAdjustmentsService: PhysicalAdjustmentsService,
    private _toastService: ToastService,
    private _sessionStorage : SessionStorageService
  ) { }

  ngOnInit() {
    this.ReceiptHeader = new PickupDetails();
    this.getContainers();
    this.ReceiptsGridData = [];
    this.gridView = { data: [], total: 0 };
    this._sessionStorage.remove('lookup_load');
    this._sessionStorage.remove('lookup_header_data');
    this._sessionStorage.remove('lookup_grid_data');
  }

  getContainers() {
    this.show = true;
    this._receivingService.GetReceiptsContainers().subscribe(
      data => {
        this.getPickups();
        this.ReceiptHeader.ContainerList = this.ReceiptHeader.Containers = data;
        this.show = false;
      },
      err => {
        this.show = false;
        this._errorService.error(err);
      }
    );
  }

  getPickups() {
    this.show = true;
    this._receivingService.GetReceiptsPickups().subscribe(
      data => {
        this.PickupDetailsList = data;
        let pickups: PickupModel[] = [];
        this.PickupDetailsList.forEach(c => {
          pickups.push(new PickupModel(c.PickupNumber, c.ContainerId, c.VendorName, c.Location));
        })
        this.ReceiptHeader.PickupList = this.ReceiptHeader.Pickups = pickups;
        
        if(this.ReceiptHeader.ContainerList && this.ReceiptHeader.ContainerList.length === 1){
          this.ReceiptHeader.Container = this.ReceiptHeader.ContainerList[0];
          this.onContainerChange(this.ReceiptHeader.Container);
        }
        this.getBins(2);
        this.show = false;
      },
      err => {
        this.show = false;
        this._errorService.error(err);
      }
    );
  }

  confirmContainerChange(currentContainer: ContainerModel, prevContainer: ContainerModel) {
    if (this.gridView.data.length > 0) {
      this._confirmationService.confirm({
        key: 'message',
        value: {
          message: 'This Page contain unsaved data. Would you like to perform navigation?<br> Press "OK" to continue or "Cancel" to abort the navigation',
          continueCallBackFunction: () => this.onContainerChange(currentContainer),
          cancelCallBackFunction: () => this.preventContainerChange(prevContainer)
        }
      });
    }
    else {
      this.onContainerChange(currentContainer);
    }
  }

  preventContainerChange(prevContainer: ContainerModel) {
    this.ReceiptHeader.Container = new ContainerModel(prevContainer.Id, prevContainer.Number, prevContainer.Vendor);
  }

  onContainerChange(selectedContainer: ContainerModel) {
    if (selectedContainer) {
      this.ReceiptHeader.Container = selectedContainer;
      this.clearHeader();
      this.show = true;
      this.ReceiptHeader.PickupList = this.ReceiptHeader.Pickups.filter(s => s.ContainerId === selectedContainer.Id);
      if (this.ReceiptHeader.PickupList.length === 1) {
        this.ReceiptHeader.Pickup = this.ReceiptHeader.PickupList[0];
        this.onPickupChange(this.ReceiptHeader.PickupList[0]);
      }
      this.show = false;
    }
    else {
      this.clearHeader();
    }
  }

  clearHeader(){
      this.ReceiptHeader.ASN = null;
      this.ReceiptHeader.BillOfLading = null;
      this.ReceiptHeader.Comments = null;
      this.ReceiptHeader.Location = null;
      this.ReceiptHeader.ReceivedRolls = null;
      this.ReceiptHeader.ReceivedYards = null;
      this.ReceiptHeader.Shipment = null;
      this.ReceiptHeader.TotalRolls = null;
      this.ReceiptHeader.TotalYards = null;
      this.ReceiptHeader.VendorCode = null;
      this.ReceiptHeader.VendorName = null;
      this.ReceiptHeader.ContainerStopId = null;
      this.ReceiptHeader.ShipmentHdrId = null;
      this.ReceiptHeader.CurrentReceivingRolls = null;
      this.ReceiptHeader.CurrentReceivingYards = null;      
      this.ReceiptHeader.Pickup = null;
      this.ReceiptHeader.PickupList = [];
      this.gridView.data = this.ReceiptsGridData = [];
  }

  confirmPickupNumberChange(currentPNo: PickupModel, prevPNo: PickupModel) {
    if (this.gridView.data.length > 0) {
      this._confirmationService.confirm({
        key: 'message',
        value: {
          message: 'This Page contain unsaved data. Would you like to perform navigation?<br> Press "OK" to continue or "Cancel" to abort the navigation',
          continueCallBackFunction: () => this.onPickupChange(currentPNo),
          cancelCallBackFunction: () => this.preventPickupNoChange(prevPNo)
        }
      });
    }
    else {
      this.onPickupChange(currentPNo);
    }
  }

  preventPickupNoChange(prevPNo: PickupModel) {
    this.ReceiptHeader.Pickup = new PickupModel(prevPNo.PickupNumber, prevPNo.ContainerId, prevPNo.Vendor, prevPNo.Location)
  }

  onPickupChange(selectedPickUp: PickupModel) {
    if (selectedPickUp) {
      this.ReceiptHeader.Pickup = selectedPickUp;
      this.show = true;
      this.gridView.data = [];
      this.PODetailsList = [];
      let POlist: POModel[] = [];
      this.ReceiptHeader.Container = this.ReceiptHeader.ContainerList.find(s => s.Id === selectedPickUp.ContainerId);
      let selectedPickup = this.PickupDetailsList.find(c => c.PickupNumber === selectedPickUp.PickupNumber);
      this.getPODetails(selectedPickup.ContainerId, selectedPickup.ContainerStopId);
      this.ReceiptHeader.ASN = selectedPickup.ASN;
      this.ReceiptHeader.BillOfLading = selectedPickup.BillOfLading;
      this.ReceiptHeader.Comments = selectedPickup.Comments;
      this.ReceiptHeader.Location = selectedPickup.Location;
      this.ReceiptHeader.ReceivedRolls = selectedPickup.ReceivedRolls;
      this.ReceiptHeader.ReceivedYards = selectedPickup.ReceivedYards;
      this.ReceiptHeader.Shipment = selectedPickup.Shipment;
      this.ReceiptHeader.TotalRolls = selectedPickup.TotalRolls;
      this.ReceiptHeader.TotalYards = selectedPickup.TotalYards;
      this.ReceiptHeader.VendorCode = selectedPickup.VendorCode;
      this.ReceiptHeader.VendorName = selectedPickup.VendorName;
      this.ReceiptHeader.ContainerStopId = selectedPickup.ContainerStopId;
      this.ReceiptHeader.ShipmentHdrId = selectedPickup.ShipmentHdrId;
      this.ReceiptHeader.CurrentReceivingRolls = null;
      this.ReceiptHeader.CurrentReceivingYards = null;
      this.ReceiptsGridData = [];
      if (this.PODetailsList !== undefined || this.PODetailsList.length === 0) {
        this.PODetailsList.forEach(p => {
          POlist.push(new POModel(p.PONumber, p.POLineId, p.POHdrId));
        })
      }
      this._receivingService.GetGridDetails(selectedPickUp.ContainerId, selectedPickUp.PickupNumber).subscribe(
        data => {
          data.forEach(d => {
            let receiptGrid = new POLineModel();
            receiptGrid.POList = receiptGrid.POs = POlist;
            if (receiptGrid.POList === undefined || receiptGrid.POList.length === 0) {
              receiptGrid.POList.push(new POModel(d.PONumber, d.POLineId, d.POHdrId));
              this.getAramarkItems(d.PONumber);
            }
            if (receiptGrid.AramarkItemList === undefined || receiptGrid.AramarkItemList.length === 0) {
              receiptGrid.AramarkItemList.push(new AramarkItemModel(d.AramarkItemId, d.AramarkItem, d.Color, d.Width, d.FiberContent, d.VendorItem, d.COO));
            }
            receiptGrid.BinLocations = receiptGrid.BinLocationList = this.BinLocationList;
            receiptGrid.AramarkItem = receiptGrid.AramarkItemList.find(a => a.AramarkItemCode === d.AramarkItem);
            receiptGrid.BinLocation = receiptGrid.BinLocationList === undefined ? null : receiptGrid.BinLocationList.find(b => b.AreaCode === d.BinLocation);
            receiptGrid.PO = POlist.find(s => s.POLineId === d.POLineId);
            receiptGrid.RollYards = d.Quantity;
            receiptGrid.LotNumber = d.LotNumber;
            receiptGrid.Color = d.Color;
            receiptGrid.Width = d.Width;
            receiptGrid.FiberContent = d.FiberContent;
            receiptGrid.VendorItem = d.VendorItem;
            receiptGrid.COO = d.COO;
            receiptGrid.Defective = d.Defective;
            receiptGrid.POComments = d.POComments;
            receiptGrid.RollCase = d.RollCaseNbr;
            receiptGrid.POList = POlist;
            receiptGrid.RollCaseId = d.RollCaseId;
            this.ReceiptHeader.ReceivedRolls = d.ReceivedRolls;
            this.ReceiptHeader.ReceivedYards = d.ReceivedYards;
            this.ReceiptHeader.CurrentReceivingRolls = d.CurrentReceivingRolls;
            this.ReceiptHeader.CurrentReceivingYards = d.CurrentReceivingYards;
            this.ReceiptsGridData.push(receiptGrid);
          })
          this.gridView.data = this.ReceiptsGridData;
          this.show = false;
        },
        err => {
          this.show = false;
          this._errorService.error(err);
        }
      )
    }
    else {
      this.ReceiptHeader.ASN = null;
      this.ReceiptHeader.BillOfLading = null;
      this.ReceiptHeader.Comments = null;
      this.ReceiptHeader.Location = null;
      this.ReceiptHeader.ReceivedRolls = null;
      this.ReceiptHeader.ReceivedYards = null;
      this.ReceiptHeader.Shipment = null;
      this.ReceiptHeader.TotalRolls = null;
      this.ReceiptHeader.TotalYards = null;
      this.ReceiptHeader.VendorCode = null;
      this.ReceiptHeader.VendorName = null;
      this.ReceiptHeader.ContainerStopId = null;
      this.ReceiptHeader.ShipmentHdrId = null;
      this.ReceiptHeader.CurrentReceivingRolls = null;
      this.ReceiptHeader.CurrentReceivingYards = null;      
      this.ReceiptHeader.Pickup = null;
      this.ReceiptHeader.PickupList = this.ReceiptHeader.Pickups.filter(s => s.ContainerId === this.ReceiptHeader.Container.Id);
      this.gridView.data = this.ReceiptsGridData = [];
    }    
  }

  getPODetails(containerId: number, containerStopId: number) {
    this._receivingService.getReceiptsPODetails(containerId, containerStopId).subscribe(
      data => {
        this.PODetailsList = data;
      },
      err => {
        this._errorService.error(err);
      }
    );
  }

  getPOs(): POModel[] {
    let pos: POModel[] = [];
    this.PODetailsList.forEach(c => {
      pos.push(new POModel(c.PONumber, c.POLineId, c.POHdrId));
    })
    return pos;
  }

  onPOChange(PO: POModel, POLine: POLineModel) {
    if (PO !== undefined) {
      POLine.AramarkItemList = POLine.AramarkItems = this.getAramarkItems(PO.PONumber);
      if (POLine.AramarkItemList.length === 1) {
        POLine.AramarkItem = POLine.AramarkItemList[0];
        POLine.Color = POLine.AramarkItemList[0].Color;
        POLine.Width = POLine.AramarkItemList[0].Width;
        POLine.FiberContent = POLine.AramarkItemList[0].FiberContent;
        POLine.VendorItem = POLine.AramarkItemList[0].VendorItem;
        POLine.COO = POLine.AramarkItemList[0].COO;
      }
    }
  }

  getAramarkItems(poNumber: string): AramarkItemModel[] {
    let aramarkItems: AramarkItemModel[] = [];
    this.PODetailsList.filter(c => c.PONumber === poNumber).forEach(c => {
      aramarkItems.push(new AramarkItemModel(c.AramarkItemId, c.AramarkItemCode, c.Color, c.Width, c.FiberContent, c.VendorItem, c.COO));
    });
    return aramarkItems;
  }

  onAramarkItemChange(aramarkItem: AramarkItemModel, POLine: POLineModel) {
    if (aramarkItem !== undefined) {
      POLine.Color = aramarkItem.Color;
      POLine.Width = aramarkItem.Width;
      POLine.FiberContent = aramarkItem.FiberContent;
      POLine.VendorItem = aramarkItem.VendorItem;
      POLine.COO = aramarkItem.COO;
    }
  }

  getPOLines() {
    let poLine = new POLineModel();
    return [poLine, poLine];
  }

  getBins(locationId: number) {
    this._physicalAdjustmentsService.getBinLocation(locationId).subscribe(
      data => {
        this.BinLocationList = data;
      },
      err => {
        this._errorService.error(err);
      }
    );
  }

  onAdd() {
    let poLine = new POLineModel();
    let containerId = this.ReceiptHeader.Container.Id;
    let containerStopId = this.ReceiptHeader.ContainerStopId;
    poLine.POList = poLine.POs = this.PODetailsList;
    if (poLine.POList.length === 1) {
      poLine.PO = poLine.POList[0];
      this.onPOChange(poLine.PO, poLine);
    }
    poLine.BinLocations = poLine.BinLocationList = this.BinLocationList;
    poLine.Defective = 0;
    poLine.IsAdded = true;
    this.ReceiptsGridData.push(poLine);
    this.gridView.data = this.ReceiptsGridData;
    this.ReceiptHeader.CurrentReceivingRolls = this.ReceiptsGridData.length;
  }

  onCopy() {
    if (this.ReceiptsGridData.filter(c => c.IsSelected === true).length === 0) {
      this._toastService.warn('Please select a roll to copy.');
    }
    if (this.ReceiptsGridData.filter(c => c.IsSelected === true).length > 1) {
      this._toastService.warn('Please select only one roll to copy.');
    }
    else {
      let selectedPoLine = this.ReceiptsGridData.find(c => c.IsSelected === true);
      let poLine = new POLineModel();
      poLine.AramarkItem = selectedPoLine.AramarkItem;
      poLine.AramarkItemList = selectedPoLine.AramarkItemList;
      poLine.BinLocationList = selectedPoLine.BinLocationList;
      poLine.Color = selectedPoLine.Color;
      poLine.Comments = "";
      poLine.COO = selectedPoLine.COO;
      poLine.Defective = 0;
      poLine.FiberContent = selectedPoLine.FiberContent;
      poLine.IsSelected = true;
      poLine.LotNumber = selectedPoLine.LotNumber;
      poLine.PO = selectedPoLine.PO;
      poLine.POList = selectedPoLine.POList;
      poLine.RollYards = selectedPoLine.RollYards;
      poLine.VendorItem = selectedPoLine.VendorItem;
      poLine.Width = selectedPoLine.Width;
      this.ReceiptsGridData.push(poLine);
      selectedPoLine.IsSelected = false;
      this.gridView.data = this.ReceiptsGridData;
      this.ReceiptHeader.CurrentReceivingRolls = this.ReceiptsGridData.length;
    }
  }

  onDeleteConfirm() {
    if (this.ReceiptsGridData.filter(c => c.IsSelected === true).length === 0) {
      this._toastService.warn('Please select a roll to delete.');
      return;
    }
    this._confirmationService.confirm({
      key: 'message',
      value: {
        message: 'Are you sure! do you want to delete selected rolls?',
        continueCallBackFunction: () => this.onDelete()
      }
    })
  }

  onDelete() {
    let selectedPOLines = this.ReceiptsGridData.filter(c => c.IsSelected === true);
    selectedPOLines.forEach(c => {
      if (c.BinLocation) {
        c.InventoryAreaId = c.BinLocation.Id;
      }
      c.Quantity = c.RollYards,
        c.POComments = c.Comments,
        c.RollCaseNbr = c.RollCase,
        c.ShipmentHdrId = this.ReceiptHeader.ShipmentHdrId,
        c.PONumber = c.PO.PONumber,
        c.POLineId = c.PO.POLineId,
        c.POHdrId = c.PO.POHdrId,
        c.AramarkItemId = c.AramarkItem.Id,
        c.ReceiptDate = c.ReceiptDate
    });
    selectedPOLines.forEach(c => {
      this.ReceiptsGridData.splice(this.ReceiptsGridData.findIndex(c => c.IsSelected === true), 1);
    });
    this.gridView.data = this.ReceiptsGridData;
    this.ReceiptHeader.CurrentReceivingRolls = this.ReceiptsGridData.length;
  }

  onDefectiveChange(poLine: POLineModel) {
    if (poLine.Defective >= Number(poLine.RollYards)) {
      this._toastService.warn('Defective yards should be less than or equal to roll yards.');
      poLine.Defective = 0;
    }
  }

  onSaveConfirm() {
    this.show = true;
    let rollError = this.FieldValidationForPostAndSave("Save");
    if(rollError){
      this._toastService.error("Kindly enter missing fields: " + rollError);
    }
    else{
    if(Number(this.ReceiptHeader.CurrentReceivingYards) !== this.getTotalYards())
      {
        this._toastService.error("Total Yards(Sum of all Roll Yards in grid) & Cur. Receiving Yards should be equal");
    }
    else{
    let currentYards: number = this.getTotalYards();
    if (currentYards > this.ReceiptHeader.TotalYards - this.ReceiptHeader.ReceivedYards) {
      this.show = false;
      this._alertService.alert({
        key: 'alertMessage',
        value: 'Total of already received yards and receiving yards should be less than or equal to total yards.'
      });
    }
    else {
      if (currentYards !== this.ReceiptHeader.CurrentReceivingYards) {
        this.ReceiptHeader.CurrentReceivingYards = currentYards;
      }
      this.ReceiptsGridData.forEach(v => this.validateSaveReceipts(v));
      if (this.ReceiptsGridData.some(e => e.IsValid === false)) {
        return;
      }
      if (this.ReceiptsGridData.some(r => r.RollYards <= 0)) {
        this.show = false;
        this._toastService.error("Rolls Yards should be greater than the zero.");
        return;
      }
      this.show = false;
      this.onSave();
    }}}
    this.show = false;
  }

  onSave() {
    this.show = true;
    this.ReceiptHeader.CurrentReceivingYards = this.getTotalYards();
    this.ReceiptsGridData.forEach(c => {
      if (c.BinLocation) {
        c.InventoryAreaId = c.BinLocation.Id;
      }
      c.Quantity = c.RollYards,
        c.POComments = c.POComments,
        c.RollCaseNbr = c.RollCase,
        c.ShipmentHdrId = this.ReceiptHeader.ShipmentHdrId,
        c.PONumber = c.PO.PONumber,
        c.POLineId = c.PO.POLineId,
        c.POHdrId = c.PO.POHdrId;
      if (c.AramarkItemId) {
        c.AramarkItemId = c.AramarkItem.Id;
      }
      c.RollCaseId = c.RollCaseId
    });
    this._receivingService.SaveReceiptsRollCases({ ReceiptHeader: this.ReceiptHeader, ReceiptDetails: this.ReceiptsGridData }).subscribe(
      data => {
        this.show = false;
        this.ReceiptsGridData = [];
        this.getGridDetails();
        this._toastService.success('Rolls saved successfully, with container '+this.ReceiptHeader.Container.Number+" and PickUp # " + this.ReceiptHeader.Pickup.PickupNumber);
      },
      err => {
        this.show = false;
        this._errorService.error(err);
      }
    );
  }

  FieldValidationForPostAndSave(action : string){
    let errorInfo: Array<string> = [];
    let rollError: string;
    if (!this.ReceiptHeader.Container) {
      errorInfo.push("Container");
    }
    if(!this.ReceiptHeader.Pickup){
      errorInfo.push("Pickup #")
    }
    
    if (!this.ReceiptHeader.CurrentReceivingRolls) {
      errorInfo.push("Cur. Receiving Rolls");
    }
    if (!this.ReceiptHeader.CurrentReceivingYards) {
      errorInfo.push("Cur. Receiving Yards");
    }
    let gridError = 0;
    this.ReceiptsGridData.forEach(r =>{
      if(action === "Post"){
       if(!r.PO || !r.AramarkItem || !r.RollCase || !r.RollYards || !r.LotNumber 
        || !r.BinLocation || !r.Color  || !r.FiberContent || !r.VendorItem || !r.COO)
        {
          gridError++;
        }
      }
      else if(action === "Save"){
        if(!r.PO.PONumber || !r.AramarkItem || !r.RollCase || !r.RollYards || !r.LotNumber 
        || !r.Color ||  !r.FiberContent || !r.VendorItem || !r.COO)
        {
          gridError ++;
        }
      }
    })
    if(gridError > 0 ){
        errorInfo.push("All columns in the grid except 'Bin Location, Comments' for all rows");
    }
    rollError = errorInfo.join(", ");
    return rollError;
  }

  onPostConfirm() {
    let rollError = this.FieldValidationForPostAndSave("Post");
    if (rollError) {
      this._toastService.error("Kindly enter missing fields: " + rollError);
    }
    else{
    if(Number(this.ReceiptHeader.CurrentReceivingYards) === this.getTotalYards())
      {
        let currentYards: number = this.getTotalYards();
        this.show = false;
        this.onPost();
    }
    else{
      this._toastService.warn("Total Yards(Sum of all Roll Yards in grid) & Cur. Receiving Yards should be equal");
    }
    }
  }
  onPost() {
    this.show = true;
    //post call
    this.ReceiptsGridData.forEach(c => {
      c.InventoryAreaId = c.BinLocation.Id,
        c.Quantity = c.RollYards,
        c.POComments = c.POComments,
        c.RollCaseNbr = c.RollCase,
        c.ShipmentHdrId = this.ReceiptHeader.ShipmentHdrId,
        c.PONumber = c.PO.PONumber,
        c.POLineId = c.PO.POLineId,
        c.POHdrId = c.PO.POHdrId,
        c.AramarkItemId = c.AramarkItem.Id
    });
    this._receivingService.PostReceiptsRollCases({ ReceiptHeader: this.ReceiptHeader, ReceiptDetails: this.ReceiptsGridData }).subscribe(
      data => {
        this.show = false;
        // this._alertService.alert({
        //   key: 'alertMessage',
        //   value: 'Rolls posted successfully.'
        // });     
        this._toastService.success('Rolls posted successfully with container '+this.ReceiptHeader.Container.Number+" and PickUp # " + this.ReceiptHeader.Pickup.PickupNumber);
        this.ReceiptHeader.Container = null;
        this.ReceiptHeader.PickupList = this.ReceiptHeader.Pickups;
        this.ReceiptHeader.Pickup = null;
        this.ReceiptHeader.ASN = null;
        this.ReceiptHeader.VendorCode = null;
        this.ReceiptHeader.Shipment = null;
        this.ReceiptHeader.Location = null;
        this.ReceiptHeader.VendorName = null;
        this.ReceiptHeader.BillOfLading = null;
        this.ReceiptHeader.TotalRolls = null;
        this.ReceiptHeader.TotalYards = null;
        this.ReceiptHeader.ReceivedRolls = null;
        this.ReceiptHeader.ReceivedYards = null;
        this.ReceiptHeader.CurrentReceivingRolls = null;
        this.ReceiptHeader.CurrentReceivingYards = null;
        this.ReceiptHeader.Comments = null;
        this.gridView.data = [];
        this.ngOnInit();
      },
      err => {
        this.show = false;
        this._errorService.error(err);
      }
    );
  }

  onExportPDF(grid: GridComponent) {
    grid.saveAsPDF();
  }

  onExportExcel(grid: GridComponent) {
    grid.saveAsExcel();
  }

  onSelectAllCheck(event) {
    this.ReceiptsGridData.forEach(c => c.IsSelected = event.target.checked);
  }

  isAllSelected() {
    return this.ReceiptsGridData.length > 0 && this.ReceiptsGridData.every(c => c.IsSelected === true);
  }

  onCurrentReceivingRollChange() {
    let currentReceivingRolls = this.ReceiptHeader.CurrentReceivingRolls;
    // if (this.ReceiptHeader.TotalRolls < this.ReceiptHeader.ReceivedRolls + this.ReceiptHeader.CurrentReceivingRolls) {
    //   this._toastService.warn("Cur. Receiving Rolls can't be greater than "+(this.ReceiptHeader.TotalRolls - this.ReceiptHeader.ReceivedRolls));
    //   this.ReceiptHeader.CurrentReceivingRolls = this.ReceiptsGridData.length;
    //   return;
    // }
    if (this.ReceiptsGridData.length === 0) {
      if (currentReceivingRolls > 0) {
        for (let i = 0; i < currentReceivingRolls; i++) {
          this.onAdd();
        }
      }
      return;
    }
    if (this.ReceiptsGridData.length > currentReceivingRolls) {
      this._confirmationService.confirm({
        key: 'message',
        value: {
          message: 'Are you sure! Do you want to delete exceeded rolls?',
          continueCallBackFunction: () => this.removeRequiredRolls()
        }
      })
    }
    else if (this.ReceiptsGridData.length < currentReceivingRolls) {
      this._confirmationService.confirm({
        key: 'message',
        value: {
          message: 'Are you sure! Do you want to add more rolls?',
          continueCallBackFunction: () => this.addRequiredRolls()
        }
      })
    }
  }

  addRequiredRolls() {
    let requiredRollsToAdd = this.ReceiptHeader.CurrentReceivingRolls - this.ReceiptsGridData.length;
    for (let i = 0; i < requiredRollsToAdd; i++) {
      this.onAdd();
    }
  }

  removeRequiredRolls() {
    let requiredRollsToRemove = this.ReceiptsGridData.length - this.ReceiptHeader.CurrentReceivingRolls;
    this.ReceiptsGridData.splice(this.ReceiptsGridData.length - requiredRollsToRemove);
    this.gridView.data = this.ReceiptsGridData;
  }

  onCurrentReceivingYardsChange(currRecevingYards) {
    // let receivingYards = event.target.value;
    // if (receivingYards === undefined || receivingYards === 0) {
    //   this.ReceiptsGridData.forEach(y => receivingYards += y.RollYards);
    // }
    // let oldValue = Number(this.ReceiptHeader.CurrentReceivingYards);
    // if (this.ReceiptHeader.TotalYards < this.ReceiptHeader.ReceivedYards + Number(receivingYards)) {
    //   this._toastService.warn('Total of already received yards and receiving yards should be less than or equal to total yards.');
    //   this.ReceiptHeader.CurrentReceivingYards = oldValue;
    //   return;
    // }
    // this.ReceiptHeader.CurrentReceivingYards = receivingYards;
    if(currRecevingYards > this.ReceiptHeader.TotalYards - this.ReceiptHeader.ReceivedYards){
      this._toastService.warn("Cur. Receiving Yards can't be greater than "+(this.ReceiptHeader.TotalYards - this.ReceiptHeader.ReceivedYards));
      this.ReceiptHeader.CurrentReceivingYards = 0;
    }
   
  }

  handleContainerFilter(value) {
    this.ReceiptHeader.ContainerList = this.ReceiptHeader.Containers.filter((s) => (s.Number.toString().toLocaleUpperCase()).startsWith(value.toString().toLocaleUpperCase()));
  }

  handlePickupNumberFilter(value) {
    this.ReceiptHeader.PickupList = this.ReceiptHeader.Pickups.filter((s) => (s.PickupNumber.toString().toLocaleUpperCase()).startsWith(value.toString().toLocaleUpperCase()));
  }

  handleBinLocationFilter(value, roll: POLineModel) {
    roll.BinLocationList = roll.BinLocations.filter((s) => (s.AreaCode.toString().toLocaleUpperCase()).startsWith(value.toString().toLocaleUpperCase()));
  }

  handlePOFilter(value, roll: POLineModel) {
    roll.POList = roll.POs.filter((s) => (s.PONumber.toString().toLocaleUpperCase()).startsWith(value.toString().toLocaleUpperCase()));
  }

  handleAramarkItemFilter(value, roll: POLineModel) {
    roll.AramarkItemList = roll.AramarkItems.filter((s) => (s.AramarkItemCode.toString().toLocaleUpperCase()).startsWith(value.toString().toLocaleUpperCase()));
  }

  getTotalPOLines() {
    return this.ReceiptsGridData.length;
  }

  getTotalYards() {
    let totalYards = 0;
    this.ReceiptsGridData.forEach(c => {
      if (c.RollYards) {
        totalYards += Number(c.RollYards);
      }
    });
    return totalYards;
  }

  canDisableAddAndCopy() {
    return this.ReceiptHeader.TotalRolls === (this.ReceiptHeader.ReceivedRolls + this.ReceiptHeader.CurrentReceivingRolls)
  }

  sortChange(sort: SortDescriptor[]): void {
    this.sort = sort;
    this.sortReceipts();
  }

  sortReceipts() {
    this.gridView = {
      data: orderBy(this.ReceiptsGridData, this.sort),
      total: this.ReceiptsGridData.length
    };
  }

  validateReceipts(receipts: POLineModel) {
    let missedFields: Array<string> = [];
    let errorInfo: string;
    if (receipts.PO === undefined) {
      missedFields.push('PONumber');
    }
    if (receipts.AramarkItem === undefined) {
      missedFields.push('Aramark Item');
    }
    if (receipts.RollCase === undefined || receipts.RollCase === '') {
      missedFields.push('Roll Case #');
    }
    if (receipts.RollYards === undefined) {
      missedFields.push('Roll Yards');
    }
    if (receipts.LotNumber === undefined || receipts.LotNumber === '') {
      missedFields.push('Lot #');
    }
    if (receipts.BinLocation === undefined) {
      missedFields.push('Bin Location');
    }
    errorInfo = missedFields.join(" , ");

    if (errorInfo) {
      this.show = false;
      this._toastService.error("Kindly enter the required field(s):  " + errorInfo);
      receipts.IsValid = false;
    }
    else {
      receipts.IsValid = true;
    }
  }
  validateSaveReceipts(receipts: POLineModel) {
    let missedFields: Array<string> = [];
    let errorInfo: string;
    if (receipts.PO === undefined) {
      missedFields.push('PO #');
    }
    if (receipts.AramarkItem === undefined) {
      missedFields.push('Aramark Item');
    }
    if (receipts.RollCase === undefined || receipts.RollCase === '') {
      missedFields.push('Roll Case #');
    }
    if (receipts.RollYards === undefined) {
      missedFields.push('Roll Yards');
    }
    if (receipts.LotNumber === undefined || receipts.LotNumber === '') {
      missedFields.push('Lot #');
    }
    errorInfo = missedFields.join(", ");

    if (errorInfo) {
      this.show = false;
      this._toastService.error("Please enter the required field(s):  " + errorInfo);
      receipts.IsValid = false;
    }
    else {
      receipts.IsValid = true;
    }
  }

  getGridDetails() {
    let POlist: POModel[] = [];
    if (this.PODetailsList !== undefined || this.PODetailsList.length === 0) {
      this.PODetailsList.forEach(p => {
        POlist.push(new POModel(p.PONumber, p.POLineId, p.POHdrId));
      })
    }
    this._receivingService.GetGridDetails(this.ReceiptHeader.Container.Id, this.ReceiptHeader.Pickup.PickupNumber).subscribe(
      data => {
        data.forEach(d => {
          let receiptGrid = new POLineModel();
          receiptGrid.POList = receiptGrid.POs = POlist;
          if (receiptGrid.POList === undefined || receiptGrid.POList.length === 0) {
            receiptGrid.POList.push(new POModel(d.PONumber, d.POLineId, d.POHdrId));
            this.getAramarkItems(d.PONumber);
          }
          if (receiptGrid.AramarkItemList === undefined || receiptGrid.AramarkItemList.length === 0) {
            receiptGrid.AramarkItemList.push(new AramarkItemModel(d.AramarkItemId, d.AramarkItem, d.Color, d.Width, d.FiberContent, d.VendorItem, d.COO));
          }
          receiptGrid.BinLocations = receiptGrid.BinLocationList = this.BinLocationList;
          receiptGrid.AramarkItem = receiptGrid.AramarkItemList.find(a => a.AramarkItemCode === d.AramarkItem);
          receiptGrid.BinLocation = receiptGrid.BinLocationList.find(b => b.AreaCode === d.BinLocation);
          receiptGrid.PO = POlist.find(s => s.POLineId === d.POLineId);
          receiptGrid.RollYards = d.Quantity;
          receiptGrid.LotNumber = d.LotNumber;
          receiptGrid.Color = d.Color;
          receiptGrid.Width = d.Width;
          receiptGrid.FiberContent = d.FiberContent;
          receiptGrid.VendorItem = d.VendorItem;
          receiptGrid.COO = d.COO;
          receiptGrid.Defective = d.Defective;
          receiptGrid.POComments = d.POComments;
          receiptGrid.RollCase = d.RollCaseNbr;
          receiptGrid.POList = POlist;
          receiptGrid.RollCaseId = d.RollCaseId;
          this.ReceiptHeader.ReceivedRolls = d.ReceivedRolls;
          this.ReceiptHeader.ReceivedYards = d.ReceivedYards;
          this.ReceiptHeader.CurrentReceivingRolls = d.CurrentReceivingRolls;
          this.ReceiptHeader.CurrentReceivingYards = d.CurrentReceivingYards;
          this.ReceiptsGridData.push(receiptGrid);
        })
        this.gridView.data = this.ReceiptsGridData;
        this.show = false;
      },
      err => {
        this.show = false;
        this._errorService.error(err);
      }
    )
  }

  public onClose(event: any) {
    // (close)="onClose($event)" // add text to call onClose function
    event.preventDefault();
    //Close the list if the component is no longer focused
    setTimeout(() => {
      if (!this.dropdownlist.nativeElement.contains(document.activeElement)) {
        this.dropdownlist.toggle(false);
      }
    });
  }

  onBinChange(bin) {
    this._physicalAdjustmentsService.getRollCases(bin.Id).subscribe(
      data => {
        if (data.length > 0) {
          this._toastService.warn('Selected Bin contains one or more rolls');
        }
      },
      (err: HttpErrorResponse) => {
        this.show = false;
        this._errorService.error(err);
      });
  }
}

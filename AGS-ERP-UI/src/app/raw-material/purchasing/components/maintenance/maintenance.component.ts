import { Component, OnInit, group, Input } from "@angular/core";
import { Container } from "@angular/compiler/src/i18n/i18n_ast";
import {
  process,
  GroupDescriptor,
  State,
  aggregateBy,
  SortDescriptor,
  orderBy
} from "@progress/kendo-data-query";
import { Group } from "../../../inventory/container-po/models/group";
import { ContainerBuildDetail } from "../../../inventory/container-po/models/container-build-detail";
import { AramarkItemDetails } from "../../../inventory/physical-adjustments/models/aramark-item-details.model";
import { VendorDetails } from "../../../inventory/physical-adjustments/models/vendor-details.model";
import { ContainerType } from "../../../inventory/container-po/models/containertype";
import { PurchaseOrder } from "../../../inventory/container-po/models/purchase-order";
import { PickupLocation } from "../../../inventory/container-po/models/pickup-location";
import { FabricContainer } from "../../../receiving/models/fabric-container.model";
import { PurchasingService } from "../../purchasing.service";
import { POMaintenance } from "../../models/POMaintenance";
import { SessionStorageService } from "../../../../shared/wrappers/session-storage.service";
import { CommonService } from "../../../../shared/services/common.service";
import { POStatus } from "../../models/POStatus";
import {POHdr} from "../../models/POHdr"
import { HttpErrorResponse } from "@angular/common/http";
import { ErrorService } from "../../../../shared/services/error.service";
import { ToastService } from "../../../../shared/services/toast.service";
import { GridDataResult } from "@progress/kendo-angular-grid";
import { ConfirmationService } from "../../../../shared/services/confirmation.service";
import { MaintenanceGrid } from "../../models/MaintenanceGrid";
import { MaintenanceHeader } from "../../models/MaintenanceHeader";
import { element } from "protractor";
import { environment } from "../../../../../environments/environment";
import { OnDestroy } from "@angular/core/src/metadata/lifecycle_hooks";
import { LocalStorageService } from "../../../../shared/wrappers/local-storage.service";
@Component({
  selector: "app-maintenance",
  templateUrl: "./maintenance.component.html",
  styleUrls: ["./maintenance.component.css"],
  providers: [PurchasingService]
})
export class MaintenanceComponent implements OnInit,OnDestroy {
  @Input() PoHdrID: string;
  actualConfirmCaptured = true;
  actualDisabled = false;
  show = false;
  disableStatus = true;
  POGridData: GridDataResult = { data: [], total: 0 };
  selectedPOHdrId: any;
  headerData: Array<MaintenanceHeader> = [];
  gridData: Array<MaintenanceGrid> = [];
  saveData: Array<POMaintenance> = [];
  deleteData: Array<POMaintenance> = [];
  PONbr: number;
  PO_HDRId: number;
  PO_StatusID: number;
  OrderDate: Date = null;
  PODate: Date = null;
  Vendor: string = null;
  VendorId: number = 0;
  RequestedDate: Date = null;
  ShipTo: string = null;
  PoStatus: string = null;
  PoToVendor: string = null;
  PickUpLoc: string = null;
  OrigSchedCreateDate: Date = null;
  OrigSchedReceiptDate: Date = null;
  OrigSchedConfirmDate: Date = null;
  OrigSchedShipDate: Date = null;
  SchedCreateDate: Date = null;
  ScheduledDateShipped: Date = null;
  ScheduledDateReceived: Date = null;
  SchedConfirmDate: Date = null;
  ActualDateConfirmed: Date = null;
  ActualDateShipped: Date = null;
  ActualDateReceived: Date = null;
  ActualDateCreate: Date = null;
  OB: number = null;
  IsSelected: boolean = false;
  totalquantityCreated: number = 0;
  totalquantityConfirm: number = 0;
  totalquantityShipped: number = 0;
  totalquantityReceived: number = 0;
  totalquantityOB: number = 0;
  extendedCreated: number = 0;
  extendedConfirm: number = 0;
  extendedShipped: number = 0;
  extendedReceived: number = 0;
  extendedOB: number = 0;
  ActualConfirmMin: Date;
  selectedStatus: POStatus = null;
  status: Array<POStatus>;
  deleteDisable: boolean = true;
  unitPrice: string;
  statusDisable: boolean = true;
  DisableButton: boolean = true;
  checkboxDisable :boolean = true;
  count: number = 0;
  Cancelcount: number = 0;
  SchldShippedDisabled: boolean = false;
  SchldShippedReceived: boolean = false;
  PoStatusDisable: boolean = false;
  POReportUrl: string;
  currentSort: SortDescriptor[] = [];
  displayGridData:Array<string>=[];
  oldMaintaineceGrid:MaintenanceGrid[] = [];
  POCommunication:boolean = false;

  constructor(
    private _purchasingService: PurchasingService,
    private _sessionStorage: SessionStorageService,
    private _commonService: CommonService,
    private _errorService: ErrorService,
    private _toastService: ToastService,
    private _confirmationService: ConfirmationService,
    private _localStorageService: LocalStorageService
  ) {}

  ngOnInit() {
    this.show = true;
    this.getPOStatus();
    this.selectedPOHdrId = this._sessionStorage.get(
     "po_maintainance_selectedPO_POHdrId"
   );
    this.selectedPOHdrId = this.PoHdrID;
    // this.selectedPOHdrId = 260;
    this.getMaintenanceHeaderData(this.selectedPOHdrId);
    this.show = false;
    const login = JSON.parse(
      this._localStorageService.get('ags_erp_user_previlage')
    );
    let userName = login.FullName;
    //this.POReportUrl = environment.reportBaseUrl + 'RM_PO?PO=' + this.selectedPOHdrId;
    this.POReportUrl = environment.reportServerUrl + 'RM_PO&rs:Command=Render&rs:Format=PDF&PO=' + this.selectedPOHdrId + '&uname=' + userName;
  }
  getMaintenanceHeaderData(selectedPO_HDR_ID) {
    this._purchasingService
      .getMaintenanceHeaderData(selectedPO_HDR_ID)
      .subscribe(data => {
        if (data) {
          data.forEach(element => {
            this.PONbr = element.PONbr;
            this.PODate = element.PODate;
            this.Vendor = element.Vendor;
            this.RequestedDate = element.RequestedDate;
            this.ShipTo = element.ShipTo;
            this.PickUpLoc = element.PickupLoc;
            this.PoToVendor = element.POSentToVendor;
            this.VendorId = element.VendorId;
            this.PO_HDRId = element.POHdrID;
            this.PO_StatusID = element.POStatusId;
            this.POCommunication = element.IsPOCommunicated;

            if (element.OrigSchedCreateDate != null) {
              this.OrigSchedCreateDate = new Date(element.OrigSchedCreateDate);
            }
            if (element.OrigSchedShipDate != null) {
              this.OrigSchedShipDate = new Date(element.OrigSchedShipDate);
            }
            if (element.OrigSchedConfirmDate != null) {
              this.OrigSchedConfirmDate = new Date(
                element.OrigSchedConfirmDate
              );
            }
            if (element.OrigSchedReceiptDate != null) {
              this.OrigSchedReceiptDate = new Date(
                element.OrigSchedReceiptDate
              );
            }

            if (element.SchedCreateDate != null) {
              this.SchedCreateDate = new Date(element.SchedCreateDate);
            }
            if (element.SchedConfirmDate != null) {
              this.SchedConfirmDate = new Date(element.SchedConfirmDate);
            }
            if (element.SchedShipDate != null) {
              this.ScheduledDateShipped = new Date(element.SchedShipDate);
            }
            if (element.SchedReceiptDate != null) {
              this.ScheduledDateReceived = new Date(element.SchedReceiptDate);
            }

            if (element.ActualCreateDate != null) {
              this.ActualDateCreate = new Date(element.ActualCreateDate);
            }
            if (element.ActualConfirmDate != null) {
              this.ActualDateConfirmed = new Date(element.ActualConfirmDate);
              this.ActualDisabled(this.ActualDateConfirmed);
            }
            if (element.ActualShipDate != null) {
              this.ActualDateShipped = new Date(element.ActualShipDate);
              this.OnActualShipped(this.ActualDateShipped);
            }
            if (element.ActualReceiptDate != null) {
              this.ActualDateReceived = new Date(element.ActualReceiptDate);
              this.OnActualReceived(this.ActualDateReceived);
            }

            if(element.POStatus != null){
              this.selectedStatus = this.status.find( c=> c.Status === element.POStatus);
              this._sessionStorage.add("po_Maintenance_status",element.POStatus);
              this.DisableOnStatus(this.selectedStatus);
            }

            this.getMaintenanceGridData(element.POHdrID);
          });
        }
      });
  }

  getMaintenanceGridData(selectedPO_HDR_ID) {
    this.show = true;
    this._purchasingService
      .getMaintenanceGridData(selectedPO_HDR_ID)
      .subscribe(data => {
        let arrrayofPrice = [];
        if (data) {
          data.forEach(element => {
            element.UnitPrice =
              Number(Number(element.UnitPrice).toFixed(2)).toLocaleString();
            element.QtyConfirmed = element.QtyConfirmed.toLocaleString();
            arrrayofPrice.push(element);
          });
          this.gridData = arrrayofPrice;
          this.POGridData.data = this.gridData;
          this.oldMaintaineceGrid = arrrayofPrice;
          this.totalUnit();
        }
      });
    this.show = false;
  }
  formatTheDate(dateToFormat: Date) {
    let date = dateToFormat.getDate();
    let month = dateToFormat.getMonth() + 1;
    let year = dateToFormat.getFullYear();
    let formatedDate = month + "/" + date + "/" + year;
    return formatedDate;
  }
  totalUnit(UnitPrice?) {
    if (UnitPrice === "0" || UnitPrice === "undefined") {
      this._toastService.error(
        "Price Cannot be Zero,Please Enter Valid  Price"
      );
      this.DisableButton = true;
    } else if (UnitPrice) {
      this.DisableButton = false;
    }
    this.totalquantityCreated = 0;
    this.totalquantityConfirm = 0;
    this.totalquantityShipped = 0;
    this.totalquantityReceived = 0;
    this.totalquantityOB = 0;
    this.gridData.forEach(c => {
      this.totalquantityCreated = +c.QtyOrdered;
      this.totalquantityConfirm = +c.QtyConfirmed.replace(/,/g, "");
      this.totalquantityShipped = +c.QtyShipped;
      this.totalquantityReceived = +c.QtyReceived;
      this.totalquantityOB = +c.QtyOB;
    });
    this.extendedValue();
  }
  extendedValue() {
    if (this.gridData.length > 0) {
      this.gridData.forEach(c => {
        this.extendedCreated =
          +Number(c.UnitPrice.replace(/,/g, "").replace("$", "")) *
          c.QtyOrdered;
        this.extendedConfirm =
          +Number(c.UnitPrice.replace(/,/g, "").replace("$", "")) *
          Number(c.QtyConfirmed.replace(/,/g, ""));
        this.extendedShipped =
          +Number(c.UnitPrice.replace(/,/g, "").replace("$", "")) *
          c.QtyShipped;
        this.extendedReceived =
          +Number(c.UnitPrice.replace(/,/g, "").replace("$", "")) *
          c.QtyReceived;
        this.extendedOB =
          +Number(c.UnitPrice.replace(/,/g, "").replace("$", "")) * c.QtyOB;
      });
    }
  }
  updatetotalquantityConfirm(QtyConfirmed, UnitPrice) {
    if (QtyConfirmed && UnitPrice) {
      if (Number(QtyConfirmed) != 0 && Number(UnitPrice) != 0) {
        let totalquantityConfirm = 0;
        let extendedValue = 0;
        this.gridData.forEach(r => {
          totalquantityConfirm += Number(r.QtyConfirmed.replace(/,/g, ""));
          extendedValue =
            +Number(UnitPrice.replace(/,/g, "").replace("$", "")) *
            Number(r.QtyConfirmed.replace(/,/g, ""));
        });
        this.totalquantityConfirm = totalquantityConfirm;
        this.extendedConfirm = extendedValue;
        this.DisableButton = false;
      } else {
        this._toastService.error(
          "ConfirmQty Or Price Cannot be Zero,Please enter the valid data"
        );
      }
    }
  }
  ngOnDestroy() {
    this._commonService.Notify({
      key: 'destroyPOMaintenance',
      value: { 'PoHdrID': '' }
    });
  }
  getPOStatus() {
    this._purchasingService.getPOStatus().subscribe(
      data => {
        if (data) {
          if (data.length === 1) {
            this.selectedStatus = data[0];
          }
          this.status = data;
        }
      },
      (err: HttpErrorResponse) => {
        this._errorService.error(err);
      }
    );
  }

  OnCancel() {
    this._confirmationService.confirm({
      key: "message",
      value: {
        message:
          'Are you sure, you want to cancel this PO. Please select ok to proceed or cancel to abort',
        continueCallBackFunction: () =>
          this._purchasingService
            .Cancel(this.selectedPOHdrId)
            .subscribe(data => {
              if (data) {
                this.getMaintenanceHeaderData(this.selectedPOHdrId);
                this._toastService.success("PO is Canceled successfully");
                this.DisableButton = true;
              } else {
                this._toastService.error("Sorry.. Something went wrong");
              }
              this.show = false;
            })
      }
    });
  }
  post() {
    this.show = true;
    this._purchasingService.Cancel(this.selectedPOHdrId).subscribe(data => {
      if (!data) {
        this._toastService.error("Sorry.. Something went wrong");
      }
    });
    this._commonService.Notify({
      key: "POTab"
    });
    setTimeout(() => {
      this._commonService.Notify({
        key: "POTab"
      });
      this.show = false;
    }, 1);
  }
  OnSave(maintenanceGriddata: MaintenanceGrid[], selectedStatus) {
    let statusSaved = this._sessionStorage.get("po_Maintenance_status");
    this.count = 0;
    this.saveData = [];
    this.show = true;
    let confirmedFlag : boolean = false;

    maintenanceGriddata.forEach(c => {
      if (c.QtyReceived != 0) {
        this.count = +1;
      }
      let confirmedQuantity = Number(c.QtyConfirmed.replace(/,/g, ""));
      if(confirmedQuantity === 0){
        confirmedFlag = true;
      }
    });

    maintenanceGriddata.forEach(c => {
      if (c.QtyReceived === 0) {
        this.Cancelcount = +1;
      }
    });
    let modifiedValue:number=0;
      for(let i = 0;i < maintenanceGriddata.length;i++)
      {
        this.oldMaintaineceGrid[i].UnitPrice !== maintenanceGriddata[i].UnitPrice;
        this.oldMaintaineceGrid[i].QtyOrdered !== Number(maintenanceGriddata[i].QtyConfirmed);
        modifiedValue = modifiedValue + 1;
      }
    if (
      selectedStatus.StatusCode === "I" ||
      selectedStatus.StatusCode === "N" ||
      selectedStatus.StatusCode === "S"
    ) {
      this._toastService.error("PO Status Cannot be " + selectedStatus.Status);
      this.selectedStatus = this.status.find(x=>x.Status == statusSaved);
      this.show = false;
      return;          
    } else if (selectedStatus.StatusCode === "C" && this.Cancelcount >= 1) {
      this._toastService.error(
        "Cannot close this item as some PO has no ReceivedQty"
      );
    } else if (selectedStatus.StatusCode === "X" && this.count >= 1) {
      this._toastService.error("Cannot Cancel as this Item as some PO has no ReceivedQty ");
    } else if (statusSaved === "Planned" && selectedStatus.StatusCode === "C") {
      this._toastService.error(
        "PO Cannot be Closed as their is no receive QTY"
      );
      this.selectedStatus = this.status.find(x=>x.Status == statusSaved);
      this.show = false;
      return;      
    } else if (selectedStatus.Status === "Active" && confirmedFlag) {
      this.show = false;
      this._toastService.error(
        "Confirmed Quantity cannot be 0 when the PO Status is Active.");
        return;
    }
    else if(modifiedValue >0 && this.POCommunication)
    {
       this._toastService.error(
      "Please uncheck the check box for the PO to be send to vendor.");
    }
    else {
      let maintenanceObj = new POMaintenance();
      maintenanceObj.POHDRId = this.PO_HDRId; 
      maintenanceObj.POStatusId = selectedStatus.POStatusId;
      this. _sessionStorage.add('Hdr_Details', new POHdr(this.PONbr,selectedStatus.Status));
      maintenanceObj.SchedShipDate = this.ScheduledDateShipped;
      maintenanceObj.SchedReceiptDate = this.ScheduledDateReceived;
      maintenanceObj.ActualConfirmDate = this.ActualDateConfirmed;
      maintenanceObj.ActualShipDate = this.ActualDateShipped;
      maintenanceObj.ActualReceiptDate = this.ActualDateReceived;
      maintenanceGriddata.forEach(data => {
        maintenanceObj.LineNbr = data.LineNbr;
        maintenanceObj.POLineID = data.POLineId;
        maintenanceObj.POLineStatusID = data.POStatusId;
        maintenanceObj.POPrice = Number(
          data.UnitPrice.replace(/,/g, "").replace("$", "")
        );
        maintenanceObj.ConfirmYds = Number(data.QtyConfirmed.replace(/,/g, ""));
        maintenanceObj.ItemId = data.ItemId;
        maintenanceObj.VendorItemNbr = data.VendorItemNbr;
        maintenanceObj.QtyConfirmed = Number(
          data.QtyConfirmed.replace(/,/g, "")
        );
      
        maintenanceObj.QtyOrdered = data.QtyOrdered;
        maintenanceObj.Amount = Number(data.Amount);
        maintenanceObj.UOM = data.UOM;
        maintenanceObj.IsPOCommunicated = this.POCommunication;
      });

      this.saveData.push(maintenanceObj);
      
      this._confirmationService.confirm({
        key: "message",
        value: {
          message:
            "Are you sure you want to save / update the details? Press Ok to Continue and Cancel to go back to the page",
          continueCallBackFunction: () =>
            this._purchasingService.Save(this.saveData).subscribe(data => {
              if (data) {
                this._toastService.success("PO is saved successfully");
                if (
                  this.selectedStatus.StatusCode === "C" ||
                  this.selectedStatus.StatusCode === "X"
                ) {
                  this.statusDisable = true;
                  this.disableStatus = true;
                  this.SchldShippedReceived = true;
                  this.SchldShippedDisabled = true;
                  this.actualDisabled = true;
                  this.PoStatusDisable = true;
                  this.DisableButton = true;
                  this.checkboxDisable = true;
                }
              } else {
                this._toastService.error("Sorry.. Something went wrong");
              }
              this.show = false;
            })
        }
      });
    }
    this.show = false;
  }
  OnDelete(maintenanceGriddata: MaintenanceGrid[], selectedStatus) {
    if (selectedStatus.StatusCode === "A" && this.gridData.length > 1) {
      let row = this.gridData.filter(c => c.IsSelected == true);
      if (row.length > 0) {
        if(this.POCommunication)
        { 
          this._toastService.error(
          "Please uncheck the check box for the PO to be send to vendor.");
        }
        else
        {
          row.forEach(element => {
            let a = this.gridData.indexOf(element);
            if (a != -1) {
              this.gridData.splice(a, 1);
            }
          });
          this.deleteData = [];
          this.show = true;
          let maintenanceObj = new POMaintenance();
          maintenanceObj.POHDRId = this.PO_HDRId;
          maintenanceObj.POStatusId = this.PO_StatusID;
          maintenanceObj.SchedShipDate = this.ScheduledDateShipped;
          maintenanceObj.SchedReceiptDate = this.ScheduledDateReceived;
          maintenanceGriddata.forEach(data => {
            maintenanceObj.POLineID = data.POLineId;
            maintenanceObj.POLineStatusID = data.POStatusId;
            maintenanceObj.POPrice = Number(data.UnitPrice.replace(/,/g, ""));
            maintenanceObj.ConfirmYds = Number(
              data.QtyConfirmed.replace(/,/g, "")
            );
            this.totalUnit(maintenanceObj.POPrice);
          });
          this.deleteData.push(maintenanceObj);
          this._purchasingService.Delete(this.deleteData).subscribe(data => {
            if (data) {
              this._toastService.success("PO is Deleted successfully");
            } else {
              this._toastService.error("Sorry.. Something went wrong");
            }
            this.show = false;
          });
        }
        }
    }
    if (this.gridData.length === 1) {
      this._toastService.error("Cannot delete all the Items for this PO");
    } else {
      this._toastService.error("Cannot delete as status is not Active");
    }
  }
  onchangeActualConfirm(ActualDateCreate,ActualDateConfirmed, gridData: MaintenanceGrid[]) {
    let statusSaved = this._sessionStorage.get("po_Maintenance_status");
    let Changedstatus = [];
    if(ActualDateCreate > ActualDateConfirmed){
      this._toastService.error(
        "Actual Create Date should not be less than Actual Confirm Date"
      );
      this.ActualDateConfirmed = null;
    }
      if (ActualDateConfirmed) {
      if (statusSaved === "Planned") {
        gridData.forEach(c => {
          c.QtyConfirmed = String(c.QtyOrdered);
        });
        this.selectedStatus = this.status.find(c => c.Status === "Active");
      }
      this.actualDisabled = true;
      this.DisableButton = false;
      this.totalUnit();
      this.extendedValue();
    }      
  }
  OnActualReceived(ActualReceived) {
    if (ActualReceived) {
      this.SchldShippedReceived = true;
    }
  }

  OnActualShipped(ActualShipped) {
    if (ActualShipped) {
      this.SchldShippedDisabled = true;
    }
  }

  onSelect() {
    if (this.gridData.filter(c => c.IsSelected === true).length > 0) {
      this.deleteDisable = false;
    } else {
      this.deleteDisable = true;
    }
  }
  DisableOnStatus(status) {
    if (status.StatusCode === "C" || status.StatusCode === "X") {
      this.statusDisable = true;
      this.disableStatus = true;
      this.SchldShippedReceived = true;
      this.SchldShippedDisabled = true;
      this.actualDisabled = true;
      this.PoStatusDisable = true;
      this.checkboxDisable = true;
    }
    if (
      status.StatusCode === "A" ||
      status.StatusCode === "I" ||
      status.StatusCode === "S" ||
      status.StatusCode === "N" ||
      status.StatusCode === "L"
    ) {
      this.statusDisable = false;
      this.disableStatus = false;
      this.PoStatusDisable = false;
      this.SchldShippedDisabled = true;
      this.SchldShippedReceived = true;
      this.checkboxDisable = false;
    }
  }

  ActualDisabled(ActualDateConfirmed) {
    this.actualDisabled = false;
    if (ActualDateConfirmed) {
      this.actualDisabled = true;
    }
  }
  Datedisable(ActualDateReceived) {
    if (ActualDateReceived) {
      this.actualConfirmCaptured = true;
    }
  }

  OnStatusChange(selectedStatus) {
    let statusSaved = this._sessionStorage.get("po_Maintenance_status");
    this.disableStatus = true;
    if (selectedStatus != statusSaved) {
      this.DisableButton = false;
    if (statusSaved === "Active") {
        if (this.selectedStatus.StatusCode === "L") {
          this.actualDisabled = true;
          this.SchldShippedDisabled = true;
          this.SchldShippedReceived = true;
          this._toastService.error("Cannot Change status to " + this.selectedStatus.Status);
          this.selectedStatus = this.status.find(x=>x.Status === statusSaved);
          this.show = false;
          return this.selectedStatus;
        }else if (selectedStatus.StatusCode === "A") {
        this.disableStatus = false;
        this.statusDisable = false;
        this.actualDisabled = false;
        this.SchldShippedDisabled = false;
        this.SchldShippedReceived = false;
        this.selectedStatus = selectedStatus;
      } else if (selectedStatus.StatusCode === "C") {
        this.disableStatus = true;
        this.statusDisable = true;
        this.actualDisabled = true;
        this.SchldShippedDisabled = true;
        this.SchldShippedReceived = true;
        this.checkboxDisable = true;
        this.gridData.forEach(c => {
          if (c.QtyReceived <= 0) {
            this._toastService.error(
              "Selected PO cannot be" +
                selectedStatus.Status +
                " ,as there is some PO Quantity received "
            );
      this.selectedStatus = selectedStatus;
          } 
        
        });
        
      } else if (selectedStatus.StatusCode === "X") {
        this.disableStatus = true;
        this.statusDisable = true;
        this.actualDisabled = true;
        this.SchldShippedDisabled = true;
        this.SchldShippedReceived = true;
        this.checkboxDisable = true;
            this.gridData.forEach(c => {
              if (c.QtyReceived != 0) {
                this._toastService.error(
                  "Selected PO cannot be" +
                    selectedStatus.Status +
                    " ,as there is some PO Quantity received "
                );
                this.selectedStatus = this.status.find(x=>x.Status == statusSaved);
                this.show = false;
                return this.selectedStatus;
              }
            });
       }
      } else if (statusSaved === "Planned" || statusSaved === "Active") {
        if (
          this.selectedStatus.StatusCode === "L" || this.selectedStatus.StatusCode === "A"
        ) {
          this.disableStatus = false;
          this.statusDisable = false;
          this.actualDisabled = false;
          this.SchldShippedDisabled = false;
          this.SchldShippedReceived = false;
          this.checkboxDisable = false;
        } else if (
          selectedStatus.StatusCode === "I" ||
          this.selectedStatus.StatusCode === "S" ||
          this.selectedStatus.StatusCode === "N"
        ) {
          this.gridData.forEach(c => {
            this._toastService.error(
              "PO status cannot be " + selectedStatus.Status
            );     
          });
          this._toastService.error("Cannot Change status to " + this.selectedStatus.Status);
          this.selectedStatus = this.status.find(x=>x.Status === statusSaved);
          this.show = false;
          return this.selectedStatus;
        }
      } else if (
        selectedStatus.StatusCode === "I" ||
        this.selectedStatus.StatusCode === "S" ||
        this.selectedStatus.StatusCode === "N"
      ) {
        this.gridData.forEach(c => {
          this._toastService.error(
            "PO status cannot be " + selectedStatus.Status
          );
        });

        this._toastService.error("Cannot Change status to " + this.selectedStatus.Status);
        this.selectedStatus = this.status.find(x=>x.Status === statusSaved);
        this.show = false;
        return this.selectedStatus;
      }
    } else this.selectedStatus = selectedStatus;
    return this.selectedStatus;
  }

  /**
   * Get the type of sort & Field to sort
   * @param sort
   */
  currentSortChange(sort: SortDescriptor[]): void {
    this.currentSort = sort;
    this.sortCurrentDataRolls();
  }

  /**
   * Sort the data and assign to grid
   */
  sortCurrentDataRolls() {
    this.POGridData = {
      data: orderBy(this.POGridData.data, this.currentSort),
      total: this.POGridData.data.length
    };
  }



}

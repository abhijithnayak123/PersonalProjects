import { Component, OnInit, ViewChild } from '@angular/core';
import { ComboBoxComponent } from '@progress/kendo-angular-dropdowns/dist/es/combobox.component';
import { Vendor } from '../../models/vendor';
import { ContainerPoService } from '../../container-po.service';
import { SuccessService } from '../../../../../shared/services/success.service';
import { ErrorService } from '../../../../../shared/services/error.service';
import { ConfirmationService } from '../../../../../shared/services/confirmation.service';
import { AlertService } from '../../../../../shared/services/alert.service';
import { ToastService } from '../../../../../shared/services/toast.service';
import { HttpErrorResponse } from '@angular/common/http';
import { Container } from '../../models/container';
import { ContainerDetails } from '../../models/container-details';
import { ContainertrackingHeaderdetails } from '../../models/containertracking-headerdetails';
import { IntlService, NumberPipe } from '@progress/kendo-angular-intl';
import { Note } from '../../models/note';
import { error } from 'selenium-webdriver';
import { LocalStorageService } from '../../../../../shared/wrappers/local-storage.service';
import { ContainerTrackingInfo } from '../../models/container-tracking-info';
import { isNull } from 'util';
import { SortDescriptor } from '@progress/kendo-data-query/dist/es/sort-descriptor';
import { GridDataResult } from '@progress/kendo-angular-grid';
import { orderBy } from '@progress/kendo-data-query/dist/es/array.operators';
import { AttachmentType } from '../../models/attachment-type';
import { TrackingAttachment } from '../../models/tracking-attachment';
import 'rxjs/Rx';
import { environment } from '../../../../../../environments/environment';
import * as moment from 'moment';
import { CTHeader } from '../../models/ct-header';
import { DatePipe } from '@angular/common';
@Component({
  selector: 'app-container-tracking',
  templateUrl: './container-tracking.component.html',
  styleUrls: ['./container-tracking.component.css'],
  providers: [ContainerPoService, NumberPipe, DatePipe]
})
export class ContainerTrackingComponent implements OnInit {
  @ViewChild('combo') public combo: ComboBoxComponent;
  @ViewChild('fileInput') fileInput;
  vendors: Array<Vendor>;
  vendorsList: Array<Vendor>;
  public gridNotes: string;
  selectedVendor: Vendor;
  selectedVendorCopy: Vendor;
  containerList: Array<Container>;
  containers: Array<Container>;
  selectedContainer: Container;
  containerDetails: Array<ContainerDetails> = [];
  containerHeaderDetails: ContainertrackingHeaderdetails;
  ScheduleDate: Date;
  pickUpDate: Date;
  pickUpDateCopy: Date;
  expBorderDate: Date;
  NotesGrid: Array<Note>;
  validateExpectedBorderDate: Date;
  totalRolls: number;
  totalYards: number;
  openedAttachment: boolean;
  openedCommentbox: boolean;
  created: boolean;
  shipped: boolean;
  confirm: boolean;
  border: boolean;
  factory: boolean;
  recived: boolean;
  show: boolean;
  fieldDisable: boolean;
  login: any;
  currentEventActualDate: string;
  sort: SortDescriptor[] = [];
  gridView: GridDataResult = { data: [], total: 0 };
  notesSort: SortDescriptor[] = [];
  notesGridView: GridDataResult = { data: [], total: 0 };
  attachmentList: Array<TrackingAttachment> = [];
  attachments: Array<TrackingAttachment> = [];
  attachmentTypeList: Array<AttachmentType> = [];
  selectedAttachmentType: AttachmentType;
  initialCurrentStatus: number;
  selectedAttachment: TrackingAttachment;
  enableAttachmentButton: boolean;
  isDataChanged: boolean;
  ContainerReportUrl: string;
  initialActualOriginDate: Date;
  initialConfirmDate: Date;
  disableFields = true;
  constructor(
    private _containerPoService: ContainerPoService,
    private _successService: SuccessService,
    private _errorService: ErrorService,
    private _confirmationService: ConfirmationService,
    private _alertService: AlertService,
    private _toastService: ToastService,
    private _intlService: IntlService,
    private _localStorageService: LocalStorageService,
    private _numberPipe: NumberPipe,
    private _datePipe: DatePipe
  ) {
  }

  ngOnInit() {
    this.getVendors();
    this.ScheduleDate = null;
    this.openedAttachment = this.openedCommentbox = false;
    this.gridNotes = '';
    this.totalRolls = this.totalYards = 0;
    this.show = this.created = this.shipped = this.confirm = this.border = this.factory = this.recived = this.fieldDisable = false;
    this.defaultHeader();
  }
  getConfirmationOnVendorChange() {
    if (this.isDataChanged && (this.selectedVendor !== this.selectedVendorCopy)) {
      this._confirmationService.confirm(
        {
          key: 'message',
          value: {
            message: 'This Page contain unsaved data. Would you like to change container?<br> Press "OK" to continue or "Cancel" to abort.',
            continueCallBackFunction: () => this.resetData(),
            cancelCallBackFunction: () => this.cancelVendorChange()
          }
        }
      );
    } else {
      this.resetData();
    }
  }
  resetData() {
    this.show = this.created = this.shipped = this.confirm = this.border = this.factory = this.recived = this.fieldDisable = false;
    this.containerDetails = this.NotesGrid = [];
    this.gridView = this.notesGridView = { data: [], total: 0 };
    this.selectedContainer = this.selectedAttachment = this.selectedAttachmentType = null;
    this.isDataChanged = false;
    this.selectedVendorCopy = this.selectedVendor;
    this.pickUpDateCopy = this.pickUpDate;
    this.totalRolls = this.totalYards = 0;
    this.defaultHeader();
    this.getContainerList();
  }

  cancelVendorChange() {
    const vendor = this.selectedVendorCopy;
    this.selectedVendor = new Vendor(vendor.VendorId, vendor.RMVendorCode, vendor.RMVendorName);
    this.pickUpDate = this.pickUpDateCopy === undefined ? null : new Date(this.pickUpDateCopy);
  }

  getContainerList() {
    this.show = true;
    const vendorId = this.selectedVendor.VendorId;
    const date = this.pickUpDate != null ? moment(this.pickUpDate).format('MM-DD-YYYY') : null;
    this._containerPoService.containerTrackingGetContainers(vendorId, date).subscribe(
      data => {
        this.disableFields = false;
        this.containerList = this.containers = data;
        if(this.containers.length === 1){
          this.selectedContainer = this.containers[0];
          this.getConfirmationOnChangeOfContainer(this.containers[0])
        }
        this.show = false;
      },
      (err: HttpErrorResponse) => {
        this.show = false;
        this._errorService.error(err);
      });
  }

  getVendors() {
    this.show = true;
    this._containerPoService.containerTrackingGetVendors().subscribe(
      data => {
        if (data) {
          this.vendors = this.vendorsList = data;
          if(this.vendors.length === 1){
          this.selectedVendor = this.vendors[0];
          }
        }
        this.show = false;
      },
      (err: HttpErrorResponse) => {
        this.show = false;
        this._errorService.error(err);
      });
  }

  getConfirmationOnChangeOfContainer(container) {
    if (this.isDataChanged) {
      this._confirmationService.confirm(
        {
          key: 'message',
          value: {
            message: 'This Page contain unsaved data. Would you like to change container?<br> Press "OK" to continue or "Cancel" to abort.',
            continueCallBackFunction: () => this.onContainerChange(container),
            cancelCallBackFunction: () => this.cancelContainerChange()
          }
        }
      );
    } else {
      this.onContainerChange(container);
    }
  }

  cancelContainerChange() {
    const container = this.selectedContainer;
    this.selectedContainer = new Container(container.ContainerId, container.VendorId, container.ContainerNbr, container.VendorNames);
  }
  onContainerChange(container) {
    this.isDataChanged = false;
    this.selectedContainer = null;
    this.selectedContainer = container;
    if (this.selectedContainer) {
      this.getHeaderData();
    }
    const login = JSON.parse(
      this._localStorageService.get('ags_erp_user_previlage')
    );
    let userName = login.FullName;
    //this.ContainerReportUrl = environment.reportBaseUrl + 'Container_Summary?ContainerId=' + container.ContainerId;
    this.ContainerReportUrl = environment.reportServerUrl + 'Container_Summary&rs:Command=Render&rs:Format=PDF&ContainerId=' + container.ContainerId + '&uname=' + userName;
  }

  getHeaderData() {
    this.show = true;
    this._containerPoService.containerTrackingGetHeaderDetails(this.selectedContainer.ContainerId).subscribe(
      data => {
        this.containerHeaderDetails = data;
        this.initialCurrentStatus = this.containerHeaderDetails.CurrEventId;
        this.containerHeaderDetails.ExpectedBorderDate =
          this.containerHeaderDetails.ExpectedBorderDate != null ? new Date(this.containerHeaderDetails.ExpectedBorderDate) : null;
        this.containerHeaderDetails.PrevEventExpectedDate =
          this.containerHeaderDetails.PrevEventExpectedDate != null ? new Date(this.containerHeaderDetails.PrevEventExpectedDate) : null;
        this.containerHeaderDetails.CurrEventExpectedDate =
          this.containerHeaderDetails.CurrEventExpectedDate != null ? new Date(this.containerHeaderDetails.CurrEventExpectedDate) : null;
        this.containerHeaderDetails.NextEventExpectedDate =
          this.containerHeaderDetails.NextEventExpectedDate != null ? new Date(this.containerHeaderDetails.NextEventExpectedDate) : null;
        this.containerHeaderDetails.ExpectedConfirmationDate =
          this.containerHeaderDetails.ExpectedConfirmationDate != null ?
            new Date(this.containerHeaderDetails.ExpectedConfirmationDate) : null;
        this.containerHeaderDetails.ExpectedPickupDate =
          this.containerHeaderDetails.ExpectedPickupDate != null ? new Date(this.containerHeaderDetails.ExpectedPickupDate) : null;
        this.containerHeaderDetails.ExpectedAtFactoryDate =
          this.containerHeaderDetails.ExpectedAtFactoryDate != null ? new Date(this.containerHeaderDetails.ExpectedAtFactoryDate) : null;
        this.containerHeaderDetails.ExpectedDcDate =
          this.containerHeaderDetails.ExpectedDcDate != null ? new Date(this.containerHeaderDetails.ExpectedDcDate) : null;
        this.containerHeaderDetails.CarrierConfirmationDate =
          this.containerHeaderDetails.CarrierConfirmationDate != null ?
            new Date(this.containerHeaderDetails.CarrierConfirmationDate) : null;
        this.containerHeaderDetails.VendorConfirmationDate =
          this.containerHeaderDetails.VendorConfirmationDate != null ?
            new Date(this.containerHeaderDetails.VendorConfirmationDate) : null;
        this.containerHeaderDetails.ActualOriginDate =
          this.containerHeaderDetails.ActualOriginDate != null ? new Date(this.containerHeaderDetails.ActualOriginDate) : null;
        this.containerHeaderDetails.ActualBorderDate =
          this.containerHeaderDetails.ActualBorderDate != null ? new Date(this.containerHeaderDetails.ActualBorderDate) : null;
        this.containerHeaderDetails.ActualAtFactoryDate =
          this.containerHeaderDetails.ActualAtFactoryDate != null ? new Date(this.containerHeaderDetails.ActualAtFactoryDate) : null;
        this.containerHeaderDetails.ActualDcDate =
          this.containerHeaderDetails.ActualDcDate != null ? new Date(this.containerHeaderDetails.ActualDcDate) : null;
        this.containerHeaderDetails.CreatedDate =
          this.containerHeaderDetails.CreatedDate != null ? new Date(this.containerHeaderDetails.CreatedDate) : null;
        this.containerHeaderDetails.OGExpectedBorderDate = this.containerHeaderDetails.ExpectedBorderDate;
        this.containerHeaderDetails.OGExpectedAtFactoryDate = this.containerHeaderDetails.ExpectedAtFactoryDate;
        this.containerHeaderDetails.OGExpectedPickupDate = this.containerHeaderDetails.ExpectedPickupDate;
        this.containerHeaderDetails.OGExpectedDcDate = this.containerHeaderDetails.ExpectedDcDate;
        this.containerHeaderDetails.OGExpectedConfirmationDate = this.containerHeaderDetails.ExpectedConfirmationDate;
        this.initialActualOriginDate = this.containerHeaderDetails.ActualOriginDate;
        this.initialConfirmDate = this.containerHeaderDetails.CarrierConfirmationDate;
        this.getAttachmentsDDL();
        this.SetDates();
        this.GetHeaderStatus();
        this.DisableFields();
        this.show = false;
      },
      (err: HttpErrorResponse) => {
        this.show = false;
        this._errorService.error(err);
      });
  }

  getGridData() {
    this.show = true;
    this.totalRolls = this.totalYards = 0;
    this._containerPoService.containerTrackingGetContainerDetails(this.selectedContainer.ContainerId, this.selectedVendor.VendorId)
      .subscribe(
      data => {
        this.containerDetails = data;
        this.containerDetails.forEach(p => {
          this.totalRolls += p.TotalRolls,
            this.totalYards += p.TotalYards;
          p.TotalYards = this.formatQuantity(p.TotalYards);
          p.TotalRolls = this.formatQuantity(p.TotalRolls);
          const login = JSON.parse(
            this._localStorageService.get('ags_erp_user_previlage')
          );
          let userName = login.FullName;
          // p.VendorReportUrl = environment.reportBaseUrl + 'Pickup_Summary?ContainerId=' + p.ContainerId + '&PickupNbr=' + p.ContainerStopId;
          p.VendorReportUrl = environment.reportServerUrl + 'Pickup_Summary&rs:Command=Render&rs:Format=PDF&ContainerId=' + p.ContainerId + '&PickupNbr=' + p.ContainerStopId + '&uname=' + userName;
        });
        this.loadGrid();
        this.getNotesGridData();
        this.show = false;
      },
      (err: HttpErrorResponse) => {
        this.show = false;
        this._errorService.error(err);
      });
  }

  getNotesGridData() {
    this.show = true;
    this._containerPoService.ContainerTrackingGetNotes(this.selectedContainer.ContainerId).subscribe(
      data => {
        this.NotesGrid = data;
        this.notesSort = [{ 'field': 'CreatedOn', 'dir': 'desc' }];
        this.sortNotesChange(this.notesSort);
        this.show = false;
      },
      (err: HttpErrorResponse) => {
        this.show = false;
        this._errorService.error(err);
      }
    );
  }
  handleContainerFilter(value) {
    this.containers = this.containerList.filter((s) => (s.ContainerNbr.toUpperCase()).startsWith(value.toUpperCase()));
  }
  handleFilter(value: string) {
    this.vendors = this.vendorsList.filter((s) => (s.RMVendorName.toUpperCase()).startsWith(value.toUpperCase()));
  }

  clearForm() {
    this.attachmentTypeList = this.containerDetails = this.NotesGrid = this.containers = this.containerList = [];
    this.gridView = this.notesGridView = { data: [], total: 0 };
    this.selectedContainer = this.selectedVendor = null;
    this.created = this.shipped = this.confirm = this.border = this.factory = this.recived = false;
    this.selectedAttachment = this.selectedAttachmentType = null;
    this.totalRolls = this.totalYards = 0;
    this.defaultHeader();
  }

  SetDates() {
    const headerdetails = this.containerHeaderDetails;
    const curEvent = headerdetails.CurrEventId;
    const preEvent = headerdetails.PrevEventId;
    const nextEvent = headerdetails.NextEventId;

    if (curEvent === 1 && nextEvent === 2) {
      headerdetails.CreatedDate = headerdetails.CurrEventExpectedDate;
      headerdetails.ExpectedConfirmationDate = headerdetails.NextEventExpectedDate;
    } else if (curEvent === 2 && nextEvent === 3) {
      headerdetails.CreatedDate = headerdetails.PrevEventExpectedDate;
      headerdetails.ExpectedConfirmationDate = headerdetails.CurrEventExpectedDate;
      headerdetails.ExpectedPickupDate = headerdetails.NextEventExpectedDate;
    } else if (curEvent === 3 && nextEvent === 4) {
      headerdetails.ExpectedConfirmationDate = headerdetails.PrevEventExpectedDate;
      headerdetails.ExpectedPickupDate = headerdetails.CurrEventExpectedDate;
      headerdetails.ExpectedBorderDate = headerdetails.NextEventExpectedDate;
    } else if (curEvent === 4 && nextEvent === 5) {
      headerdetails.ExpectedPickupDate = headerdetails.PrevEventExpectedDate;
      headerdetails.ExpectedBorderDate = headerdetails.CurrEventExpectedDate;
      headerdetails.ExpectedAtFactoryDate = headerdetails.NextEventExpectedDate;
    } else if (curEvent === 5 && nextEvent === 6) {
      headerdetails.ExpectedBorderDate = headerdetails.PrevEventExpectedDate;
      headerdetails.ExpectedAtFactoryDate = headerdetails.CurrEventExpectedDate;
      headerdetails.ExpectedDcDate = headerdetails.NextEventExpectedDate;
    } else if (curEvent === 6) {
      headerdetails.ExpectedAtFactoryDate = headerdetails.PrevEventExpectedDate;
      headerdetails.ExpectedDcDate = headerdetails.CurrEventExpectedDate;
    }

  }

  public closeAttachment(status) {
    this.selectedAttachmentType = null;
    this.openedAttachment = false;
  }

  public closeCommentbox(status) {
    this.gridNotes = '';
    this.openedCommentbox = false;
  }

  public openAttachment() {
    if (this.attachmentTypeList.length === 0) {
      this.getAttachmentTypeDDL();
    }
    this.enableAttachmentButton = false;
    this.openedAttachment = true;
  }

  public openCommentbox() {
    this.openedCommentbox = true;
  }
  SaveComments() {
    this.show = true;
    this.login = JSON.parse(
      this._localStorageService.get('ags_erp_user_previlage'));

    const notes = new Note(this.selectedContainer.ContainerId, this.selectedContainer.ContainerNbr, 0,
      this.gridNotes, '', new Date(), new Date());

    this._containerPoService.PostCTNotes(notes).subscribe(
      x => {
        if (!x) {
          this.show = false;
          this.closeCommentbox('');
          this._toastService.error('Error!! Please try again.');
        } else {
          this.show = false;
          this._toastService.success('Comments saved successfully.');
          this.closeCommentbox('');
          this.getNotesGridData();
        }
      },
      (err: HttpErrorResponse) => {
        this.closeCommentbox('');
        this.show = false;
        this._errorService.error(err);
      });

  }
  clearSearch() {
    this.selectedVendor = null;
    this.pickUpDate = null;
    this.vendors = this.vendorsList;
  }
  GetHeaderStatus() {
    const headerdetails = this.containerHeaderDetails;
    const curEvent = headerdetails.CurrEventId;
    const preEvent = headerdetails.PrevEventId;
    const nextEvent = headerdetails.NextEventId;
    this.created = this.shipped = this.confirm = this.border = this.factory = this.recived = false;

    if (curEvent === 1 && nextEvent === 2) {
      this.created = this.confirm = true;
    } else if (curEvent === 2 && nextEvent === 3) {
      this.created = this.confirm = this.shipped = true;
    } else if (curEvent === 3 && nextEvent === 4) {
      this.confirm = this.shipped = this.border = true;
    } else if (curEvent === 4 && nextEvent === 5) {
      this.shipped = this.border = this.factory = true;
    } else if (curEvent === 5 && nextEvent === 6) {
      this.border = this.factory = this.recived = true;
    }
  }

  UpdateHeaderStatus(currentStatus: number, currentDate: string, negDate: string) {
    const headerdetails = this.containerHeaderDetails;
    this.isDataChanged = true;

    headerdetails.CurrEventId = currentStatus;
    headerdetails.PrevEventId = currentStatus - 1;
    headerdetails.NextEventId = currentStatus + 1;
    if (currentDate != null) {
      if (!this.containerHeaderDetails[currentDate]) {
        this.containerHeaderDetails[currentDate] = new Date();
      }
      this.currentEventActualDate = currentDate;
    } else {
      this.currentEventActualDate = 'CreatedDate';
    }
    if (negDate != null) {
      this.containerHeaderDetails[negDate] = null;
    }
    this.DisableFields();
  }
  DisableFields() {
    const headerdetails = this.containerHeaderDetails;
    if (headerdetails.CurrEventId === 3) {
      this.fieldDisable = false;
    } else {
      this.fieldDisable = true;
    }
  }

  postConfirm() {
    const status = this.containerHeaderDetails.CurrEventId;
    let isValid = true;
    if (status === 3) {
      isValid = this.validateGrid();
    }

    if (this.initialCurrentStatus === 3 && status === 2) {
      this._confirmationService.confirm({
        key: 'message',
        value: {
          message: 'Are you sure , you want to revert back to Confirm status ?' +
            ' On doing so all shipment details entered will be removed for the container.' +
            '<br> Please select ok to proceed and cancel to abort.',
          continueCallBackFunction: () => this.postData(),
          cancelCallBackFunction: () => this.revertStatus()
        }
      });
      return;
    }

    if (isValid) {
      this._confirmationService.confirm({
        key: 'message',
        value: {
          message: 'Are you sure you want to save / update the details?',
          continueCallBackFunction: () => this.postData()
        }
      });
    }
  }

  revertStatus() {
    this.containerHeaderDetails.CurrEventId = this.initialCurrentStatus;
    this.containerHeaderDetails.CarrierConfirmationDate = new Date(this.initialConfirmDate);
    this.containerHeaderDetails.ActualOriginDate = new Date(this.initialActualOriginDate);
  }
  postData() {
    this.show = true;
    let action = 10;
    if (this.currentEventActualDate !== undefined) {
      this.containerHeaderDetails.CurrEventExpectedDate = this.containerHeaderDetails[this.currentEventActualDate];
    }
    if (this.initialCurrentStatus > this.containerHeaderDetails.CurrEventId) {
      action = 20;
      this.containerHeaderDetails.CurrEventId = this.initialCurrentStatus;
    }
    const header = this.getHeaderDetails();
    const ctDetails = new ContainerTrackingInfo(header, this.containerDetails, action);
    ctDetails.TrackingDetails.forEach(r => {
     if(typeof r.TotalYards === "string"){
        r.TotalYards = Number(r.TotalYards.replace(/,/g,''));
     }
    if(typeof r.TotalRolls === "string"){
        r.TotalRolls = Number(r.TotalRolls.replace(/,/g,''));
     }
    });
    this._containerPoService.PostContainerTrackingrData(ctDetails).subscribe(
      data => {
        if (data) {
          this._toastService.success('Container details and status acknowledged');
          if (this.containerHeaderDetails.CurrEventId === 6) {
            this.clearForm();
          } else {
            this.getNotesGridData();
            this.getHeaderData();
            this.getGridData();
          }
          this.isDataChanged = false;
          this.GetHeaderStatus();
        } else {
          this._toastService.error('Error!! Please try again.');
        }
        this.show = false;
      },
      (err: HttpErrorResponse) => {
        this.show = false;
        this._errorService.error(err);
      });
  }

  validateGrid(): boolean {
    const gridData = this.containerDetails;
    const errorInfo: Array<string> = [];
    let erroMessage: string;
    if (gridData.some(x => isNull(x.ShipmentNbr) || x.ShipmentNbr === '')) {
      errorInfo.push('Shipment');
    }
    if (gridData.some(x => isNull(x.AsnNbr) || x.AsnNbr === '')) {
      errorInfo.push('ASN');
    }
    if (gridData.some(x => isNull(x.BillOfLadingNbr) || x.BillOfLadingNbr === '')) {
      errorInfo.push('Bill Of Lading');
    }
    if (gridData.some(x => isNull(x.TotalRolls) || x.TotalRolls === 0 || x.TotalRolls === '')) {
      errorInfo.push('Total Rolls');
    }
    if (gridData.some(x => isNull(x.TotalYards) || x.TotalYards === 0 || x.TotalYards === '')) {
      errorInfo.push('Total Yards');
    }
    erroMessage = errorInfo.join(', ');

    if (erroMessage) {
      this._toastService.error('Please enter ' + erroMessage);
      return false;
    }
    return true;
  }

  sortChange(sort: SortDescriptor[]): void {
    this.sort = sort;
    this.loadGrid();
  }
  loadGrid() {
    this.gridView = {
      data: orderBy(this.containerDetails, this.sort),
      total: this.containerDetails.length
    };
  }

  onRollsChange(pickupNum) {
    if(pickupNum.TotalRolls >= 1000){
      this._toastService.error("Total Rolls Cannot be greater than 999")
      pickupNum.TotalRolls = 0;
    }
    else{
    this.totalRolls = 0;
    this.isDataChanged = true;
    this.containerDetails.forEach(p => {
      this.totalRolls += Number(p.TotalRolls);
    });
    }
  }

  onYardsChanges(roll) {
    this.totalYards = 0;
    this.isDataChanged = true;
    this.containerDetails.forEach(p => {
      this.totalYards += Number(p.TotalYards);
    });
    if(Number(roll.TotalYards.replace(/,/g,'')) !== roll.RequestedYards){
      this._alertService.alert({
          key: 'alertMessage',
          value: "Total Yards entered does not match with the Total Quantity for the Pickup #."+"<br>"+"Please update the Total Quantity in the Build Screen, else Missed Quantity will not be captured"
      });
    }
  }
  sortNotesChange(sort: SortDescriptor[]): void {
    this.notesSort = sort;
    this.loadNotesGrid();
  }
  loadNotesGrid() {
    this.notesGridView = {
      data: orderBy(this.NotesGrid, this.notesSort),
      total: this.NotesGrid.length
    };
  }
  getAttachmentsDDL() {
    this.show = true;
    this._containerPoService.ContainerTrackingGetAttachmentsList(this.selectedContainer.ContainerId).subscribe(
      data => {
        if (data) {
          this.attachmentList = this.attachments = data;
          this.getGridData();
        } else {
          this._toastService.error('Error occured while getting Attachment(s).');
        }
        this.show = false;
      },
      (err: HttpErrorResponse) => {
        this.show = false;
        this._errorService.error(err);
      });
  }

  getAttachmentTypeDDL() {
    this._containerPoService.ContainerTrackingGetAttachmentTypes().subscribe(
      data => {
        if (data) {
          this.attachmentTypeList = data;
        } else {
          this._toastService.error('Error occured while getting Attachment(s).');
        }
      },
      (err: HttpErrorResponse) => {
        this._errorService.error(err);
      });
  }

  onActualDateChange(currentDate, prevDate) {
    let pDate = this.containerHeaderDetails[prevDate];
    let cDate = this.containerHeaderDetails[currentDate];
    cDate = new Date(cDate.getFullYear(), cDate.getMonth(), cDate.getDate());
    pDate = new Date(pDate.getFullYear(), pDate.getMonth(), pDate.getDate());
    this.isDataChanged = true;
    if (cDate < pDate) {
      this.containerHeaderDetails[currentDate] = pDate;
      this._toastService.error('Please enter a valid Date. Date cannot be less than ' +
        moment(pDate).format('MM/DD/YYYY'));
    }
  }

  onScheduleDateChange(currentDate, prevDate) {
    let pDate = this.containerHeaderDetails[prevDate];
    let cDate = this.containerHeaderDetails[currentDate];
    cDate = new Date(cDate.getFullYear(), cDate.getMonth(), cDate.getDate());
    pDate = new Date(pDate.getFullYear(), pDate.getMonth(), pDate.getDate());

    this.isDataChanged = true;
    if (cDate < pDate) {
      const ogValue = this.containerHeaderDetails['OG' + currentDate];
      this.containerHeaderDetails[currentDate] = ogValue === null ? null : new Date(ogValue);
      this._toastService.error('Please enter a valid Date. Date cannot be less than ' +
        moment(pDate).format('MM/DD/YYYY'));
    }
  }

  defaultHeader() {
    this.containerHeaderDetails = new ContainertrackingHeaderdetails(0, 0, '', 0, '', 0,
      '', 0, '', null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, '', 0,
      '', '', '', '', '', '', null, null, null, null, null, '', null);
  }

  verifyUpload() {
    const file = this.fileInput.nativeElement;
    if (file.files && file.files[0] && this.selectedAttachmentType) {
      const fileToUpload = file.files[0].name;
      // .doc,.docx, .pdf, .xls, .xlsx
      if (fileToUpload.search('.docx') > 0 || fileToUpload.search('.pdf') > 0 || fileToUpload.search('.xlsx') > 0 ||
        fileToUpload.search('.doc') > 0 || fileToUpload.search('.xls') > 0) {
        this.uploadFile();
      } else {
        this._toastService.error('Please select only Word/PDF/Excel file to upload');
      }
    } else {
      this._toastService.error('Please select the required fields(Document Type / File Name)');
    }
  }
  uploadFile() {
    const file = this.fileInput.nativeElement;
    if (file.files && file.files[0]) {
      const fileToUpload = file.files[0];
      const formFile = new FormData();
      formFile.append('file', fileToUpload);
      const attachment = new TrackingAttachment(0, this.selectedContainer.ContainerId, this.selectedContainer.ContainerNbr, 0,
        this.selectedAttachmentType.AttachmentTypeId, this.selectedAttachmentType.Code, '', fileToUpload.name, '', '',
        '', '', new Date(), new Date());

      // appending the attachment information to the form.
      formFile.append('attachment', JSON.stringify(attachment));

      this._containerPoService.ContainerTrackingUploadFile(formFile).subscribe(
        res => {
          this.openedAttachment = false;
          if (res) {
            this.getAttachmentsDDL();
            this._toastService.success('File Uploaded successfully.');
          } else {
            this._toastService.error('File Uploaded failed. Please try again.');
          }
          this.selectedAttachmentType = null;
        },
        (err: HttpErrorResponse) => {
          this.openedAttachment = false;
          this.selectedAttachmentType = null;
          this._errorService.error(err);
        }
      );
    }
  }

  validateFile() {
    const file = this.fileInput.nativeElement;
    if (file.files && file.files[0]) {
      const fileToUpload = file.files[0];
      this.enableAttachmentButton = true;
    }
  }
  downloadFile() {
    // using blob
    // this._containerPoService.ContainerTrackingDownloadFile(this.selectedAttachment.FileName, this.selectedAttachment.ContainerNbr)
    //   .subscribe(
    //   res => {
    //     const blob = new Blob([res], { type: 'application/octet-stream' });
    //     const link = document.createElement('a');
    //     link.href = window.URL.createObjectURL(blob);
    //     link.download = this.selectedAttachment.FileName;
    //     link.click();
    //   },
    //   (err: HttpErrorResponse) => {
    //     this._errorService.error(err);
    //   });

    // hitting URL directly in window
    const downloadUrl = environment.apiUrl + 'containertracking/download-file/';
    window.open(downloadUrl + this.selectedAttachment.DownloadFileName + '/' + this.selectedAttachment.DisplayName
      + '/' + this.selectedAttachment.ContainerNbr);
  }

  deleteConfirm() {
    this._confirmationService.confirm({
      key: 'message',
      value: {
        message: 'Are you sure you want to delete?',
        continueCallBackFunction: () => this.deleteFile()
      }
    });
  }
  deleteFile() {
    this._containerPoService.ContainerTrackingDeleteFile(this.selectedAttachment)
      .subscribe(
      res => {
        this.getAttachmentsDDL();
        this.selectedAttachment = null;
        this._toastService.success('File deleted successfully.');
      },
      (err: HttpErrorResponse) => {
        this._errorService.error(err);
      });
  }

  onGridValueChange() {
    this.isDataChanged = true;
  }

  handleAttachmentFilter(value) {
    this.attachmentList = this.attachments.filter((s) => (s.FileName.toUpperCase()).startsWith(value.toUpperCase()));
  }

  formatQuantity(value: number) {
    return this._numberPipe.transform(value);
  }
  getHeaderDetails() {
    const headerDetails = this.containerHeaderDetails;
    const header = new CTHeader(headerDetails.ContainerId, headerDetails.VendorId, headerDetails.CurrEventId,
      this.formatDate(headerDetails.CurrEventExpectedDate), headerDetails.ContainerNbr,
      this.formatDate(headerDetails.ExpectedConfirmationDate), this.formatDate(headerDetails.ExpectedPickupDate),
      this.formatDate(headerDetails.ExpectedBorderDate), this.formatDate(headerDetails.ExpectedAtFactoryDate),
      this.formatDate(headerDetails.ExpectedDcDate), headerDetails.Comments, headerDetails.ContainerRefNumber);
    return header;
  }

  formatDate(date) {
    return this._datePipe.transform(date, 'MM/dd/yyyy hh:mm:ss');
  }
}

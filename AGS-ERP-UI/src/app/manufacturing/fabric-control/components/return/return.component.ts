import { Component, OnInit } from '@angular/core';
import { FabricControlService } from '../../fabric-control.service';
import { FabricControlModel } from '../../models/fabricControl.model';
import { CutOrder } from '../../models/cut-order.model';
import { LoaderNumber } from '../../models/loader-number.model';
import { HttpErrorResponse } from '@angular/common/http';
import * as moment from 'moment';
import { ReturnRollcase } from '../../models/return-rollcase';
import { Jsonp } from '@angular/http/src/http';
import { SortDescriptor } from '@progress/kendo-data-query/dist/es/sort-descriptor';
import { GridDataResult } from '@progress/kendo-angular-grid';
import { orderBy } from '@progress/kendo-data-query/dist/es/array.operators';
import { ControlModel } from '../../models/fabric-control.model';
import { PhysicalAdjustmentsService } from '../../../../raw-material/inventory/physical-adjustments/physical-adjustments.service';
import { BinLocationsModel } from '../../../../raw-material/inventory/physical-adjustments/models/bin-locations.model';
import { SuccessService } from '../../../../shared/services/success.service';
import { AlertService } from '../../../../shared/services/alert.service';
import { ToastService } from '../../../../shared/services/toast.service';
import { ErrorService } from '../../../../shared/services/error.service';
import { LocalStorageService } from '../../../../shared/wrappers/local-storage.service';
import { ConfirmationService } from '../../../../shared/services/confirmation.service';

@Component({
  selector: 'app-return',
  templateUrl: './return.component.html',
  styleUrls: ['./return.component.css'],
  providers: [PhysicalAdjustmentsService]
})
export class ReturnComponent implements OnInit {
  cutOrderList: Array<CutOrder>;
  returnGridData: Array<ReturnRollcase>;
  login: any;
  selectedCutOrder: CutOrder;
  binLocations: Array<BinLocationsModel> = [];
  selectedBinLocationId: BinLocationsModel;
  show: boolean;
  totalSelectedRolls = 0;
  totalConsumedRolls = 0;
  totalAllocated = 0;
  totalPulled = 0;
  totalReturnRolls = 0;
  sort: SortDescriptor[] = [];
  gridView: GridDataResult = { data: [], total: 0 };
  cutOrders: Array<CutOrder>;
  binLocationList: Array<BinLocationsModel> = [];
  sewPlant: string;
  cutPlant: string;
  style: string;
  styleDesc: string;
  color: string;
  cutQuantity: number;
  fiscalWeek: string;
  cutDate: string;

  constructor(private _fabricControlService: FabricControlService,
    private _successService: SuccessService,
    private _confirmationService: ConfirmationService,
    private _localStorageService: LocalStorageService,
    private _errorService: ErrorService,
    private _physicalAdjustmentsService: PhysicalAdjustmentsService,
    private _toastService: ToastService,
    private _alertService: AlertService
  ) { }

  ngOnInit() {
    this.returnGridData = new Array<ReturnRollcase>();
    this.getCutOrders();
  }

  getCutOrders() {
    this.show = true;
    this._fabricControlService.getCutOrdersForReturn().subscribe(
      data => {
        this.cutOrderList = data;
        this.getBinLocations();
        this.cutOrders = this.cutOrderList;
        this.show = false;
      },
      (err: HttpErrorResponse) => {
        this.show = false;
        this._errorService.error(err);
      });
  }
  onCutOrderChange(cutOrder:CutOrder) {
    if(cutOrder)
    {
      this.sewPlant=cutOrder.SewPlant;
      this.color =cutOrder.Color;
      this.cutDate=cutOrder.CutDate;
      this.cutPlant = cutOrder.CutPlant;
      this.fiscalWeek = cutOrder.FiscalWeek;
      this.cutQuantity = cutOrder.CutQuantity;
      this.style = cutOrder.Style;
      this.styleDesc =cutOrder.StyleDesc;
    }
    if (cutOrder !== undefined) {
      this.show = true;
      this._fabricControlService.getReturnGridData(cutOrder.CutHbrId).subscribe(data => {
        if (data) {
          this.returnGridData = data;
          this.returnGridData.forEach(p => {
            p.BinLocation = this.binLocations.find(x => x.AreaCode === p.BinLocation);
            this.totalAllocated += Number(p.AllocationQty);
            this.totalPulled += Number(p.PulledQty);
            this.totalConsumedRolls += Number(p.ConsumedQty);
            this.binLocationList =p.BinLocation;
          });
          this.sortRollCases();
          this.loadRollCases();
        }
        this.show = false;
      },
        (err: HttpErrorResponse) => {
          this.show = false;
          this._errorService.error(err);
        });
    }
  }
  onCheckAll(event) {
    this.returnGridData.forEach(c => c.IsSelected = event.target.checked);
    this.setReturnInfo();
  }
  selectCheckAll() {
    return this.gridView.data.length > 0 && this.gridView.data.every(c => c.IsSelected === true);
  }

  getBinLocations() {
    this.show = true;
    const bins = this._localStorageService.get('returnBinLocations');
    if (!bins) {
      this._physicalAdjustmentsService.getBinLocation(2).subscribe(
        data => {
          this.show = false;
          this.binLocations = data;
          this._localStorageService.add('returnBinLocations', JSON.stringify(this.binLocations));
          this.binLocationList = this.binLocations;
        },
        (err: HttpErrorResponse) => {
          this.show = false;
          this._errorService.error(err);
        }
      );
    } else {
      this.binLocations = JSON.parse(bins);
      this.binLocationList = this.binLocations
    }
  }
  canEnableReturn() {
    return this.returnGridData.some(a => a.IsSelected === true);
  }

  confirmReturn() {
    const validateCount = this.returnGridData.filter(x => x.BinLocation === undefined).length;
    if (validateCount > 0) {
      this._toastService.error('Please select BinLocation for selected rolls.');
    } else {
      const count = this.returnGridData.filter(x => x.IsSelected === true).length;
      this._confirmationService.confirm({
        key: 'message',
        value: {
          message: 'Please confirm on returing of ' + count + ' selected rolls.',
          continueCallBackFunction: () => this.onReturn()
        }
      });
    }
  }
  onReturn() {
    this.show = true;
    const rollsToConsume: Array<ControlModel> = [];
    const selectedRolls = this.returnGridData.filter(x => x.IsSelected === true);

    selectedRolls.forEach(x => {
      const controlModel = new ControlModel();
      controlModel.AllocationQty = x.AllocationQty;
      controlModel.ItemCode = x.ItemCode;
      controlModel.FiberContent = x.FiberContent;
      controlModel.RMVendorName = x.RMVendorName;
      controlModel.RMVendorItemNumber = x.RMVendorItemNumber;
      controlModel.Width = x.Width;
      controlModel.BinLocation = x.BinLocation;
      controlModel.RollCaseId = x.RollCaseId;
      controlModel.CutHDRId = x.CutHDRId;
      controlModel.CutItemId = x.CutItemId;
      controlModel.AllocationItemId = x.AllocationItemId;
      controlModel.AllocationHDRId = x.AllocationHDRId;
      controlModel.AllocationRollId = x.AllocationRollId;
      controlModel.ConsumedQty = x.ConsumedQty;
      controlModel.RMVendorSiteId = x.RMVendorSiteId;
      controlModel.RMVendorId = x.RMVendorId;
      controlModel.CuttingVendorSiteId = x.CuttingVendorSiteId;
      controlModel.AllocationBatchId = x.AllocationBatchId;
      controlModel.LotNumber = x.LotNumber;
      controlModel.PulledQty = x.PulledQty;
      controlModel.ReturnedQty = x.ReturnedQty;
      controlModel.LotNumber = x.LoaderNumber;
      controlModel.BinLocation = x.BinLocation;
      controlModel.InventoryDTLId = x.InventoryDTLId;
      controlModel.InventoryHDRId = x.InventoryHDRId;
      controlModel.ActionType = 60;
      controlModel.ReturnedToInventoryAreaId = x.BinLocation.Id;
      rollsToConsume.push(controlModel);
    });
    this._fabricControlService.ReturnRolls(rollsToConsume).subscribe(
      data => {
        const isAllocated = data;
        if (isAllocated) {
          this.resetGrid();
          this.successMessage();
        } else {
          // this._alertService.alert({
          //   key: 'alertMessage',
          //   value: 'Error occured while returning rolls. Please retry.'
          // });
          this._toastService.error('Error occured while returning rolls. Please retry.');
        }
        this.show = false;
      },
      (err: HttpErrorResponse) => {
        this.show = false;
        this._errorService.error(err);
      }
    );
  }
  setReturnInfo() {
    this.login = JSON.parse(
      this._localStorageService.get('ags_erp_user_previlage'));

    this.totalReturnRolls = 0;

    this.returnGridData.filter(s => s.IsSelected === true)
      .forEach(s => {
        s.ReturnedBy = this.login.UserName,
          s.ReturnedDate = this._fabricControlService.getDateTime(),
          s.ReturnedQty = (s.PulledQty - s.ConsumedQty),
          this.totalReturnRolls += Number(s.ReturnedQty);
      });
    this.returnGridData.filter(s => s.IsSelected === false)
      .forEach(s => { s.ReturnedBy = '', s.ReturnedDate = '', s.ReturnedQty = 0; });

    this.totalSelectedRolls = this.returnGridData.filter(x => x.IsSelected === true).length;
  }
  canEnable() {
    return this.returnGridData.some(a => a.IsSelected === true);
  }
  sortRollCases() {
    this.returnGridData = this.returnGridData.sort((r1, r2) => {
      if (Number(r1.RollCaseId) > Number(r2.RollCaseId)) {
        return 1;
      } else if (Number(r1.RollCaseId) < Number(r2.RollCaseId)) {
        return -1;
      }
      return 0;
    });
  }
  resetGrid() {
    this.returnGridData = [];
    this.selectedCutOrder = null;
    this.gridView = { data: [], total: 0 };
    this.totalAllocated = this.totalConsumedRolls = this.totalPulled = this.totalSelectedRolls = 0;
  }
  sortChange(sort: SortDescriptor[]): void {
    this.sort = sort;
    this.loadRollCases();
  }
  loadRollCases() {
    this.gridView = {
      data: orderBy(this.returnGridData, this.sort),
      total: this.returnGridData.length
    };
  }
  handleFilter(value: string) {
    this.cutOrders = this.cutOrderList.filter((s) => (s.CutOrder).startsWith(value.toLocaleUpperCase()));
  }
  successMessage() {
    this._toastService.success('Selected rolls are returned successfully.');
  }

  handleBinLocationFilter(value:string){
    this.binLocationList = this.binLocations.filter((s) => (s.AreaCode).startsWith(value.toLocaleUpperCase()));
  }
}

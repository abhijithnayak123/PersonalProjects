import { Component, OnInit } from '@angular/core';
import { AllocateModel } from '../../models/allocate.model';
import { FabricControlService } from '../../fabric-control.service';
import { CutOrder } from '../../models/cut-order.model';
import { FabricControlModel } from '../../models/fabricControl.model';
import { Consumption } from '../../models/consumption.model';
import { LoaderNumber } from '../../models/loader-number.model';
import { retry } from 'rxjs/operator/retry';
import { HttpErrorResponse } from '@angular/common/http';
import * as moment from 'moment';
import { ConsumeRollCase } from '../../models/consumerollcases';
import { ControlModel } from '../../models/fabric-control.model';
import { SortDescriptor } from '@progress/kendo-data-query/dist/es/sort-descriptor';
import { GridDataResult } from '@progress/kendo-angular-grid';
import { orderBy } from '@progress/kendo-data-query/dist/es/array.operators';
import { SuccessService } from '../../../../shared/services/success.service';
import { ConfirmationService } from '../../../../shared/services/confirmation.service';
import { ErrorService } from '../../../../shared/services/error.service';
import { LocalStorageService } from '../../../../shared/wrappers/local-storage.service';
import { ToastService } from '../../../../shared/services/toast.service';
import { AlertService } from '../../../../shared/services/alert.service';

@Component({
  selector: 'app-consume',
  templateUrl: './consume.component.html',
  styleUrls: ['./consume.component.css']
})
export class ConsumeComponent implements OnInit {
  cutOrderList: Array<CutOrder>;
  loaderNumbers: Array<LoaderNumber>;
  consumeGridData: Array<ConsumeRollCase>;
  availableRolls: number;
  login: any;
  totalSelectedRolls = 0;
  totalConsumedRolls = 0;
  selectedCutOrder: CutOrder;
  totalAllocated = 0;
  totalPulled = 0;
  show: boolean;
  sort: SortDescriptor[] = [];
  gridView: GridDataResult = { data: [], total: 0 };
  cutOrders: Array<CutOrder>;
  sewPlant: string;
  cutPlant: string;
  style: string;
  styleDesc: string;
  color: string;
  cutQuantity: number;
  fiscalWeek: string;
  cutDate: string;

  constructor(
    private _fabricControlService: FabricControlService,
    private _successService: SuccessService,
    private _confirmationService: ConfirmationService,
    private _errorService: ErrorService,
    private _localStorageService: LocalStorageService,
    private _toastService: ToastService,
    private _alertService: AlertService
  ) { }

  ngOnInit() {
    this.getCutOrders();
    this.consumeGridData = new Array<ConsumeRollCase>();
  }
  getCutOrders() {
    this.show = true;
    this._fabricControlService.getCutOrdersForConsume().subscribe(
      data => {
        this.cutOrderList = data;
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
      this._fabricControlService.getConsumeData(cutOrder.CutHbrId).subscribe(data => {
        this.consumeGridData = data;
        this.consumeGridData.forEach(p => {
          p.ConsumedBy = '',
            p.ConsumedDate = '',
            this.totalAllocated += p.AllocationQty,
            this.totalPulled += p.PulledQty;            
        });
        this.sortRollCases();
        this.loadRollCases();
        this.show = false;
      },
        (err: HttpErrorResponse) => {
          this.show = false;
          this._errorService.error(err);
        });     
    }
    this.loaderNumbers = this._fabricControlService.getLoaderNumbers();
  }
  onCheckAll(event) {
    this.totalConsumedRolls = 0;
    this.consumeGridData.forEach(c => c.IsSelected = event.target.checked);
    this.setConsumeInfo();
    this.getTotalConsumedYds();
  }
  canEnableConsume() {
    return this.consumeGridData.some(a => a.IsSelected === true);
  }
  onConsume() {
    if (this.consumeGridData.some(x => x.ConsumedQty > x.AllocationQty)) {
      this._toastService.error('Consumed cannot be more than Allocated for selected rolls');
      return;
    }
    const consumedRolls = this.consumeGridData.filter(c => c.IsSelected === true);
    if (consumedRolls.length > 0) {
      this._confirmationService.confirm({
        key: 'message',
        value: {
          message: 'Please confirm to consume the selected rolls.',
          continueCallBackFunction: () => this.consume()
        }
      });

    }
  }
  consume() {
    this.show = true;
    const rollsToConsume: Array<ControlModel> = [];
    const selectedRolls = this.consumeGridData.filter(x => x.IsSelected === true);

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
      controlModel.ConsumedQty = x.ConsumedQty;
      controlModel.LotNumber = x.LotNumber;
      controlModel.PulledQty = x.PulledQty;
      controlModel.ConsumedBy = x.ConsumedBy;
      controlModel.ConsumedDate = x.ConsumedDate;
      controlModel.ActionType = 50;
      controlModel.AllocationItemId = x.AllocationItemId;
      controlModel.AllocationRollId = x.AllocationRollId;
      controlModel.Pulled = true;
      controlModel.InventoryHDRId = x.InventoryHDRId;
      controlModel.InventoryDTLId = x.InventoryDTLId;
      controlModel.CutItemId = x.CutItemId;
      rollsToConsume.push(controlModel);
    });
    this._fabricControlService.ConsumeCutOrders(rollsToConsume).subscribe(
      data => {
        const isAllocated = data;
        if (isAllocated) {
          this.resetGrid();
          this.successMessage();
        } else {
          // this._alertService.alert({
          //   key: 'alertMessage',
          //   value: 'Error occured while consuming rolls. Please retry.'
          // });
          this._toastService.error('Error occured while consuming rolls. Please retry.');
        }
        this.show = false;
      },
      (err: HttpErrorResponse) => {
        this.show = false;
        this._errorService.error(err);
      }
    );
  }
  setConsumeInfo() {
    this.login = JSON.parse(
      this._localStorageService.get('ags_erp_user_previlage'));
    this.totalSelectedRolls = this.consumeGridData.filter(x => x.IsSelected === true).length;

    this.consumeGridData.filter(x => x.IsSelected === true)
      .forEach(s => { s.ConsumedBy = this.login.UserName, s.ConsumedDate = this.getDate(); s.ConsumedQty = s.PulledQty });

    this.consumeGridData.filter(x => x.IsSelected === false)
      .forEach(s => { s.ConsumedBy = ' ', s.ConsumedDate = '', s.ConsumedQty = 0; });
  }

  updateRowInfo(dataItem: ConsumeRollCase) {
    dataItem.ConsumedQty = dataItem.PulledQty;
    this.setConsumeInfo();
  }

  getDate() {
    return moment(new Date()).format('MM/DD/YYYY');
  }
  validateConsumedQty(dataItem: ConsumeRollCase) {
    this.totalConsumedRolls = 0;
    if (Number(dataItem.ConsumedQty) > Number(dataItem.AllocationQty)) {
      this._toastService.error('Consumed cannot be more than Allocated');
    } else {
      this.consumeGridData.filter(x => x.IsSelected).forEach(a =>
        this.totalConsumedRolls += Number(a.ConsumedQty)
      );
    }
  }

  successMessage() {
    this._toastService.success('Selected rolls are consumed successfully.');
  }

  resetGrid() {
    this.consumeGridData = [];
    this.gridView = { data: [], total: 0 };
    this.selectedCutOrder = null;
    this.totalAllocated = this.totalConsumedRolls = this.totalPulled = this.totalSelectedRolls = 0;
  }
  sortChange(sort: SortDescriptor[]): void {
    this.sort = sort;
    this.loadRollCases();
  }
  loadRollCases() {
    this.gridView = {
      data: orderBy(this.consumeGridData, this.sort),
      total: this.consumeGridData.length
    };
  }
  sortRollCases() {
    this.consumeGridData = this.consumeGridData.sort((r1, r2) => {
      if (Number(r1.RollCaseId) > Number(r2.RollCaseId)) {
        return 1;
      } else if (Number(r1.RollCaseId) < Number(r2.RollCaseId)) {
        return -1;
      }
      return 0;
    });
  }
  handleFilter(value: string) {
    this.cutOrders = this.cutOrderList.filter((s) => (s.CutOrder).startsWith(value.toLocaleUpperCase()));
  }
  getTotalConsumedYds() {
    this.totalConsumedRolls = 0;
    if (this.gridView.data.length > 0) {
      this.gridView.data.forEach(c => {
        this.totalConsumedRolls += Number(c.ConsumedQty);
      });
    }
    return this.totalConsumedRolls;
  }
  selectCheckAll() {
    this.totalConsumedRolls = 0;
    this.getTotalConsumedYds();
    return this.gridView.data.length > 0 && this.gridView.data.every(c => c.IsSelected === true);
  }

}

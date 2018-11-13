import { Component, OnInit } from '@angular/core';
import { AllocateModel } from "../../models/allocate.model";
import { FabricControlService } from "../../fabric-control.service";
import { CutOrder } from '../../models/cut-order.model';
import { FabricControlModel } from '../../models/fabricControl.model';
import { LoaderNumber } from '../../models/loader-number.model';
import { HttpErrorResponse } from '@angular/common/http';
import { ControlModel } from '../../models/fabric-control.model';
import { DateInputComponent } from '@progress/kendo-angular-dateinputs/dist/es/dateinput/dateinput.component';
import { DatePipe } from '@angular/common/src/pipes/date_pipe';
import { parseDate } from '@telerik/kendo-intl/dist/es/main';
import { SortDescriptor } from '@progress/kendo-data-query/dist/es/sort-descriptor';
import { GridDataResult } from '@progress/kendo-angular-grid';
import { orderBy } from '@progress/kendo-data-query/dist/es/array.operators';
import { SuccessService } from '../../../../shared/services/success.service';
import { ConfirmationService } from '../../../../shared/services/confirmation.service';
import { LocalStorageService } from '../../../../shared/wrappers/local-storage.service';
import { ErrorService } from '../../../../shared/services/error.service';
import { ToastService } from '../../../../shared/services/toast.service';

@Component({
  selector: 'app-pull',
  templateUrl: './pull.component.html',
  styleUrls: ['./pull.component.css']
})
export class PullComponent implements OnInit {

  loaderNumbers: Array<LoaderNumber>;
  cutOrderList: Array<CutOrder> = [];
  fabricControlData: FabricControlModel;
  login: any;
  selectedCutOrder: CutOrder;
  pullRolls: Array<ControlModel> = [];
  show: boolean = false;
  sewPlant: string;
  cutPlant: string;
  style: string;
  styleDescription: string;
  color: string;
  actualCutQty: number;
  fiscalWeek: string;
  cutDate: string;
  selectedLoadNumber: LoaderNumber;
  totalAllocated:number=0;
  checkAll:boolean=false;
  public sort: SortDescriptor[] = [];
  public gridView: GridDataResult = {data: [],total : 0}
  cutOrders: Array<CutOrder> = [];
  comments:string;
  totalPullRolls:number=0;


  constructor(
    private _fabricControlService: FabricControlService,
    private _successService: SuccessService,
    private _confirmationService: ConfirmationService,
    private _localStorageService: LocalStorageService,
    private _errorService: ErrorService,
    private _toastService: ToastService) { }

  ngOnInit() {
    this.getCutOrders();
    this.login = JSON.parse(
      this._localStorageService.get('ags_erp_user_previlage'));
  }
  getCutOrders() {    
    this.show = true;
    this._fabricControlService.getCutOrdersForPull().subscribe(
      data => {
        this.cutOrderList = data;     
        this.cutOrders = this.cutOrderList;  
        this.show = false; 
      },
      (err: HttpErrorResponse) => {
        this._errorService.error(err);
        this.show = false;
      });
  }

  onCutOrderChange(cutOrder: CutOrder) {
    if (cutOrder !== undefined) {
      this.show = true;
      this._fabricControlService.gePullHeaderData(cutOrder.CutHbrId).subscribe(
        data => {
          this.sewPlant = cutOrder.SewPlant,
            this.cutPlant = cutOrder.CutPlant,
            this.style = cutOrder.Style,
            this.color = cutOrder.Color,
            this.styleDescription = cutOrder.StyleDesc,
            this.actualCutQty = cutOrder.CutQuantity,
            this.fiscalWeek = cutOrder.FiscalWeek,
            this.cutDate = cutOrder.CutDate,
            this.selectedLoadNumber = new LoaderNumber(0, 0),
            this.pullRolls = data,
            this.pullRolls.forEach(p=>{p.PulledQty = 0})
            this.pullRolls = this.sortByRollCaseId(this.pullRolls),
            this.gridView.data = this.pullRolls,
            this.totalPullRolls = this.gridView.data.length
        },
        (err: HttpErrorResponse) => {
          this.show = false;
          this._errorService.error(err);
        });
    }
    this.getLoadNumber();
    this.show = false;
  }
  onCheckAll(event) {
    this.gridView.data.forEach(c => c.IsSelected = event.target.checked);
    this.setPulledInfo()
  }
  getTotalPulledYards() {
    let totalSelectedYards = 0;
    this.gridView.data.filter(c => c.IsSelected === true).forEach(c => {
      totalSelectedYards += c.AllocationQty
    });
    return totalSelectedYards;
  }

  canEnablePull() {
    return this.gridView.data.some(a => a.IsSelected === true);
  }

  confirmPull() {
    if (this.gridView.data.some(c => c.IsSelected === true)) {
      this._confirmationService.confirm({        
        key: 'message',
        value: { 
          message: 'Please confirm to pull ' + this.gridView.data.filter(p => p.IsSelected === true).length + ' selected roll(s).', 
          continueCallBackFunction: () => this.onPull() 
        }
      });
    }
  }
  onPull() {
    let pulledRolls = this.gridView.data.filter(p=>p.IsSelected===true);
    if(pulledRolls.length > 0)
    {
      pulledRolls.forEach(s=>{s.ActionType = 40,s.Pulled=true});
      this._fabricControlService.onPull(pulledRolls).subscribe(
        data=>{
          if(data)
          {
            this.gridView.data = [];
            this.sewPlant = null;
            this.cutPlant = null;
            this.style = null;
            this.styleDescription = null;
            this.color = null;
            this.actualCutQty = null;
            this.fiscalWeek = null;
            this.cutDate = null;
            this.comments = null;
            this.selectedCutOrder = null;     
            this.totalAllocated =null;
            this.getSelectedRolls();
            this.getTotalPulledYards();
            this.totalPullRolls = 0;
            this._toastService.success('Rolls are Pulled Successfully');
            this.onRefresh();
          }
        },
        (err :HttpErrorResponse)=>{
          this._toastService.error("Sorry, Couldn't Pull rolls. Something went wrong")
        }
      )
    }
  }
  setPulledInfo() {
    let pullDate = this._fabricControlService.getDateTime();
    this.pullRolls.filter(s => s.IsSelected === true)
      .forEach(s => { s.PulledBy = this.login.UserName, s.PullTimeStamp = pullDate.toString() });
    this.pullRolls.filter(s => s.IsSelected === false)
      .forEach(s => { s.PulledBy = "", s.PullTimeStamp = "" });
    this.pullRolls.filter(p => p.IsSelected === true)
      .forEach(s => { s.PulledQty = s.AllocationQty });
    this.pullRolls.filter(p => p.IsSelected === false)
      .forEach(s => { s.PulledQty = 0 });
  }

  getSelectedRolls(){
    return this.gridView.data.filter(s=>s.IsSelected===true).length;
  }

  selectCheckAll(){
    return this.gridView.data.length > 0 && this.gridView.data.every(c => c.IsSelected === true);
  }

  getLoadNumber() {
    this.loaderNumbers = [new LoaderNumber(0, 0), new LoaderNumber(1, 1), new LoaderNumber(2, 2), new LoaderNumber(3, 3), new LoaderNumber(4, 4),
    new LoaderNumber(5, 5), new LoaderNumber(6, 6), new LoaderNumber(7, 7), new LoaderNumber(8, 8), new LoaderNumber(9, 9), new LoaderNumber(10, 10)];
  }

  onRefresh() {
    this.checkAll=false;
  }
getTotalAllocated(){
  this.totalAllocated =0;  
  this.gridView.data.forEach(p=>{
    this.totalAllocated += p.AllocationQty;
  })
  return this.totalAllocated;
}
handleFilter(value:string) {
  this.cutOrders = this.cutOrderList.filter((s) => (s.CutOrder).startsWith(value.toLocaleUpperCase()));
}

sortChange(sort: SortDescriptor[]): void {
  this.sort = sort;
  this.sortPullRolls();
}
sortPullRolls() {
  this.gridView = {
    data: orderBy(this.pullRolls, this.sort),
    total: this.pullRolls.length
  };
}

sortByRollCaseId(pullRolls :Array<ControlModel>){
  pullRolls = pullRolls.sort((r1, r2) => {
    if (r1.RollCaseId > r2.RollCaseId) {
      return 1;
    }
    else if (r1.RollCaseId < r2.RollCaseId) {
      return -1;
    }
    return 0;
  });
  return pullRolls;
}
}

import { Injectable } from '@angular/core';
import { AllocateModel } from "./models/allocate.model";
import { FabricControlModel } from "./models/fabricControl.model";
import { CutOrder } from './models/cut-order.model';
import { LoaderNumber } from './models/loader-number.model';
import { Consumption } from './models/consumption.model';
import * as moment from 'moment';
import { retry } from 'rxjs/operator/retry';
import { AllocateFilterRollCase } from "./models/allocateFilterRollCase.model";
import { AllocateResult } from "./models/allocateResult.model";
import { ControlModel } from './models/fabric-control.model';
import { ConsumeRollCase } from './models/consumerollcases';
import { ReturnRollcase } from './models/return-rollcase';
import { HttpService } from '../../shared/wrappers/http.service';
import { Allocation } from './models/allocation.model';

@Injectable()
export class FabricControlService {

  gridData: Array<AllocateModel>;
  public date;
  constructor(private _httpService: HttpService) { }

  getCutUnAllocatedOrders() {
    return this._httpService.get<CutOrder[]>('fabriccontrol/cutorders/unallocate')
      .map(response => {
        return response;
      });
  }

  getFilterData(cutHbrId: number) {
    return this._httpService.get<AllocateFilterRollCase[]>('fabriccontrol/filterdetails/' + cutHbrId)
      .map(response => {
        return response;
      });
  }

  getAllocatedResultData(rollcase: AllocateFilterRollCase) {
    let lotQueryString = '';
    if (rollcase.SelectedLots && rollcase.SelectedLots.length > 0) {
      rollcase.SelectedLots.forEach(c => {
        lotQueryString += 'lotnumber=' + c + '&';
      });
      return this._httpService.
      get<ControlModel[]>('fabriccontrol/result/' + rollcase.CutHbrId + '/' + rollcase.VendorSiteId + '/'+ rollcase.AramarkItem + '/?' + lotQueryString);
    }
  }

  AllocateTheCutOrders(rollsToAllocate: Array<ControlModel>) {
    return this._httpService.post('fabriccontrol/allocate', rollsToAllocate)
      .map(response => {
        return response;
      });
  }

  DeAllocateTheCutOrders(rollsToDeAllocate: Array<ControlModel>) {
    return this._httpService.post('fabriccontrol/deallocate', rollsToDeAllocate)
      .map(response => {
        return response;
      });
  }

  getAllocatedCutOrders() {
    return this._httpService.get<CutOrder[]>('fabriccontrol/cutorders/allocate')
      .map(response => {
        return response;
      });
  }

  getAllocatedRollsForCurrentGrid(cutHdrId: number) {
    // return this._httpService.get<AllocateResult[]>('fabriccontrol/deallocate/'+cutHdrId)
    // .map(response =>{
    //   return response;
    // })
    return this._httpService.get<ControlModel[]>('fabriccontrol/deallocate/' + cutHdrId)
      .map(response => {
        return response;
      })
  }
  getCutOrdersForDeAllocate() {
    let cutOrders: Array<CutOrder>;
    // cutOrders = [new CutOrder(1, "DA10730"), new CutOrder(2, "DA10731"), new CutOrder(3, "DA10732")]
    return cutOrders;
  }
  getCutOrdersForPull() {
    let cutOrders = this._httpService.get<CutOrder[]>('fabriccontrol/cutorders/pull');
    return cutOrders;
  }
  getDeAllocateHeaderData(cutOrder) {
    let a = this._httpService.get<FabricControlModel>('fabriccontrol/fabriccontroldetails/cutOrder/DeAllocate');
    return a;
  }
  gePullHeaderData(cutOrder) {
    let a = this._httpService.get<ControlModel[]>('fabriccontrol/pull/' + cutOrder);
    return a;
  }
  getConsumeData(cutHdrId) {
    const data = this._httpService.get<ConsumeRollCase[]>('fabriccontrol/consume/' + cutHdrId);
    return data;
  }

  getConsumptionTypes() {
    let consumptionTypes: Array<Consumption>;
    consumptionTypes = [new Consumption(1, "Complete"), new Consumption(2, "Partial"), new Consumption(3, "Over"),
    new Consumption(4, "Not Used")];
    return consumptionTypes;
  }
  getReturnGridData(cutHdrId) {
    const returnData = this._httpService.get<ReturnRollcase[]>('fabriccontrol/return/' + cutHdrId);
    return returnData;
  }

  getAllocationTypes() {
    let allocationTypes: Array<Allocation>;
    allocationTypes = [new Allocation(1, "Complete"), new Allocation(2, "Partial")];
    return allocationTypes;
  }
  getDateTime() {
    this.date = moment(new Date()).format("MM/DD/YYYY hh:MM:SS A");
    return this.date;
  }
  getDate(date: string) {
    this.date = moment(date).format("DD-MM-YYYY");
    return this.date;
  }

  onAllocate(fabricControlData: FabricControlModel) {
    let status = this._httpService.post<FabricControlModel>('fabriccontrol/fabriccontroldetails/allocate', fabricControlData);
    return status;
  }

  getLoaderNumbers() {
    let loaderNumbers: Array<LoaderNumber>;
    loaderNumbers = [new LoaderNumber(0, 0), new LoaderNumber(1, 1), new LoaderNumber(2, 2), new LoaderNumber(3, 3),
    new LoaderNumber(4, 4), new LoaderNumber(5, 5), new LoaderNumber(6, 6), new LoaderNumber(7, 7),
    new LoaderNumber(8, 8), new LoaderNumber(9, 9), new LoaderNumber(10, 10)];
    return loaderNumbers;
  }

  onDeAllocate(fabricControlData: FabricControlModel) {
    let status = this._httpService.post<FabricControlModel>('fabriccontrol/fabriccontroldetails/deallocate', fabricControlData);
    return status;
  }

  onPull(pullRolls: Array<ControlModel>) {
    let status = this._httpService.post<ControlModel[]>('fabriccontrol/pull', pullRolls);
    return status;
  }

  getCutOrdersForReturn() {
    const cutOrders = this._httpService.get<CutOrder[]>('fabriccontrol/cutorders/return');
    return cutOrders;
  }

  getCutOrdersForConsume() {
    const cutOrders = this._httpService.get<CutOrder[]>('fabriccontrol/cutorders/consume');
    return cutOrders;
  }

  ConsumeCutOrders(rollsToConsume: Array<ControlModel>) {
    return this._httpService.post('fabriccontrol/consume', rollsToConsume)
      .map(response => {
        return response;
      });
  }
  ReturnRolls(rolls: Array<ControlModel>) {
    return this._httpService.post('fabriccontrol/returnrollcases', rolls)
      .map(response => {
        return response;
      });
  }
 
}

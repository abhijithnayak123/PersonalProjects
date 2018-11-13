import { Injectable } from '@angular/core';
import { HttpService } from '../../shared/wrappers/http.service';
import { Vendor } from './models/vendor';
import { FabricWatchList } from './models/fabric-watch-list';
import { FutureWatchList } from './models/future-watch-list';
import { PlanningReport } from './models/planning-report';
import { AramarkItem } from './models/aramark-item';
import { FabricOrder } from "./models/fabric-order";

@Injectable()
export class FabricPlanningService {
  constructor(private _httpService: HttpService) { }

  /** Get All Vendors */
  getVendors(watchListType: string) {
    const vendors = this._httpService.get<Vendor[]>('watchlist/vendors/' + watchListType)
      .map(response => {
        return response;
      });
    return vendors;
  }

  /** Get Fabric Watch List data Based on Vendor Id */
  getFabricWatchListData(vednorId: number) {
    const watchList = this._httpService.get<FabricWatchList[]>('watchlist/fabric/' + vednorId)
      .map(response => {
        return response;
      });
    return watchList;
  }

  /** Get Future Watch List data Based on Vendor Id */
  getFutureWatchlistData(vendorId: number) {
    const watchList = this._httpService.get<FutureWatchList[]>('watchlist/future/' + vendorId)
      .map(response => {
        return response;
      });
    return watchList;
  }
  getFabricPlanningReportData(vendorId: number, itemId: number) {
    const watchList = this._httpService.get<PlanningReport[]>('watchlist/FabricPlanningReport/' + vendorId + '/' + itemId)
      .map(response => {
        return response;
      });
    return watchList;
  }
  getAramarkItems() {
    const items = this._httpService.get<AramarkItem[]>('watchlist/AramarkItem')
    .map(response => {
      return response;
    });
  return items;
  }

  getFabricOrder(vendorId : number){
    return this._httpService.get<FabricOrder[]>('watchlist/fabriccutOrder/'+vendorId)
    .map(response => {
      return response;
    });
  }
  placeOrder(orders:Array<FabricOrder>){
    return this._httpService.post('watchlist/placeCutOrder',orders)
    .map(response =>{
      return response;
    })
  }
}


import { Injectable } from '@angular/core';
import { HttpService } from '../../shared/wrappers/http.service';
import { Vendor } from '../purchasing/models/vendor';
import { VendorDetails } from '../inventory/physical-adjustments/models/vendor-details.model';
import { PickupLocation } from '../inventory/container-po/models/pickup-location';
import { PONumber } from './models/poNumber';
import { POItemNumber } from './models/POItemNumber';
import { POStatus } from './models/POStatus';
import { POFilterData } from './models/POFilterData';
import { POMaintenance } from './models/POMaintenance';
import { MaintenanceHeader } from './models/MaintenanceHeader';
import { MaintenanceGrid } from './models/MaintenanceGrid';
import { POSearchHeader } from './models/POMaintenanceHeader';
import { FabricOrder1 } from "./models/fabric-order1";


@Injectable()
export class PurchasingService {
    constructor(private _httpService: HttpService) { }

    getPOStatus(){
     return this._httpService.get<POStatus[]>('potracking/status')
      .map(response =>{
        return response;
      });
    }
    
  
    getMaintenanceData(selectedPOHdrId){
      return this._httpService.get<POMaintenance[]>('potracking/maintenancedata/'+selectedPOHdrId)
      .map(response =>{
        return response;
      });
    }
    getMaintenanceHeaderData(selectedPOHdrId){
      return this._httpService.get<MaintenanceHeader[]>('potracking/maintenance/header/'+selectedPOHdrId)
      .map(response =>{
        return response;
      });
    }
    getMaintenanceGridData(selectedPOHdrId){
      return this._httpService.get<MaintenanceGrid[]>('potracking/maintenance/grid/'+selectedPOHdrId)
      .map(response =>{
        return response;
      });
    }

    getFabricOrder(vendorId : number){
    return this._httpService.get<FabricOrder1[]>('watchlist/fabriccutOrder/'+vendorId)
    .map(response => {
      return response;
    });
  }
  placeOrder(orders:Array<FabricOrder1>){
    return this._httpService.post('watchlist/placeCutOrder',orders)
    .map(response =>{
      return response;
    })
  }
  /** Get All Vendors */
  getVendors(watchListType: string) {
    const vendors = this._httpService.get<Vendor[]>('watchlist/vendors/' + watchListType)
      .map(response => {
        return response;
      });
    return vendors;
  }
    
 Save(pomaintenance) {
    return this._httpService.post('potracking/maintenance/save',pomaintenance)
      .map(response => {
        return response;
      });
  }
  Cancel(selectedPOHdrId){
    return this._httpService.get<boolean>('potracking/maintenance/cancel/'+selectedPOHdrId)
    .map(response =>{
      return response;
    })
  }
  Delete(pomaintenance) {
    return this._httpService.post('potracking/maintenance/delete',pomaintenance)
      .map(response => {
        return response;
      });
  }
  getPOHeaderData(){
    return this._httpService.get<POSearchHeader[]>('potracking/search/header')
  }
}

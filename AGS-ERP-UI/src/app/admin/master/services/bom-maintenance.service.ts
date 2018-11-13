import { Injectable } from '@angular/core';
import { HttpService } from "../../../shared/wrappers/http.service";
import { BomMaintenanceSearchDB } from "../models/BomMaintenanceSearchDB";
import { BomFilteredGridData } from "../models/BomFilteredData";
import { BomMaintenanceStyle } from "../models/BomMaintenanceStyle";
import { BomMaintenanceColor } from "../models/BomMaintenanceColor";
import { BomMaintenanceMfgVendor } from "../models/BomMaintenanceMfgVendor";
import { BomMaintenanceSpec } from "../models/BomMaintenanceSpec";
import { BomMaintenanceHeader } from '../models/BomMaintenanceHeader';
import { BomDetail } from "../models/BomDetail";

@Injectable()
export class BomMaintenanceService {

  constructor(
    private _httpService: HttpService
  ) { }

  getSearchData(){
    return this._httpService.get<BomMaintenanceSearchDB[]>('bom-management/bom-item-search')
    .map(response => {
        return response;
    });
  }

  getFilteredGridData(){
    return this._httpService.get<BomFilteredGridData[]>('bom-management/items')
    .map(response => {
        return response;
    });
  }

  getStyles(isInCreateState : boolean){
      return this._httpService.get<BomMaintenanceStyle[]>('bom-management/bom-styles/'+isInCreateState)
      .map(response =>{
       return response;
      })
  }

  getColorsByStyleId(styleId : number,isInCreateState : boolean){
       return this._httpService.get<BomMaintenanceColor[]>('bom-management/bom-colors/'+styleId+"/"+isInCreateState)
      .map(response => {
        return response;
      })
  }

   getMfgVendorsByColorId(styleId: number, colorId : number,isInCreateState : boolean){
      return this._httpService.get<BomMaintenanceMfgVendor[]>('bom-management/bom-vendors/'+styleId+'/'+colorId+"/"+isInCreateState)
      .map(response => {
        return response;
      })
  }

  getSpecList(selectedStyle: BomMaintenanceStyle,selectedColor : BomMaintenanceColor){
    return this._httpService.get<BomMaintenanceSpec[]>('bom-management/bom-specifications/'+selectedStyle.Id+'/'+selectedColor.Id)
    .map(response =>{
      return response;
    })
  }

  getBomHeaderData(selectedStyle: BomMaintenanceStyle,selectedColor : BomMaintenanceColor,selectedVendor : BomMaintenanceMfgVendor){
    return this._httpService.get<BomMaintenanceHeader>('bom-management/bom-header/' + selectedStyle.Id + '/' + selectedColor.Id + '/' + selectedVendor.Id)
    .map(response =>{
      return response;
    })
  }

  getBomComponentGridData(bomHdrId : number){
    return this._httpService.get<BomFilteredGridData[]>('bom-management/bom-components/'+bomHdrId)
    .map(response => {
      return response;
    })
  }

  saveBOM(bomDetail : BomDetail){
    return this._httpService.post('bom-management/save', bomDetail)
      .map(response =>{
      console.log(response);
      return response;
    })
  }

  deleteBOM(bomDetail : BomMaintenanceHeader){
    return this._httpService.post('bom-management/delete-bom', bomDetail)
    .map(response => {return response});
  }
  
}

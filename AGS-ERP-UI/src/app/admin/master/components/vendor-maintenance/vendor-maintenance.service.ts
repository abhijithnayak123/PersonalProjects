import { Injectable } from "@angular/core";
import { HttpService } from "../../../../shared/wrappers/http.service";
import { Vendor } from  '../../models/Vendor';
import { VendorModel } from  '../../models/VendorModel';
import { VendorStatus } from  '../../models/VendorStatus';
import { Country } from "../../models/Country";
import { SupplierSiteDetails } from "../../models/SupplierSiteDetails";
import { VendorContact } from "../../models/VendorContact";
import {Site} from "../../models/Site";
import {Type} from "../../models/Type";
import { Inco } from "../../models/Inco";
import { Port } from "../../models/Port";
import { PoCommunication } from "../../models/PoCommunication";
import { DeliveryEvent } from "../../models/DeliveryEvent";
import {Asn} from "../../models/Asn";

@Injectable()
export class VendorMaintenanceService {
    constructor(
        private _httpService: HttpService
    ){}

  
    getVendors(){
        return this._httpService.get<Vendor[]>('vendor-management/suppliers')
        .map(response => {
          return response;
        });
     }

     getVendorDetailsById(VendorId: number){
        return this._httpService.get<VendorModel>('vendor-management/supplier-details/'+VendorId)
        .map(response => {
          return response;
        });
     }

     getVendorStatus() {
        return this._httpService.get<VendorStatus[]>('vendor-management/status')
          .map(response => {
            return response;
          });
    }

   
    getCountries(){
        return this._httpService.get<Country[]>('vendor-management/countries')
        .map(response => {
          return response;
        });
     }

    getVendorSites(VendorId: number){
        return this._httpService.get<SupplierSiteDetails[]>('vendor-management/site-details/'+VendorId)
        .map(response => {
          return response;
        });
    }

    getVendorContracts(VendorId: number){
        return this._httpService.get<VendorContact[]>('vendor-management/contact-details/'+VendorId)
        .map(response => {
          return response;
        });
    }
     
    getContractSites(VendorId: number){
        return this._httpService.get<Site[]>('vendor-management/vendor-sites/'+VendorId)
        .map(response => {
          return response;
        });
    }

    getSiteTypes(){
      return this._httpService.get<Type[]>('vendor-management/supplier-site-types')
      .map(response => {
        return response;
      });
  }

  getIncos(){
    return this._httpService.get<Inco[]>('vendor-management/inco')
    .map(response => {
      return response;
    });
  }

  getPorts(){
    return this._httpService.get<Port[]>('vendor-management/port')
    .map(response => {
      return response;
    });
  }

  getAsn(){
    return this._httpService.get<Asn[]>('vendor-management/asn')
    .map(response => {
      return response;
    });
  }

  getCommunications(){
    return this._httpService.get<PoCommunication[]>('vendor-management/po-communication')
    .map(response => {
      return response;
    });
  }

  getDeliveryEvent(){
    return this._httpService.get<DeliveryEvent[]>('vendor-management/delivery-event')
    .map(response => {
      return response;
    });
  }

  saveSupplier(model: VendorModel){
    return this._httpService.post('vendor-management/save-supplier', model)
        .map(response => {
          return response;
    });
  }

  deleteSupplier(model: VendorModel){
    return this._httpService.post('vendor-management/delete-supplier', model)
        .map(response => {
          return response;
    });
  }

  saveContact(contacts: Array<VendorContact>){
    return this._httpService.post('vendor-management/save-vendor-contact', contacts)
        .map(response => {
          return response;
    });
  }
  
 
  deleteContact(contacts: Array<VendorContact>){
    return this._httpService.post('vendor-management/delete-vendor-contact', contacts)
        .map(response => {
          return response;
    });
  }

  saveVendorSite(site: SupplierSiteDetails){
    return this._httpService.post('vendor-management/save-supplier-site', site)
        .map(response => {
          return response;
    });
  }
  
 
  deleteVendorSite(site: SupplierSiteDetails){
    return this._httpService.post('vendor-management/delete-supplier-site', site)
        .map(response => {
          return response;
    });
  }

}
import { Component, OnInit, Input } from '@angular/core';
import { RowClassArgs, GridComponent, GridDataResult } from '@progress/kendo-angular-grid';
import { Align } from '@progress/kendo-angular-popup';
import { VendorMaintenanceService } from './vendor-maintenance.service';
import { HttpErrorResponse } from "@angular/common/http";
import { ErrorService } from "../../../../shared/services/error.service";
import { SuccessService } from '../../../../shared/services/success.service';
import { ConfirmationService } from '../../../../shared/services/confirmation.service';
import { ToastService } from '../../../../shared/services/toast.service';
import { SortDescriptor,orderBy} from "@progress/kendo-data-query";

import { Vendor} from '../../models/Vendor';
import { VendorModel } from '../../models/VendorModel';
import { Country } from '../../models/Country';
import { SupplierSiteDetails } from '../../models/SupplierSiteDetails';
import { VendorStatus } from '../../models/VendorStatus';
import { Type } from '../../models/Type';


@Component({
  selector: 'app-vendor-maintenance',
  templateUrl: './vendor-maintenance.component.html',
  styleUrls: ['./vendor-maintenance.component.css'],
  providers: [VendorMaintenanceService]
})
export class VendorMaintenanceComponent implements OnInit {
  toggleClassshowToggle: string = "";
  toggleClassshowSupplier: string = "";
  toggleClassshowVendor: string = "";
  anchorAlign: Align = { horizontal: "right", vertical: "bottom" };
  popupAlign: Align = { horizontal: "right", vertical: "top" };
  showWhereUsed: boolean = false;
  showSupplier: boolean = false;
  showVendorSite: boolean = false;
  showPopuplayer: boolean = false;
  public opened: boolean = false;

  isErrors : boolean = false;

  SupplierSiteDetails: any[];
  model: VendorModel;
  site: SupplierSiteDetails;
  record: SupplierSiteDetails;
  VendorList: Array<Vendor>=[];
  CountryList: Array<Country>=[];
  TypesList: Array<Type>=[];
  StatusList: Array<VendorStatus>=[];
  SiteList: Array<SupplierSiteDetails>=[];
  show: boolean = false;
  isVendorSelected: boolean = false;
  vendorId: number;
  siteItem: SupplierSiteDetails;

  gridDataList: GridDataResult = {data:[],total : 0}
  filterSort: SortDescriptor[] = [];
  VendorSiteDetails : Array<SupplierSiteDetails>=[];

  constructor(private _vendorMaintenance:VendorMaintenanceService,
              private _successService: SuccessService,
              private _confirmationService: ConfirmationService,
              private _toastService: ToastService,
              private _errorService: ErrorService){ }

  ngOnInit() {
    this.show = true;
    this.model = new VendorModel();
    this.model.isAddMode = false;
    this.model.isEditMode = false;
    this.isVendorSelected = false;
    this.getVendors();
    this.getStatus();
    this.getCountries();
    this.getSupplierTypes();
    this.gridDataList.data = [];
    this.isErrors = false;
  }

  public onCloseallPopup(): void {
    this.showSupplier = false;
    this.showWhereUsed = false;
    this.toggleClassshowToggle = this.showWhereUsed ? "active" : "";
    this.toggleClassshowSupplier = this.showSupplier ? "active" : "";
    this.showPopuplayer = false;      
  }

  public onTogglePopupData(type : string, status : boolean): void {
    if(type === "WU"){
      this.showWhereUsed = !this.showWhereUsed;
      this.showVendorSite = false;
      this.showPopuplayer = status;
      this.toggleClassshowToggle = status ? "active" : "";
      this.toggleClassshowVendor = "";
    }
    else if(type === "VS"){
      this.showWhereUsed = false;
      this.showVendorSite = !this.showVendorSite;
      this.showPopuplayer = status;
      this.toggleClassshowToggle = "";
      this.toggleClassshowVendor = status ? "active" : "";
    }
            
  }


  public onToggleSitePopupData(type : string, status : boolean): void {
    if(type === "VP"){
      this.showSupplier = !this.showSupplier;
      this.showWhereUsed = false;
      this.showPopuplayer = status;
      this.toggleClassshowSupplier = status ? "active" : "";
      this.toggleClassshowToggle = "";
    }
  }

  // Close the transfer popup
  close() {
    this.opened = false;
  }

  onCancelVendorcontactList() {
    this.showSupplier = false;
    this.showWhereUsed = false;
    this.showVendorSite = false;
    this.toggleClassshowToggle = this.showWhereUsed ? "active" : "";
    this.toggleClassshowSupplier = this.showSupplier ? "active" : "";
    this.toggleClassshowVendor = this.showVendorSite ? "active" : "";
    this.showPopuplayer = false;
  }


  public getVendors(){
      this.show = true;
      this._vendorMaintenance.getVendors().subscribe(
        data =>{
          this.VendorList = this.model.VendorList= data;
          this.show = false;
        },
        (err: HttpErrorResponse) => {
          this._errorService.error(err);
          this.show = false;
        });
  }


  public getCountries(){
    this.show = true;
    this._vendorMaintenance.getCountries().subscribe(
      data =>{
        this.CountryList = this.model.Countries= data;
        this.show = false;
      },
      (err: HttpErrorResponse) => {
        this._errorService.error(err);
        this.show = false;
      });
  }

  public getStatus(){
    this.show = true;
    this._vendorMaintenance.getVendorStatus().subscribe(
      data =>{
        this.StatusList= this.model.StatusList= data;
        this.show = false;
      },
      (err: HttpErrorResponse) => {
        this._errorService.error(err);
        this.show = false;
      });
  }

  //Grid dropdowns.
  public getVendorSites(VendorId: number){
    
    this._vendorMaintenance.getVendorSites(VendorId).subscribe(
      data =>{
        data.forEach(c => {
            c.StatusList = this.StatusList;
            c.TypesList = this.TypesList;
            c.isAdded = false;
            c.isSelected = false;
            c.isAddMode = false;
            c.VendorStatus = c.StatusList.find(x=>x.Id === c.StatusId);
            c.Type = c.TypesList.find(x=>x.Id === c.TypeId);
            if(this.model.StatusId === 1){
                c.StatusList = c.StatusList.filter(x=>x.Code === 'CREATED');
                c.VendorStatus = c.StatusList[0];
                c.isDisabled = true;
            }else if(this.model.StatusId === 3){
              c.StatusList = c.StatusList.filter(x=>x.Code === 'OBSOLETE');
              c.VendorStatus = c.StatusList[0];
              c.isDisabled = true;
            }else{             
                if(c.StatusId === 3){
                  c.StatusList = c.StatusList.filter(x=>x.Code === 'OBSOLETE');
                  c.VendorStatus = c.StatusList[0];
                  c.isDisabled = true;
                }else{
                  c.StatusList = c.StatusList.filter(x=>x.Code !== 'CREATED');
                  c.VendorStatus = c.StatusList[0];
                  c.isDisabled = false;
                }             
            }        
         });
        this.SiteList = this.gridDataList.data= data;
        this.show = false;
      },
      (err: HttpErrorResponse) => {
        this._errorService.error(err);
        this.show = false;
      });
  }


  onAdd(){
    this.model = new VendorModel();
    this.gridDataList.data = [];
    this.model.VendorList= this.VendorList;
    this.model.Countries = this.CountryList;
    this.model.SupplierSiteDetails = undefined;
    this.model.StatusList = this.StatusList;
    this.model.VendorStatus = this.model.StatusList.find(x=>x.Code === 'CREATED');
    this.model.isAddMode = true;
    this.model.isEditMode = false;
    this.isVendorSelected = false;
  }

  onEdit(){
    this.model.isAddMode = false;
    this.model.isEditMode = true;
  }

  onSiteDelete(site: SupplierSiteDetails){ 
    console.log(site);
    if(site.StatusId === 2){
      if(this.gridDataList.data.filter(x=>x['Status'] === 'PRODUCTION').length == 0){
        this._toastService.warn('There should be at least one supplier in production.');
        return;  
      }
    }    
    this._confirmationService.confirm({
      key: 'message',
      value: {
        message: 'Are you sure! do you want to delete this selected site?',
        continueCallBackFunction: () => this.onSiteDeleteConfirmation(site)
      }
    });
  }

  onSiteDeleteConfirmation(site: SupplierSiteDetails){
    site.isSelected = true;
    let selectedSuppliers: SupplierSiteDetails;
    console.log(this.gridDataList.data.length);
    if(site.isAdded){
     this.gridDataList.data = this.gridDataList.data.filter(x => !(x.isSelected && x.isAdded));
     this.model.SupplierSiteDetails = this.gridDataList.data;
    }else{
      this.gridDataList.data = this.gridDataList.data.filter(x => x.isSelected === true);
      this.model.SupplierSiteDetails = this.gridDataList.data;
      console.log(this.model.SupplierSiteDetails);
      console.log(site);
      this._vendorMaintenance.deleteSupplier(this.model).subscribe(
        data =>{
          this.deleteMessage();
          this.show = false;
          this.onVendorChange(this.model.Vendor);
        },
        (err: HttpErrorResponse) => {
          this._errorService.error(err);
          this.show = false;
        });
    }
  }
  
  onVendorChange(vendor: Vendor){
     if(vendor){
      this.show = true;
      this.isVendorSelected=true;  
      this._vendorMaintenance.getVendorDetailsById(vendor.Id).subscribe(
        data =>{
          this.model = data;
          setTimeout(() => {
            this.getVendorSites(vendor.Id);
          },1000);
          this.model.VendorList= this.VendorList;
          this.model.Countries = this.CountryList;
          this.model.StatusList = this.StatusList;
          this.model.SupplierSiteDetails = this.SiteList;
          this.model.Vendor = this.model.VendorList.find(x=>x.Id === this.model.Id);
          this.model.VendorStatus = this.model.StatusList.find(x=>x.Id === this.model.StatusId);
          this.model.VendorCountry= this.model.Countries.find(x=>x.Name === this.model.Country);
          if(this.model.StatusId !== 1){
            this.model.StatusList = this.model.StatusList.filter(x=>x.Code !== 'CREATED');
          }else{
            this.model.StatusList = this.model.StatusList.filter(x=>x.Code !== 'OBSOLETE');
          }
          
        },
        (err: HttpErrorResponse) => {
          this._errorService.error(err);
          this.show = false;
        });
     }
  }

  handleVendorFilter(value){
    this.model.VendorList = this.VendorList.filter((s) => (s.Name.toString().toLocaleUpperCase()).startsWith(value.toString().toLocaleUpperCase()));
  }

  handleStatusFilter(value){
  }

  onStatusChange(value){
    if(value){
       this.model.StatusId = this.model.VendorStatus.Id;
       this.model.Status = this.model.VendorStatus.Code;
    }
  }

  onCountryChange(value){
    if(value){
      this.model.CountryId= value.Id;
    }
  }

  handleCountryFilter(value){
     this.model.Countries = this.CountryList.filter((s) => (s.Name.toString().toLocaleUpperCase()).startsWith(value.toString().toLocaleUpperCase()));
   }

   onVendorTypeChange(value){
     if(value){
       this.site.TypeId = this.site.Type.Id;
     }
   }

   handleVendorTypeFilter(value){
    this.site.TypesList = this.TypesList.filter((s) => (s.Description.toString().toLocaleUpperCase()).startsWith(value.toString().toLocaleUpperCase()));
   }

   onVendorStatusChange(value){
    if(value){
      this.site.StatusId = this.site.VendorStatus.Id;
      this.site.Status = this.site.VendorStatus.Code;
    }
  }

  handleSupplierStatusFilter(value){
    this.site.StatusList = this.StatusList.filter((s) => (s.Code.toString().toLocaleUpperCase()).startsWith(value.toString().toLocaleUpperCase()));
  }

  filterSortChange(sort: SortDescriptor[]): void {
    this.filterSort = sort;
    this.sortFilterDataRolls();
  }

  sortFilterDataRolls() {
    this.gridDataList = {
      data: orderBy(this.gridDataList.data, this.filterSort),
      total: this.gridDataList.data.length
    };
  }

  AddNewSite(){
     this.site = new SupplierSiteDetails();
     let num = this.gridDataList.data.length;
     this.site.StatusList= this.StatusList;
     this.site.TypesList = this.TypesList;
     this.site.isAdded = true;
     this.site.isSelected = false;
     this.site.FabricItem = false;
     this.site.Trim = false;
     this.site.SendForecast = false;
     this.site.isAddMode = true;
     this.site.OTS = false;
     this.site.recordNumber = num+1;
     if(this.model.isAddMode){
          this.site.VendorStatus = this.model.StatusList.find(x=>x.Code === 'CREATED');
          this.site.isDisabled = true;
     }else{
            if(this.model.StatusId === 1){
              this.site.StatusList = this.site.StatusList.filter(x=>x.Code === 'CREATED');
              this.site.VendorStatus = this.site.StatusList[0];
              this.site.isDisabled = true;
            }else if(this.model.StatusId === 3){
              this.site.StatusList = this.site.StatusList.filter(x=>x.Code === 'OBSOLETE');
              this.site.VendorStatus = this.site.StatusList[0];
              this.site.isDisabled = true;
            }else{
              this.site.StatusList = this.site.StatusList.filter(x=>x.Code !== 'CREATED');
              this.site.isDisabled = false;
            }        
     }
     if(this.model.Vendor){
       this.site.VendorId = this.model.Vendor.Id;
     }else{
       this.site.VendorId = 0;
     }

     this.gridDataList.data.push(this.site);
  }

  public getSupplierTypes(){
    this._vendorMaintenance.getSiteTypes().subscribe(
      data =>{
        this.TypesList = data;
        this.show = false;
      },
      (err: HttpErrorResponse) => {
        this._errorService.error(err);
        this.show = false;
      });
  }

  openVendorSite(site: SupplierSiteDetails){
    this.siteItem = site;
    if(this.siteItem.isAdded === true){
        let num = this.gridDataList.data.length;
        this.siteItem.recordNumber = num+1;
    }else{
      this.siteItem.recordNumber = 0;
    }
    this.showVendorSite = true;
  }

  updateSiteRecords(site){
    this.onCancelVendorcontactList();
    console.log('in update site records');
    console.log(site);
    if(site.isAddMode === true){
      this.record = this.gridDataList.data.find(x=>x.recordNumber === site.recordNumber);
      this.record = site;
    }else{
      this.record = this.gridDataList.data.find(x=>x.Id === site.Id);
      this.record = site;
    }
    console.log('After changing the grid data');
    console.log(this.gridDataList.data);
  }

  saveSupplier(): any{
    this.isErrors = false;
   if(this.gridDataList.data.length === 0 && this.model.VendorStatus.Code === 'PRODUCTION'){
      this._toastService.error('There should be atlease one site detail when the supplier status is in PRODUCTION');
      return;
    }
    if(!this.model.Name ||
       !this.model.Code ||
       !this.model.ShortDescription ||
       !this.model.AddressLine1 ||
       !this.model.City ||
       !this.model.State ||
       !this.model.VendorCountry ||
       !this.model.Zip){
        this.isErrors = true;
        this._toastService.error('Please provide all required Information.');
        return;
       }

     if(this.model.Consignment && (!this.model.ConsignmentTakeDays || this.model.ConsignmentTakeDays == 0)){
      this.isErrors = true;
      this._toastService.error('Please provide Consignment Take Days.');
      return;
     }

     if(this.gridDataList.data.length > 0){
       this.gridDataList.data.forEach (x => {
           if(!x.Name ||
              !x.Code ||
              !x.Description ||
              !x.Type ||
              !x.OrderToPickup){
                this.isErrors = true;
                this._toastService.error('Please provide all required details for the site.');
                return;
              }
       });
     }
    if(!this.isErrors){
        this._confirmationService.confirm({
          key: 'message',
          value: {
            message: 'Do you want to save the Supplier?',
            continueCallBackFunction: () => this.saveSupplierConfirm()
          }
        });
     }

     console.log(this.model);
  }

 

  saveSupplierConfirm():any{
    
    this.show = true;
    this.model.StatusId= this.model.VendorStatus.Id;
    this.model.Status = this.model.VendorStatus.Code;
    this.model.CountryId = this.model.VendorCountry.Id;
    this.model.Country = this.model.VendorCountry.Name;
    this.model.Countries = [];
    this.model.StatusList = [];
    if(this.gridDataList.data.length <= 0){
      this.model.SupplierSiteDetails=[];
    }else{
      this.model.SupplierSiteDetails = this.gridDataList.data;
    }
    this._vendorMaintenance.saveSupplier(this.model).subscribe(
      data => {
         this.show = false;
         if(this.model.Id){
           this.updateMessage();
           this.model.isAddMode = false;
           this.model.isEditMode = true;
           this.onVendorChange(this.model.Vendor);
         }else{
           this.successMessage();
           this.model.isAddMode = true;
           this.model.isEditMode = false;
           this.model.Vendor = this.VendorList.find(x=>x.Name === this.model.Name);
           this.onVendorChange(this.model.Vendor);
         }   
          this.show = false;      
      },
      (err: HttpErrorResponse) => {
        this.show = false;
        this._toastService.error('Error occured while creating new Item. Please retry.');
      });
  }

  successMessage() {
    this._toastService.success('Supplier has been created successfully.');
  }

  updateMessage() {
    this._toastService.success('Supplier has been updated successfully.');
  }

  deleteMessage() {
    this._toastService.success('Selected Site has been deleted successfully.');
  }

  onSupplierCancel():any{
    this._confirmationService.confirm({
      key: 'message',
      value: {
        message: 'Do you want to cancel your changes?',
        continueCallBackFunction: () => this.cancelSupplierConfirm()
      }
    });

  }

  cancelSupplierConfirm(){
    this.ngOnInit();
  }

}

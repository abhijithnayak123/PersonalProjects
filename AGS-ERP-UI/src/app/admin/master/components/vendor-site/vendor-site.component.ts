import { Component, OnInit , Input, Output, EventEmitter} from '@angular/core';
import { HttpErrorResponse } from "@angular/common/http";
import { ErrorService } from "../../../../shared/services/error.service";
import { SuccessService } from '../../../../shared/services/success.service';
import { ConfirmationService } from '../../../../shared/services/confirmation.service';
import { ToastService } from '../../../../shared/services/toast.service';
import { VendorMaintenanceService } from '../vendor-maintenance/vendor-maintenance.service';
import { Inco } from "../../models/Inco";
import { Port } from "../../models/Port";
import { PoCommunication } from "../../models/PoCommunication";
import { DeliveryEvent } from "../../models/DeliveryEvent";
import {Asn} from "../../models/Asn";
import { Country } from '../../models/Country';
import { SupplierSiteDetails } from '../../models/SupplierSiteDetails';
import { VendorSite } from '../../models/VendorSite';

@Component({
  selector: 'app-vendor-site',
  templateUrl: './vendor-site.component.html',
  styleUrls: ['./vendor-site.component.css'],
  providers: [VendorMaintenanceService]
})
export class VendorSiteComponent implements OnInit {

constructor(private _vendorMaintenance:VendorMaintenanceService,
            private _successService: SuccessService,
            private _confirmationService: ConfirmationService,
            private _toastService: ToastService,
            private _errorService: ErrorService){ }

  @Input() VendorName;
  @Input('SiteItem') site: VendorSite;
  @Output() onSave: EventEmitter<VendorSite> = new EventEmitter<VendorSite>(); 
  @Output() onClose: EventEmitter<boolean> = new EventEmitter<boolean>();

  asnList : Array<Asn>=[];
  portList: Array<Port>=[];
  communications: Array<PoCommunication>=[];
  eventList : Array<DeliveryEvent>=[];
  incoList : Array<Inco>=[];
  CountryList : Array<Country>=[];
  show: boolean = false;

  isErrors: boolean = false;

  //site: SupplierSiteDetails;


  ngOnInit() {
    this.show = true;
    this.getCountries();  
    this.getPortList();
    this.getAsnTypes();
    this.getIncos();
    this.getCommunications();
    this.getEvents();
    this.getModelObject(this.site);
    this.isErrors = false;
  }

  public getModelObject(site){
    this.site.VendorName= this.VendorName;
    this.site.CountryList = this.CountryList;
    this.site.AsnList = this.asnList;
    this.site.IncoList = this.incoList;
    this.site.DeliveryList = this.eventList;
    this.site.Communications= this.communications;
    this.site.Ports = this.portList;

    this.site.Country = undefined;
    this.site.IncoSite = undefined;
    this.site.PortSite = undefined;
    this.site.POCommunicationSite = undefined;
    this.site.DeliveryEventSite = undefined;
    this.site.AsnSite = undefined;

    setTimeout(() => {
      this.site.Country = this.site.CountryList.find(x=>x.Id === this.site.CountryId);
      this.site.IncoSite = this.site.IncoList.find(x=>x.Id === this.site.INCOId);
      this.site.AsnSite = this.site.AsnList.find(x=>x.Id === this.site.ASNId);
      this.site.PortSite = this.site.Ports.find(x=>x.Id === this.site.PortId);
      this.site.POCommunicationSite = this.site.Communications.find(x=>x.Id === this.site.POCommunicationId);
      this.site.DeliveryEventSite = this.site.DeliveryList.find(x=>x.Id === this.site.DeliveryEventId);
    },2000);

    this.show = false;
  }

  public getAsnTypes(){
    this._vendorMaintenance.getAsn().subscribe(
      data =>{
        this.asnList = this.site.AsnList= data;       
      },
      (err: HttpErrorResponse) => {
        this._errorService.error(err);
        this.show = false;
      });
  }

  public getPortList(){
    this._vendorMaintenance.getPorts().subscribe(
      data =>{
        this.portList = this.site.Ports= data;
        this.show = false;
      },
      (err: HttpErrorResponse) => {
        this._errorService.error(err);
        this.show = false;
      });
  }

  public getCommunications(){
    this._vendorMaintenance.getCommunications().subscribe(
      data =>{
        this.communications = this.site.Communications= data;        
      },
      (err: HttpErrorResponse) => {
        this._errorService.error(err);
        this.show = false;
      });
  }

  public getEvents(){
    this._vendorMaintenance.getDeliveryEvent().subscribe(
      data =>{
        this.eventList = this.site.DeliveryList= data;
      },
      (err: HttpErrorResponse) => {
        this._errorService.error(err);
        this.show = false;
      });
  }

  public getIncos(){
    this._vendorMaintenance.getIncos().subscribe(
      data =>{
        this.incoList = this.site.IncoList= data;
      },
      (err: HttpErrorResponse) => {
        this._errorService.error(err);
        this.show = false;
      });
  }

  public getCountries(){
    this._vendorMaintenance.getCountries().subscribe(
      data =>{
        this.CountryList = this.site.CountryList=  data;
      },
      (err: HttpErrorResponse) => {
        this._errorService.error(err);
        this.show = false;
      });
  }

  public closeWindow():void
  {
    this.onClose.emit(false);
  }

  saveSite():any{
    console.log(this.site);
    if(!this.site.AddressLine1 ||
       !this.site.City ||
       !this.site.State ||
       !this.site.Zip  ||
       !this.site.Country ||
       !this.site.IncoSite ||
       !this.site.DeliveryEventSite ||
       !this.site.POCommunicationSite ||
       !this.site.TPNumber ||
       !this.site.PortSite){
        this.isErrors = true;
        this._toastService.error('Please provide all required Information.');
        return;
    }else{
      // if(!this.isErrors){
      //     this._confirmationService.confirm({
      //       key: 'message',
      //       value: {
      //         message: 'Do you want to save these Additional details?',
      //         continueCallBackFunction: () => this.saveAdditionalDetails(this.site)
      //         }
      //       });
      //  }      
      this.saveAdditionalDetails(this.site);
    }
  }

  saveAdditionalDetails(site):any{
      this.onSave.emit(site);
      this.SuccessMessage();
  }

  SuccessMessage(){
    this._toastService.success('Address and Aditional details have been saved successfully!.');
  }

  countryChange(value){
    if(value){
      this.site.CountryId = this.site.Country.Id;
    }
  }

  countryFilter(value){
   this.site.CountryList = this.CountryList.filter((s) => (s.Name.toString().toLocaleUpperCase()).startsWith(value.toString().toLocaleUpperCase()));
  }

  incoChange(value){
    if(value){
      this.site.INCOId = this.site.IncoSite.Id;
    }
  }

  incoFilter(value){
   this.site.IncoList = this.incoList.filter((s) => (s.Description.toString().toLocaleUpperCase()).startsWith(value.toString().toLocaleUpperCase()));
  }

  eventChange(value){
    if(value){
      this.site.DeliveryEventId = this.site.DeliveryEventSite.Id;
    }
  }

  eventFilter(value){
   this.site.DeliveryList = this.eventList.filter((s) => (s.Description.toString().toLocaleUpperCase()).startsWith(value.toString().toLocaleUpperCase()));
  }

  portChange(value){
    if(value){
      this.site.PortId = this.site.PortSite.Id;
    }
  }

  portFilter(value){
   this.site.Ports = this.portList.filter((s) => (s.Description.toString().toLocaleUpperCase()).startsWith(value.toString().toLocaleUpperCase()));
  }

  asnChange(value){
    if(value){
      this.site.ASNId = this.site.AsnSite.Id;
    }
  }

  asnFilter(value){
   this.site.AsnList = this.asnList.filter((s) => (s.Description.toString().toLocaleUpperCase()).startsWith(value.toString().toLocaleUpperCase()));
  }

  communicationChange(value){
    if(value){
      this.site.POCommunicationId = this.site.POCommunicationSite.Id;
    }
  }

  communicationFilter(value){
   this.site.Communications = this.communications.filter((s) => (s.Description.toString().toLocaleUpperCase()).startsWith(value.toString().toLocaleUpperCase()));
  }


}

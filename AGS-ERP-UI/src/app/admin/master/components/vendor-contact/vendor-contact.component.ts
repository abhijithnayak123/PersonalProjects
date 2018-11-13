import { Component, OnInit, EventEmitter, Output, Input} from '@angular/core';
import { VendorContact } from '../../models/VendorContact';
import { Site } from '../../models/site';
import { ConfirmationService } from '../../../../shared/services/confirmation.service';
import { VendorMaintenanceService } from '../vendor-maintenance/vendor-maintenance.service';
import { HttpErrorResponse } from "@angular/common/http";
import { ErrorService } from "../../../../shared/services/error.service";
import { SuccessService } from '../../../../shared/services/success.service';
import { ToastService } from '../../../../shared/services/toast.service';
import { cpus } from 'os';

@Component({
  selector: 'app-vendor-contact',
  templateUrl: './vendor-contact.component.html',
  styleUrls: ['./vendor-contact.component.css'],
  providers: [VendorMaintenanceService]
})
export class VendorContactComponent implements OnInit {

  constructor(
    private _confirmationService: ConfirmationService,
    private _vendorMaintenance:VendorMaintenanceService,
    private _errorService: ErrorService,
    private _toastService: ToastService
  ) { }

  showSpinner: boolean;
  //vendorConatctData: VendorContact[];
  vendorSiteData: Site[];
  show: boolean = false;
  model: VendorContact;
  @Input() VendorId;
  vendorConatctData : Array<VendorContact>=[];
  SitesList: Array<Site>=[];

  isErrors : boolean = false;

  @Output() cancel: EventEmitter<Boolean> = new EventEmitter();
  
  ngOnInit() {
    this.show = true;
    this.model = new VendorContact();
    this.getContractSites(this.VendorId);
    this.getVendorContracts(this.VendorId);  
    this.isErrors = false;
  }

  getVendorContracts(VendorId){
    this._vendorMaintenance.getVendorContracts(VendorId).subscribe(
      data =>{
        data.forEach(c => {
          console.log(c);
          c.SiteList = this.SitesList;
          c.IsAdded = false;
          c.Site = c.SiteList.find(x=>x.Id === c.SiteId);
        });
        this.vendorConatctData = data;
        this.show = false;
      },
      (err: HttpErrorResponse) => {
        this._errorService.error(err);
        this.show = false;
      }
    );
  }

  getContractSites(VendorId){
    this._vendorMaintenance.getContractSites(VendorId).subscribe(
      data =>{
        this.SitesList = data;
        this.show = false;
      },
      (err: HttpErrorResponse) => {
        this._errorService.error(err);
        this.show = false;
      }
    );

  }


  onAdd(){
     let contact = new VendorContact();
     contact.SiteList = this.SitesList;
     contact.IsAdded = true;
     contact.PrimaryContact=false;
     contact.POContact = false;
     contact.FinanceContact = false;
     contact.VendorId = this.VendorId;
     contact.IsSelected = false;
     this.vendorConatctData.push(contact);
  } 

  onDelete() {      
    this._confirmationService.confirm({
      key: 'message',
      value: {
        message: 'Are you sure! do you want to delete selected vendor contacts?',
        continueCallBackFunction: () => this.onDeleteConfirmation()
      }
    });
  }

  onDeleteConfirmation() {
    this.vendorConatctData = this.vendorConatctData.filter(c => !(c.IsSelected && c.IsAdded));
    let selectedContacts: VendorContact[] = this.vendorConatctData.filter(c => c.IsSelected);
    if(selectedContacts.length > 0){
    console.log(selectedContacts);
    this._vendorMaintenance.deleteContact(selectedContacts).subscribe(
      data => {
         this.show = false;
         this.deleteMessage(); 
         this.ngOnInit();     
      },
      (err: HttpErrorResponse) => {
        this.show = false;
        this._toastService.error('Error occured while creating new contact. Please retry.');
      });
    }
  }

  onSave() {
    console.log(this.vendorConatctData);
    this.vendorConatctData.forEach( d =>
      {
        if(!d.ContactName || !d.Email){
            this.isErrors = true;
            this._toastService.error('Please provide all required Information.');
            return;
        }
        if(!d.PrimaryContact && !d.FinanceContact && !d.POContact){
          this.isErrors = true;
          this._toastService.error('Please choose atleast one contact among Primary contact, Finance Contact and Po Contact.');
          return;
        }
      }
    );

    console.log(this.vendorConatctData);
    if(!this.isErrors){
          this._confirmationService.confirm({
            key: 'message',
            value: {
              message: 'Do you want to save the Contact?',
              continueCallBackFunction: () => this.saveVendorContact()
            }
          });
     }
  }

  saveVendorContact():any{
   
    this.vendorConatctData.forEach( d =>
      {
       d.SiteId = d.Site.Id;
       d.SiteName = d.Site.Name;
      });

    this._vendorMaintenance.saveContact(this.vendorConatctData).subscribe(
      data => {
         this.show = false;
         this.successMessage();  
         this.ngOnInit();  
      },
      (err: HttpErrorResponse) => {
        this.show = false;
        this._toastService.error('Error occured while creating new contact. Please retry.');
      });
  }

  successMessage() {
    this._toastService.success('Vendor Contact has been added successfully.');
  }

  deleteMessage() {
    this._toastService.success('Vendor Contact has been deleted successfully.');
  }

  onCancel (){
    this.cancel.emit(true);
  }

  isAllSelected() {
    return this.vendorConatctData.length > 0 && this.vendorConatctData.every(c => c.IsSelected === true);
  }
  
  onSelectAllCheck(event) {
    this.vendorConatctData.forEach(c => c.IsSelected = event.target.checked);
  }

  canEnableActions() {
    return !this.vendorConatctData.some(c => c.IsSelected === true);
  }

}

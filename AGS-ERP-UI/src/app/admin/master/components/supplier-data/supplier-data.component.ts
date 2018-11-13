import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Align } from '@progress/kendo-angular-popup';
import { Supplier } from '../../models/Supplier';
import { ItemMaintenanceService } from '../item-maintenance/item-maintenance.service';
import { ErrorService } from '../../../../shared/services/error.service';
import { SupplierVendor } from '../../models/SupplilerVendor';
import { COO } from '../../models/COO';
import { UoM } from '../../models/UoM';
import { SupplierVendorSite } from '../../models/SupplierVendorSite';
import { GridComponent, GridDataResult } from '@progress/kendo-angular-grid';
import { Item } from '../../models/Item';
import { ItemDetail } from '../../models/ItemDetail';
import { HttpErrorResponse } from "@angular/common/http";
import { ConfirmationService } from '../../../../shared/services/confirmation.service';
import { ToastService } from '../../../../shared/services/toast.service';
import { ItemModel } from '../../models/ItemModel';

@Component({
  selector: 'app-supplier-data',
  templateUrl: './supplier-data.component.html',
  styleUrls: ['./supplier-data.component.css'],
  providers: [ItemMaintenanceService]
})
export class SupplierDataComponent implements OnInit {
  anchorAlign: Align = { horizontal: "right", vertical: "bottom" };
  popupAlign: Align = { horizontal: "right", vertical: "top" };
  noItem : boolean = true;
  showControls: boolean = false;

  showDelete: boolean = false;

  SupplierData: Array<Supplier>=[];
  COOList: COO[];
  UOMList:Array<UoM>=[];
  VendorList: SupplierVendor[];
  filteredVendorList: SupplierVendor[];
  UniqueVendors: Array<String>=[];
  VendorSiteList: SupplierVendorSite[];
  filteredVendorSiteList: SupplierVendorSite[];
  item: ItemDetail;
  itemCode: string;
  @Input() ItemId: number;
  StatusId: number;
  UOMID:number;
  COO: string;
  show: boolean=false;
  isCreated: boolean = false;
  isAbsolete: boolean = false;
  VendorStatus: Array<string>=[];
  @Output() onClose: EventEmitter<boolean> = new EventEmitter<boolean>();

  model : ItemModel;
  

  gridView: GridDataResult = { data: [], total: 0 };

  constructor(    
    private _errorService: ErrorService,
    private _itemMaintenance: ItemMaintenanceService,
    private _toastService: ToastService,
    private _confirmationService: ConfirmationService
  ) { }
  
  ngOnInit() {
    this.show = true;
    this.getSupplierVendors();  
    this.isCreated = false;
    this.isAbsolete = false;
    this.model = new ItemModel();
    this.getItemDetails(this.ItemId);
  }

  getItemDetails(ItemId: number){
      this._itemMaintenance.getItemDetails(ItemId).subscribe(
        data => {
           this.model = data;
           this.StatusId = this.model.StatusId;
           this.UOMID = this.model.StockUOMId;
           this.itemCode = this.model.Code;
        }
      );
  }

  getSupplierData(ItemId: number) {
    this._itemMaintenance.getSupplerDataByItem(ItemId).subscribe(
      data => {
        data.forEach(c => {
          c.VendorList = this.VendorList;
          c.COOList = this.COOList;
          c.Vendor = this.VendorList.find(d => d.Id === c.VendorId);
          c.VendorSiteList = this.VendorSiteList;
          c.VendorSite = this.VendorSiteList.find(d => d.Id === c.VendorSiteId);
          c.ItemCode = this.itemCode;
          c.IsAdded= false;
          c.UOMList = this.UOMList;
          c.UOM = this.UOMList.find(x=>x.Id === this.UOMID).Code;
          c.IsCreated=false;
          c.VendorStatus = ["PRODUCTION","OBSOLETE"];
	        if(this.StatusId === 1){
            c.Status = 'CREATED';
            c.IsCreated = true;
            c.IsAbsolete = false;
          }
          if(this.StatusId === 3){
            c.Status = 'OBSOLETE';
            c.IsCreated = true;
            c.IsAbsolete = true;
            c.AllocationPercentage = 0;
          }
          if(this.StatusId === 2){
            if(c.StageId === 3){
              c.Status = 'OBSOLETE';
            }else{
              c.Status = 'PRODUCTION';
            }
            c.IsCreated = false;
            c.IsAbsolete = false;
          }
        });
        this.SupplierData = data;
      }
    );
  }

  getCOOs() {
    this._itemMaintenance.getCOOs().subscribe(
      data => {
        this.COOList = data;
        this.getSupplierData(this.ItemId);
      }
    );
  }

  getUoMList(){
    this._itemMaintenance.getUOM().subscribe(
      data =>{
        this.UOMList = data;
      },
      (err: HttpErrorResponse) => {
        this._errorService.error(err);
      });
   }

  getSupplierVendors() {
    this.show = true;
    this._itemMaintenance.getSupplierVendors().subscribe(
      data => {
        this.VendorList = data;
        this.getCOOs();
        this.getSupplierVendorSites();
        this.getUoMList();
        this.show = false;
      }
    );
  }

  getSupplierVendorSites() {
    this.VendorSiteList = [];
    this.VendorList.forEach(c => {
      this.VendorSiteList.push(new SupplierVendorSite(c.SiteId, c.SiteCode, c.SiteName, c.Id));
    });
  }

  onAdd() {
    this.show = true;
    let supplierData = new Supplier();
    supplierData.ItemId = this.ItemId;
    supplierData.ItemCode = this.itemCode;
    supplierData.IsSelected=false;
    supplierData.IsAdded=true;
    supplierData.VendorList = this.VendorList;
    supplierData.VendorSiteList = this.VendorSiteList;
    supplierData.UOM = this.UOMList.find(x=>x.Id === this.UOMID).Code;
    supplierData.AllocationPercentage = 0;
   
    supplierData.VendorStatus = ["PRODUCTION","OBSOLETE"];
    if(this.StatusId ===  1){
      supplierData.Status = 'CREATED';
      supplierData.IsCreated = true;
      supplierData.IsAbsolete = false;
    }
    if(this.StatusId === 3){
      supplierData.Status = 'OBSOLETE';
      supplierData.IsCreated = true;
      supplierData.IsAbsolete = true;
      supplierData.AllocationPercentage = 0;
    }
    if(this.StatusId === 2){
      supplierData.Status = 'PRODUCTION';
      supplierData.IsCreated = false;
      supplierData.IsAbsolete = false;
    }
    this.SupplierData.push(supplierData);
    this.show = false;
  }

  onDelete() {      
    this._confirmationService.confirm({
      key: 'message',
      value: {
        message: 'Are you sure! do you want to delete selected suppliers?',
        continueCallBackFunction: () => this.onDeleteConfirmation()
      }
    });
  }

  onDeleteConfirmation(){
    this.show = true;
    let selectedSuppliers: Array<Supplier> = [];
    this.SupplierData = this.SupplierData.filter(c => !(c.IsSelected && c.IsAdded));
    selectedSuppliers= this.SupplierData.filter(c => c.IsSelected === true);
    if(selectedSuppliers.length > 0){
      if(this.StatusId !== 1){
        this._toastService.error('selected suppliers cannot be removed as Item is in Production.');
        this.show = false;
        return;
      }
      this._itemMaintenance.deleteSuppliers(selectedSuppliers).subscribe(
        data => {
          this.show = false;
          this.deleteMessage();
          this.ngOnInit();
          this.show = false;
        },
        (err: HttpErrorResponse) => {
          this.show = false;
          this._errorService.error(err);
        }
      );
    }
  }

  hasDuplicates(vendorsList: Array<String>) {
    let recipientsArray = vendorsList.sort(); 
    var reportRecipientsDuplicate = [];
    for (var i = 0; i < recipientsArray.length - 1; i++) {
        if (recipientsArray[i + 1] == recipientsArray[i]) {
            reportRecipientsDuplicate.push(recipientsArray[i]);
        }
    }
    if(reportRecipientsDuplicate.length > 0){
      return true;
    }else{
      return false;
    }
}

  onSave() {
    this.show = true;
    this.UniqueVendors=[];
    this.SupplierData.map(x=>this.UniqueVendors.push(x.Vendor.Name));
    if(this.hasDuplicates(this.UniqueVendors)){
      this._toastService.warn('Duplicate records exists for Same Vendor. Please remove.');
      return;  
    }
    if(this.StatusId === 2){
      if(this.SupplierData.filter(x=>x['Status'] === 'PRODUCTION').length == 0){
        this._toastService.warn('There should be at least one supplier in production.');
        return;  
      }
    }
    let totalPercentage = this.SupplierData.map(item => item.AllocationPercentage).reduce((prev, next) => prev + next);
    if(totalPercentage !== 100){
      this._toastService.error('The Total Allocation Percentage should be 100');
      return;
    }

    this.SupplierData.forEach(c => {
      c.VendorId = c.Vendor === undefined ? null : c.Vendor.Id;
      c.VendorSiteId = c.VendorSite === undefined ? null : c.VendorSite.Id;
      c.StageId = c.Status === 'CREATED' ? 1 : c.Status === 'PRODUCTION' ? 2 : c.Status === 'OBSOLETE' ? 3 : null;
    });
    this._itemMaintenance.saveSuppliers(this.SupplierData).subscribe(
      data => {
        this.successMessage();
        this.show = false;
        this.ngOnInit();
      },
      (err: HttpErrorResponse) => {
        this.show = false;
        this._errorService.error(err);
      }
    );
      
  }
  onSelectAllCheck(event) {
    this.SupplierData.forEach(c => c.IsSelected = event.target.checked);
  }

  onSelectCheck(checked: boolean) {
    if(this.SupplierData.filter(x=>x.IsSelected === true).length > 0){
      this.showDelete = true;
    }else{
      this.showDelete = false;
    }
  }

  isAllSelected() {
    return this.SupplierData.length > 0 && this.SupplierData.every(c => c.IsSelected === true);
  }

  vendorChange(supplier: Supplier){
    supplier.VendorSiteList = this.VendorSiteList.filter(c => c.VendorId === supplier.Vendor.Id);
    if(supplier.VendorSiteList.length === 1){
      supplier.VendorSite = supplier.VendorSiteList[0];
    }
  }
    
  public onClosePopup(): void {
    this.onClose.emit(true);   
  }

  OnVendorStatusChange(supplier: Supplier){
        if(supplier.Status === 'OBSOLETE'){
            supplier.IsAbsolete = true;
            supplier.IsCreated = false;
            supplier.AllocationPercentage = 0;
        }else{
          supplier.IsAbsolete = false;
          supplier.IsCreated = false;
        }
  }

  handleVendorCodeFilter(value: string, supplier: Supplier) {
      supplier.VendorList = this.VendorList.filter((s) => (s.Code.toString().toLocaleUpperCase()).startsWith(value.toString().toLocaleUpperCase()));
  }

  handleVendorNameFilter(value: string, supplier: Supplier) {
      supplier.VendorList = this.VendorList.filter((s) => (s.Name.toString().toLocaleUpperCase()).startsWith(value.toString().toLocaleUpperCase()));
  }

  handleVendorSiteFilter(value: string, supplier: Supplier) {
      supplier.VendorSiteList = this.VendorSiteList.filter((s) => (s.Name.toString().toLocaleUpperCase()).startsWith(value.toString().toLocaleUpperCase()));
  }

  handleUOMFilter(value: string, supplier: Supplier) {
      supplier.UOMList = this.UOMList.filter((s) => (s.Code.toString().toLocaleUpperCase()).startsWith(value.toString().toLocaleUpperCase()));
  }

  successMessage() {
    this._toastService.success('Supplier details saved successfully.');
  }

  deleteMessage() {
    this._toastService.success('selected suppliers have been deleted.');
  }


}

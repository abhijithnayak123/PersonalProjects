import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { Align } from '@progress/kendo-angular-popup';
import { HttpErrorResponse } from "@angular/common/http";
import { ErrorService } from "../../../../shared/services/error.service";
import { SuccessService } from '../../../../shared/services/success.service';
import { ConfirmationService } from '../../../../shared/services/confirmation.service';
import { LocalStorageService } from '../../../../shared/wrappers/local-storage.service';
import { ToastService } from '../../../../shared/services/toast.service';
import { AlertService } from '../../../../shared/services/alert.service';
import { ItemMaintenanceService} from '../item-maintenance/item-maintenance.service'
import { Item } from '../../../master/models/Item';
import { Supplier } from '../../../master/models/Supplier';
import { ItemType } from '../../models/ItemType';
import { ItemStatus } from '../../models/ItemStatus';
import { ItemColor } from '../../models/ItemColor';
import { ItemDetail } from '../../models/ItemDetail';
import { UoM } from '../../models/UoM';
import { ProductCategory } from '../../models/ProductCategory';
import { GreigeItem } from '../../models/GreigeItem';
import {ItemModel} from '../../models/ItemModel';

import * as moment from 'moment';
import { CommonService } from "../../../../shared/services/common.service";


@Component({
  selector: 'app-item-maintenance',
  templateUrl: './item-maintenance.component.html',
  styleUrls: ['./item-maintenance.component.css'],
  providers: [ItemMaintenanceService]
})

export class ItemMaintenanceComponent implements OnInit, OnDestroy {

  @Input() ItemCode: string;

  toggleClassshowToggle: string = "";
  toggleClassshowSupplier: string = "";
  anchorAlign: Align = { horizontal: "right", vertical: "bottom" };
  popupAlign: Align = { horizontal: "right", vertical: "top" };
  showWhereUsed: boolean = false;
  showSupplier: boolean = false;
  showPopuplayer: boolean = false;

  showDelete: boolean = false;
  model : ItemModel;
  suppliers: Array<Supplier>=[];
  previousStatus : ItemStatus;
  ItemList: Array<Item> = [];
  filteredItemList: Array<Item> = [];
  ItemTypeList: Array<ItemType> = [];
  filteredItemTypeList: Array<ItemType> = [];
  ItemStatusList: Array<ItemStatus> = [];
  filteredItemStatusList: Array<ItemStatus> = [];
  currentStatusList: Array<ItemStatus> = [];
  ItemColorList: Array<ItemColor> = [];
  filteredItemColorList: Array<ItemColor> = [];
  UOMList:Array<UoM>=[];
  filteredUOMList:Array<UoM>=[];
  PCList:Array<ProductCategory>=[];
  filteredPCList:Array<ProductCategory>=[];
  GIList:Array<GreigeItem>=[];
  filteredGIList:Array<GreigeItem>=[];
  show: boolean;
 
  isItemSelected: boolean = false;
 

  constructor(
    private _itemMaintenance: ItemMaintenanceService,
    private _successService: SuccessService,
    private _confirmationService: ConfirmationService,
    private _localStorageService: LocalStorageService,
    private _toastService: ToastService,
    private _alertService: AlertService,
    private _errorService: ErrorService,
    private _commonService: CommonService
    ) 
  { }

  ngOnInit() {
    this.show = true;
    this.model = new ItemModel();
    this.model.isAddMode = false;
    this.model.isEditMode = false;
    this.getItemList();
    this.getItemStatusList();
    this.isItemSelected = false;   
    if(this.ItemCode){
      setTimeout(() => {
        this.model.Item = this.ItemList.find(x=>x.Code === this.ItemCode);
        this.onItemChange(this.model.Item);
      },3000);
   }
  }

  ngOnDestroy(){
    this._commonService.Notify({
      key: 'destroyItem',
      value: { 'itemCode': ''}
    });
  }

  public displayAdd(): void{
    this.show = true;
    this.model = null;
    this.model = new ItemModel();
    this.model.ItemList = this.ItemList;
    this.model.StatusList = this.ItemStatusList;
    this.model.TypeList = this.ItemTypeList;
    this.model.UOMList = this.UOMList;
    this.model.PCList = this.PCList;
    this.model.GIList = this.GIList;
    this.model.ColorList = this.ItemColorList;
    this.model.CreatedON = new Date();
    this.model.Status = this.model.StatusList.find(x=>x.Code === 'CREATED');
    this.model.isAddMode = true;
    this.show = false;
  }

  public initLists(){

  }

  public onCloseallPopup(): void {
      this.showSupplier = false;
      this.showWhereUsed = false;
      this.toggleClassshowToggle = this.showWhereUsed ? "active" : "";
      this.toggleClassshowSupplier = this.showSupplier ? "active" : "";
      this.showPopuplayer = false;         
  }

  public onTogglePopupData(type : string, status : boolean): void {
    if(type === "SD"){
      this.show = true;
      this.showSupplier = !this.showSupplier;
      this.showWhereUsed = false;
      this.showPopuplayer = status;
      this.toggleClassshowSupplier = status ? "active" : "";
      this.toggleClassshowToggle = "";
      this.show = false;
    }
    else if(type === "WU"){
      this.showWhereUsed = !this.showWhereUsed;
      this.showSupplier = false;
      this.showPopuplayer = status;
      this.toggleClassshowToggle = status ? "active" : "";
      this.toggleClassshowSupplier = "";
    }        
  }

  getItemList(){
    this.show = true;
    this._itemMaintenance.getItems().subscribe(
      data =>{
        this.ItemList = this.model.ItemList= data;
      },
      (err: HttpErrorResponse) => {
        this._errorService.error(err);
        this.show = false;
      });
  }

  getItemTypesList(){
    this.show = true;
    this._itemMaintenance.getItemTypes().subscribe(
      data =>{
        this.ItemTypeList = this.model.TypeList= data;
        this.show = false;
      },
      (err: HttpErrorResponse) => {
        this._errorService.error(err);
        this.show = false;
      });
  }

  getItemStatusList(){
   
    this._itemMaintenance.getItemStatus().subscribe(
      data =>{
        this.ItemStatusList = this.model.StatusList= data;
        this.getItemTypesList();
        this.getItemColorList();
        this.getGIList();
        this.getPCList();
        this.getUoMList();      
        //this.show = false;
      },
      (err: HttpErrorResponse) => {
        this._errorService.error(err);
        this.show = false;
      });
  }

  getItemColorList(){
    this.show = true;
    this._itemMaintenance.getItemColors().subscribe(
      data =>{
        this.ItemColorList = this.model.ColorList= data;
        this.show = false;
      },
      (err: HttpErrorResponse) => {
        this._errorService.error(err);
        this.show = false;
      });
   }

   getUoMList(){
    this.show = true;
    this._itemMaintenance.getUOM().subscribe(
      data =>{
        this.UOMList = this.model.UOMList= data;
        this.show = false;
      },
      (err: HttpErrorResponse) => {
        this._errorService.error(err);
        this.show = false;
      });
   }

   getPCList(){
    this.show = true;
    this._itemMaintenance.getProductCategory().subscribe(
      data =>{
        this.PCList = this.model.PCList = data;
        this.show = false;
      },
      (err: HttpErrorResponse) => {
        this._errorService.error(err);
        this.show = false;
      });
   }

   getGIList(){
    this.show = true;
    this._itemMaintenance.getGreigeItems().subscribe(
      data =>{
        this.GIList = this.model.GIList = data;
        this.show = false;
      },
      (err: HttpErrorResponse) => {
        this._errorService.error(err);
        this.show = false;
      });
   }

  onItemChange(item: Item){
    console.log('ITEM IN ITEM CHANGE-->'+item);
    if(item){
      this.show = true;
      this.model.Id = item.Id;
      this.model.Code = item.Code;
      this.isItemSelected=true;
      this._itemMaintenance.getItemDetails(item.Id).subscribe(
        data =>{
          this.model =  data;
          this.model.ItemList = this.ItemList;
          this.model.StatusList = this.ItemStatusList;
          this.model.TypeList = this.ItemTypeList;
          this.model.UOMList = this.UOMList;
          this.model.PCList = this.PCList;
          this.model.GIList = this.GIList;
          this.model.ColorList = this.ItemColorList;

          this.model.Item =this.model.ItemList.find(x=>x.Id === this.model.Id);
          this.model.Status= this.previousStatus=  this.model.StatusList.find(x=>x.Id === this.model.StatusId);
          this.model.Type = this.model.TypeList.find(x=>x.Id == this.model.TypeId);
          if(this.model.TypeId == 1 || this.model.TypeId == 2){
              this.model.UOMList = this.model.UOMList.filter(x=>x.Code === 'YD');
              this.model.CostUOM = this.model.UOMList[0];
              this.model.StockUOM = this.model.UOMList[0];
          }else{
            this.model.CostUOM=this.model.UOMList.find(x=>x.Id === this.model.CostUOMId);
            this.model.StockUOM = this.model.UOMList.find(x=>x.Id === this.model.StockUOMId);
          }        
          this.model.Color =  this.model.ColorList.find(x=>x.Id === this.model.ColorId);
          this.model.ProductCategory = this.model.PCList.find(x=>x.Id === this.model.ProductCategoryId);
          this.model.CreatedON = new Date(this.model.CreatedON);        
          this.model.GreigeItem = this.model.GIList.find(x=>x.Id === this.model.GreigeItemId);
          this.model.isEditMode = false;
          this.model.isAddMode = false;
        
          if(this.model.StatusId === 1){
             this.showDelete = true;
          }else{
             this.showDelete = false;
          }

          if(this.model.StatusId !== 1){
            this.model.StatusList = this.model.StatusList.filter(x=>x.Code !== 'CREATED');
          }else{
            this.model.StatusList = this.model.StatusList.filter(x=>x.Code !== 'DISCONTINUED'); 
          }
          this.show = false;
        },
        (err: HttpErrorResponse) => {
          this._errorService.error(err);
          this.show = false;
        });
    }
  }

  onStatusChange(status: ItemStatus){
    if(status.Code === 'PRODUCTION'){
      this.getSupplierData(this.model.Item.Id);
    }
    if(status){
      this.model.StatusId = status.Id;
    }    
  }

   onCUOMChange(costUOM: UoM){
     if(costUOM){
      this.model.CostUOMId = costUOM.Id;
     }else{
       if(this.model.TypeId === 1 || this.model.TypeId === 2){
        this.model.UOMList = this.model.UOMList.filter(x=>x.Code === 'YD');
        this.model.CostUOM = this.model.UOMList[0];
       }
     }   

  }

  onSUOMChange(stockUOM: UoM){
    if(stockUOM){
      this.model.StockUOMId = stockUOM.Id;
    }else{
      if(this.model.TypeId === 1 || this.model.TypeId === 2){
       this.model.UOMList = this.model.UOMList.filter(x=>x.Code === 'YD');
       this.model.StockUOM = this.model.UOMList[0];
      }
    }   
  }

  onTypeChange(type: ItemType){
    if(type){
      this.model.TypeId = type.Id;
      if(type.Code === 'FABRIC' || type.Code === 'LINING'){
        this.model.UOMList = this.model.UOMList.filter(x=>x.Code === 'YD');
        this.model.CostUOM = this.model.UOMList[0];
        this.model.StockUOM = this.model.UOMList[0];
      }else{
        this.model.UOMList = this.UOMList;
      }
    }
  }

  onGIChange(greigeItem : GreigeItem){
    if(greigeItem){
      this.model.GreigeItemId = greigeItem.Id;
    }
  }

  onColorChange(color: ItemColor){
    if(color){
      this.model.ColorId = color.Id;
    }
  }

  onPCChange(productCategory: ProductCategory){
    if(productCategory){
      this.model.ProductCategoryId= productCategory.Id;
    }
  }

  onSupplierDataClose(isClosed: boolean) {
    this.showSupplier = false;
    this.showWhereUsed = false;
    this.toggleClassshowToggle = this.showWhereUsed ? "active" : "";
    this.toggleClassshowSupplier = this.showSupplier ? "active" : "";
    this.showPopuplayer = false; 
  }

  handleItemFilter(value){
    this.model.ItemList = this.ItemList.filter((s) => (s.Code.toString().toLocaleUpperCase()).startsWith(value.toString().toLocaleUpperCase()));
  }

  handleStatusFilter(value){
      this.model.StatusList = this.ItemStatusList.filter((s) => (s.Code.toString().toLocaleUpperCase()).startsWith(value.toString().toLocaleUpperCase()));
  }

  handleColorFilter(value){
    this.model.ColorList = this.ItemColorList.filter((s) => (s.Name.toString().toLocaleUpperCase()).startsWith(value.toString().toLocaleUpperCase()));
  }

  handleTypeFilter(value){
      this.model.TypeList = this.ItemTypeList.filter((s) => (s.Code.toString().toLocaleUpperCase()).startsWith(value.toString().toLocaleUpperCase()));
  }

  handleCUOMFilter(value){
    this.model.UOMList = this.UOMList.filter((s) => (s.Description.toString().toLocaleUpperCase()).startsWith(value.toString().toLocaleUpperCase()));
  }

  handleSUOMFilter(value){
    this.model.UOMList = this.UOMList.filter((s) => (s.Description.toString().toLocaleUpperCase()).startsWith(value.toString().toLocaleUpperCase()));
  }

  handlePCFilter(value){
    this.model.PCList = this.PCList.filter((s) => (s.Code.toString().toLocaleUpperCase()).startsWith(value.toString().toLocaleUpperCase()));
  }

  handleGIFilter(value){
  this.model.GIList = this.GIList.filter((s) => (s.Code.toString().toLocaleUpperCase()).startsWith(value.toString().toLocaleUpperCase()));
  }

  onAdd() :any{
    this.isItemSelected = false;
    this.displayAdd();
  }

  onEdit() : any{
    this.model.isAddMode  = false;
    this.model.isEditMode = true;
  }

  saveItem() :any{ 
    if(this.previousStatus){
        if( (this.previousStatus.Code === 'CREATED' || this.previousStatus.Code === 'DISCONTINUED') && this.model.Status.Code === 'PRODUCTION'){
          if(this.suppliers.length < 1){
              this._toastService.warn('At least one vendor is required to change the status to Production.');
              this.model.Status = this.previousStatus;
              return;
           }
         }
      } 
    if((!this.model.Code) ||
       (!this.model.Description) || 
       (!this.model.CostUOM)|| 
       (!this.model.StockUOM) ||
       (!this.model.Type) ||
       (!this.model.ProductCategory)|| 
       (!this.model.Color))
      {
        this._toastService.error('Please provide all mandatory Item information.');
        return;
       }

    this._confirmationService.confirm({
      key: 'message',
      value: {
        message: 'Do you want to save the item?',
        continueCallBackFunction: () => this.saveItemConfirm()
      }
    });
   
  }

  saveItemConfirm(){
    this.show = true;
    this.model.Code = this.model.Code.toUpperCase();
    this.model.StatusId = this.model.Status.Id;
    this.model.CostUOMId = this.model.CostUOM.Id;
    this.model.StockUOMId = this.model.StockUOM.Id;
    console.log('Before Persisting');
    console.log(this.model);
    this._itemMaintenance.saveItem(this.model).subscribe(
      data => {
         this.show = false;
         if(this.model.Id){
           this.updateMessage();
           this.model.isAddMode = false;
           this.model.isEditMode = true;
           this.onItemChange(this.model.Item);
         }else{
          this.successMessage();
          this.model.Item = this.ItemList.find(x=>x.Code === this.model.Code);
          this.model.isAddMode = true;
          this.model.isEditMode = false;
          this.onItemChange(this.model.Item);
         }         
         //this.ngOnInit();  
      },
      (err: HttpErrorResponse) => {
        this.show = false;
        this._toastService.error('Error occured while creating new Item. Please retry.');
      }
    );
  }

  removeItemConfirm(){
    this._confirmationService.confirm({
      key: 'message',
      value: {
        message: 'Do you want to delete the item?',
        continueCallBackFunction: () => this.removeItem()
      }
    });
  }

  removeItem() { 
    this.show = true;
    this._itemMaintenance.deleteItem(this.model).subscribe(
      data => {
         this.show = false;
         this.deleteMessage();
         this.ngOnInit();
      },
      (err: HttpErrorResponse) => {
        this._errorService.error(err);
        this.show = false;
      }
    );
  }

  successMessage() {
    this._toastService.success('Item has been created successfully.');
  }

  updateMessage() {
    this._toastService.success('Item has been updated successfully.');
  }

  deleteMessage() {
    this._toastService.success('Item has been deleted successfully.');
  }

  undoItemConfirm() {
    this._confirmationService.confirm({
      key: 'message',
      value: {
        message: 'Do you want to undo the item changes?',
        continueCallBackFunction: () => this.undoItem()
      }
    });
  }

  undoItem(){
    if(this.model.isEditMode){
        this.onItemChange(this.model.Item);
    }
    if(this.model.isAddMode){
        this.displayAdd();
    }
  }

  getSupplierData(ItemId: number) {
    this._itemMaintenance.getSupplerDataByItem(ItemId).subscribe(
      data => {
        this.suppliers= data;
        this.show = false;        
     },
     (err: HttpErrorResponse) => {
       this.show = false;
       this._errorService.error(err);
     }
    );
  }
 
}

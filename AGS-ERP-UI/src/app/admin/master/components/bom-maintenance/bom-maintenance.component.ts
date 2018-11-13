import { Component, OnInit, Input } from '@angular/core';
import { RowClassArgs, GridComponent, GridDataResult } from '@progress/kendo-angular-grid';
import { Align } from '@progress/kendo-angular-popup';
import { BomMaintenanceService } from "../../services/bom-maintenance.service";
import { HttpErrorResponse } from "@angular/common/http";
import { ErrorService } from "../../../../shared/services/error.service";
import { BomMaintenanceSearchDB } from "../../models/BomMaintenanceSearchDB";
import { BomMaintenanceSearch } from "../../models/BomMaintenanceSearch";
import { BomItemType } from "../../models/BomItemType";
import { UniquePipe } from "../../../../shared/pipes/unique.pipe";
import { BomAramarkItem } from "../../models/BomAramarkItem";
import { BomSearchVendor } from "../../models/BomSearchVendor";
import { BomDescription } from "../../models/BomDescription";
import { ToastService } from "../../../../shared/services/toast.service";
import { BomFilteredGridData } from "../../models/BomFilteredData";
import { SortPipe } from "../../../../shared/pipes/sort.pipe"
import { LocalStorageService } from "../../../../shared/wrappers/local-storage.service";
import { BomMaintenanceHeader } from "../../models/BomMaintenanceHeader";
import { BomMaintenanceStyle } from "../../models/BomMaintenanceStyle";
import { BomMaintenanceColor } from "../../models/BomMaintenanceColor";
import { ConfirmationService } from "../../../../shared/services/confirmation.service";
import { BomMaintenanceMfgVendor } from "../../models/BomMaintenanceMfgVendor";
import { BomMaintenanceCopyBom } from "../../models/BomMaintenanceCopyBom";
import { BomVersion } from '../../models/BomVersion';
import { BomMaintenanceStatus } from "../../models/BomMaintenanceStatus";
import { CommonService } from "../../../../shared/services/common.service";
import { SessionStorageService } from "../../../../shared/wrappers/session-storage.service";
import { BomDetail } from "../../models/BomDetail";
import { SearchPipe } from "../../../../shared/pipes/search.pipe";


@Component({
  selector: 'app-bom-maintenance',
  templateUrl: './bom-maintenance.component.html',
  styleUrls: ['./bom-maintenance.component.css'],
  providers: [BomMaintenanceService]
})
export class BomMaintenanceComponent implements OnInit {
  
  toggleClassshowToggle: string = "";
  showCopyBom: boolean = false;
  showPopuplayer: boolean = false;
  opened: boolean = false;
  show : boolean = false;
  DbSearchData : Array<BomMaintenanceSearchDB>;
  SearchData = new BomMaintenanceSearch();
  itemTypesDb : Array<BomItemType>;
  filteredItemTypes : Array<BomItemType>;
  filteredGridData : GridDataResult = {data: [],total : 0}
  searchedFilterGridData : GridDataResult = {data: [],total : 0}
  BOMComponentGriddata : GridDataResult = {data : [],total : 0}
  HeaderFields = new BomMaintenanceHeader();
  removedRecords= 0;
  copyBom = new BomMaintenanceCopyBom();
  ArrayOnItemType : Array<BomMaintenanceSearchDB>;
  sortparams : SortParam;
  constructor(
    private _bomMaintenanceService : BomMaintenanceService,
    private _errorService: ErrorService,
    private _toastService: ToastService,
    private _sortPipe : SortPipe,
    private _confirmationService: ConfirmationService,
    private _commonService: CommonService,
    private _sessionStorage: SessionStorageService,
    private _searchPipe: SearchPipe,

  ) { }

  ngOnInit() {
    this.getSearchData();
    this.getFilteredGridData();
    this.HeaderFields.IsInCreateState = false;
    this.getStyles(false);
    if(this._sessionStorage.get('bom-load')){
      this.HeaderFields = this._sessionStorage.get('bom-header')
      this.BOMComponentGriddata = this._sessionStorage.get('bom-comp-grid')
      this._sessionStorage.removeAll(['bom-load','bom-header','bom-comp-grid'])
    }
    this.getStatus();
  }

  getStatus(){
    this.HeaderFields.MainStatusArray = this.HeaderFields.Status= [new BomMaintenanceStatus(1,'C','Created'),new BomMaintenanceStatus(2,'P','Production'),new BomMaintenanceStatus(3,'O','Obsolete')]
  }

  CreateBOM(){
    this.HeaderFields = new BomMaintenanceHeader();
    this.SearchData = new BomMaintenanceSearch();
    this.searchedFilterGridData = {data: [],total : 0};
    this.BOMComponentGriddata = {data: [],total : 0};
    this.HeaderFields.IsInCreateState = true;
    this.getStatus();
    this.HeaderFields.Status = this.HeaderFields.MainStatusArray;
    this.HeaderFields.SelectedStatus = this.HeaderFields.MainStatusArray[0];
    this.getStyles(this.HeaderFields.IsInCreateState);
    this.getSearchData();
    this.getFilteredGridData();
  }
 
  public onCloseallPopup(): void {
    if(this.isCopyBomHasChanged()){
      this._confirmationService.confirm({
        key: 'message',
        value: {
          message: 'Are you sure you want to discard the changes?',
          continueCallBackFunction: () => this.cancelCopyBom()
        }
      });
    }
    else{
      this.cancelCopyBom();
    }
  }

  public isCopyBomHasChanged(){
    if(this.copyBom.FromBom.SelectedStyle || this.copyBom.FromBom.SelectedColor || this.copyBom.FromBom.SelectedMfgVendor ||
      this.copyBom.ToBom.SelectedStyle || this.copyBom.ToBom.SelectedColor || this.copyBom.ToBom.SelectedMfgVendor){
        return true;
    }
    else{
      return false;
    }
  }

  public cancelCopyBom(){
    this.showCopyBom = false;
    this.toggleClassshowToggle = this.showCopyBom ? "active" : "";
    this.showPopuplayer = false;
    this.copyBom = new BomMaintenanceCopyBom();
  }

  public onTogglePopupData(type : string, status : boolean): void {
    if(type === "CopyBom"){
      this.showCopyBom = !this.showCopyBom;
      this.showPopuplayer = status;
      this.toggleClassshowToggle = status ? "active" : "";
    }
  }

   // Close the transfer popup
   close() {
    this.opened = false;
  }

  getFilteredGridData(){
    this.show = true;
    this._bomMaintenanceService.getFilteredGridData().subscribe(
      data => {
        this.filteredGridData.data = data;
        this.show = false;
      },
      (err: HttpErrorResponse) => {
        this._errorService.error(err);
        this.show = false;
      })
  }

  getSearchData(){
    this.show = true;
    this._bomMaintenanceService.getSearchData().subscribe( 
      data =>{
        this.DbSearchData = data;
        this.SearchData.ItemTypeList = this.SearchData.FilteredItemTypes = this.DbSearchData.map(el => new BomItemType(el.ItemTypeId,el.ItemTypeCode,el.ItemTypeName));
        this.SearchData.AramarkItemList =  this.DbSearchData.map(el => new BomAramarkItem(el.ItemId,el.ItemCode,el.ItemDescritpion));
        this.SearchData.DescriptionList =  this.DbSearchData.map(el => new BomDescription(el.ItemId,el.ItemCode,el.ItemDescritpion));
        this.SearchData.VendorList = this.SearchData.FilteredVendors = this.DbSearchData.map(el => new BomSearchVendor(el.VendorId,el.VendorCode,el.VendorName));
        this.show = false;
      },
      (err: HttpErrorResponse) => {
        this._errorService.error(err);
        this.show = false;
      });
  }

onItemTypeChange(selectedItemType : BomItemType){
    if(selectedItemType){
      this.SearchData.FilteredAramarkItems = this.SearchData.FilteredDescriptions = this.SearchData.FilteredVendors = [];
      this.SearchData.SelectedAramarkItem = this.SearchData.SelectedDescription = this.SearchData.SelectedVendor = null;
      this.DbSearchData.map(el => {
        if(el.ItemTypeId === this.SearchData.SelectedItemType.ItemTypeId){
          this.SearchData.FilteredAramarkItems.push( new BomAramarkItem(el.ItemId,el.ItemCode,el.ItemDescritpion));
          this.SearchData.FilteredDescriptions.push( new BomDescription(el.ItemId,el.ItemCode,el.ItemDescritpion));
          this.SearchData.FilteredVendors.push( new BomSearchVendor(el.VendorId,el.VendorCode,el.VendorName));
        }
      });
      if(this._sortPipe.removeDuplicates(this.SearchData.FilteredAramarkItems,'ItemId').length === 1){
        this.SearchData.SelectedAramarkItem = this.SearchData.FilteredAramarkItems.length[0];
      }
      if(this._sortPipe.removeDuplicates(this.SearchData.FilteredDescriptions,'ItemDescription').length === 1){
        this.SearchData.SelectedDescription = this.SearchData.FilteredDescriptions.length[0];
      }
      if(this._sortPipe.removeDuplicates(this.SearchData.FilteredVendors,'VendorId').length === 1){
        this.SearchData.SelectedVendor = this.SearchData.FilteredVendors.length[0];
      }
    }
    else{
     this.clearSearchFileds();
    }
  }

  onAramarkItemChange(selectedAramarkItem : BomAramarkItem){
    if(this.SearchData.SelectedAramarkItem){
      this.SearchData.FilteredDescriptions = this.SearchData.FilteredVendors = [];
      this.DbSearchData.map(el => {
        if(el.ItemTypeId === this.SearchData.SelectedItemType.ItemTypeId && el.ItemId === this.SearchData.SelectedAramarkItem.ItemId){
          this.SearchData.FilteredDescriptions.push(new BomDescription(el.ItemId,el.ItemCode,el.ItemDescritpion));
          this.SearchData.FilteredVendors.push( new BomSearchVendor(el.VendorId,el.VendorCode,el.VendorName));
        }
      })
      if(this._sortPipe.removeDuplicates(this.SearchData.FilteredDescriptions,'ItemDescription').length === 1){
        this.SearchData.SelectedDescription = this.SearchData.FilteredDescriptions[0];
      }
      if(this._sortPipe.removeDuplicates(this.SearchData.FilteredVendors,'VendorId').length === 1){
        this.SearchData.SelectedVendor = this.SearchData.FilteredVendors[0];
      }
    }
    else{
      this.SearchData.SelectedDescription = this.SearchData.SelectedVendor = null;
      this.SearchData.FilteredDescriptions = this.SearchData.DescriptionList;
      this.SearchData.FilteredVendors = this.SearchData.VendorList;
    }
  }
 
  onDescriptionChange(selectedDescription : BomDescription){
    if(this.SearchData.SelectedDescription){
      this.SearchData.FilteredAramarkItems = this.SearchData.FilteredVendors = [];
      this.DbSearchData.map(el => {
        if(el.ItemDescritpion.toLowerCase() === this.SearchData.SelectedDescription.ItemDescription.toLowerCase() && el.ItemTypeId === this.SearchData.SelectedItemType.ItemTypeId){
          this.SearchData.FilteredAramarkItems.push(new BomAramarkItem(el.ItemId,el.ItemCode,el.ItemDescritpion));
          this.SearchData.FilteredVendors.push( new BomSearchVendor(el.VendorId,el.VendorCode,el.VendorName));
        }
      });
      if(this._sortPipe.removeDuplicates(this.SearchData.FilteredAramarkItems,'ItemId').length === 1){
        this.SearchData.SelectedAramarkItem = this.SearchData.FilteredAramarkItems[0];
      }
      if(this._sortPipe.removeDuplicates(this.SearchData.FilteredVendors,'VendorId').length === 1){
        this.SearchData.SelectedVendor = this.SearchData.FilteredVendors[0];
      }
    }
    else{
      this.SearchData.SelectedAramarkItem = this.SearchData.SelectedVendor = null;
      this.SearchData.FilteredAramarkItems = this.SearchData.AramarkItemList;;
      this.SearchData.FilteredVendors = this.SearchData.VendorList;
    }
  }

  onSearchVendorChange(selectedVendor : BomSearchVendor){
    if(selectedVendor){
      this.SearchData.FilteredAramarkItems = this.SearchData.FilteredDescriptions =[];
      this.DbSearchData.map(el => {
        if(el.VendorId === this.SearchData.SelectedVendor.VendorId && el.ItemTypeId === this.SearchData.SelectedItemType.ItemTypeId){
          this.SearchData.FilteredAramarkItems.push(new BomAramarkItem(el.ItemId,el.ItemCode,el.ItemDescritpion));
          this.SearchData.FilteredDescriptions.push(new BomDescription(el.ItemId,el.ItemCode,el.ItemDescritpion));
        }
      });
      if(this._sortPipe.removeDuplicates(this.SearchData.FilteredAramarkItems,'ItemId').length === 1){
        this.SearchData.SelectedAramarkItem = this.SearchData.FilteredAramarkItems[0];
      }
      if(this._sortPipe.removeDuplicates(this.SearchData.FilteredDescriptions,'ItemDescription').length === 1){
        this.SearchData.SelectedDescription = this.SearchData.FilteredDescriptions[0];
      }
    }
    else{
      this.SearchData.SelectedAramarkItem = this.SearchData.SelectedDescription = null;
      this.SearchData.FilteredAramarkItems = this.SearchData.AramarkItemList;
      this.SearchData.FilteredDescriptions = this.SearchData.DescriptionList;
    }
  }

  validateSearchForm(){
    let errorInfo: Array<string> = [];
    let rollError: string;
    if (!this.SearchData.SelectedItemType) {
      errorInfo.push("Item Type");
    }
    if (!this.SearchData.SelectedVendor) {
      errorInfo.push("Supplier");
    }
    rollError = errorInfo.join(", ");
    if(rollError){
      this._toastService.error("Kindly enter the required field(s):  " + rollError);
    }
    else{
      this.filterGridData();
    }
  }

  filterGridData(){
    this.searchedFilterGridData.data = []
    let data = this.filteredGridData.data.filter(r => r.TypeId === this.SearchData.SelectedItemType.ItemTypeId);
    data.forEach(element => {
      if(this.BOMComponentGriddata.data.find(e => e.ItemCode === element.ItemCode)){
        element.IsInBOMCompGrid = true;
      }
      this.searchedFilterGridData.data.push(element);
    });
    if(this.SearchData.SelectedAramarkItem){
      this.searchedFilterGridData.data = this.searchedFilterGridData.data.filter(r => r.ItemId === this.SearchData.SelectedAramarkItem.ItemId);
    }
    if(this.SearchData.SelectedDescription){
      this.searchedFilterGridData.data = this.searchedFilterGridData.data.filter(r => r.Description.toLowerCase() === this.SearchData.SelectedDescription.ItemDescription.toLowerCase());
    }
  }

  clearSearchFileds(){
    this.SearchData.SelectedItemType = this.SearchData.SelectedAramarkItem = this.SearchData.SelectedDescription = null;
    this.SearchData.SelectedVendor = null;
    this.SearchData.FilteredAramarkItems = this.SearchData.FilteredDescriptions = this.SearchData.FilteredVendors = [];
  }

  populateBomCompGrid(comp){
    comp.IsInBOMCompGrid = true;
    comp.WasteFactor = '0';
    comp.AvgQty = null;
    comp.IsAdded = true;
    this.BOMComponentGriddata.data.push(comp);
  }

  getStyles(isNewBom : boolean){
    this.show = true;
    this._bomMaintenanceService.getStyles(isNewBom).subscribe(
      data => {
        this.HeaderFields.FilteredStyles = this.HeaderFields.Styles = data;
        if(this.HeaderFields.FilteredStyles.length === 1){
          this.HeaderFields.SelectedStyle = this.HeaderFields.FilteredStyles[0];
          this.getColors(this.HeaderFields.SelectedStyle);
        }
        this.show = false;
      },
      (err: HttpErrorResponse) => {
        this._errorService.error(err);
        this.show = false;
      }
    )
  }

  onStyleChangeConfirmation(currentStyle : BomMaintenanceStyle, prevStyle : BomMaintenanceStyle){
    if(this.HeaderFields.IsSpecChanged || this.HeaderFields.IsModelChanged || this.HeaderFields.IsStatusChanged || this.HeaderFields.IsComponentsChanged){
      this._confirmationService.confirm({
        key: 'message',
        value: {
          message: 'BOM has unsaved data. Do you want to discard the changes?',
          continueCallBackFunction: () => this.onStyleChange(currentStyle),
          cancelCallBackFunction: () => this.cancelStyleChange(prevStyle)
        }
      });
    }
    else{
      this.onStyleChange(currentStyle);
    }
  }

  cancelStyleChange(prevStyle : BomMaintenanceStyle){
    this.HeaderFields.SelectedStyle = new BomMaintenanceStyle(prevStyle.Id,prevStyle.Code,prevStyle.Description);
  }

  onStyleChange(currentStyle : BomMaintenanceStyle){
    this.HeaderFields.SelectedStyle = currentStyle;
    // this.HeaderFields.SelectedColor = this.HeaderFields.SelectedVendor = this.HeaderFields.SelectedSpec = this.HeaderFields.selectedVersion = this.HeaderFields.SelectedStatus = null;
    this.HeaderFields.Vendors = this.HeaderFields.FilteredVendors = [];
    this.HeaderFields.SelectedVendor = null;
    this.HeaderFields.Model = '';
    this.searchedFilterGridData.data = []
    this.BOMComponentGriddata.data = []
    if(!currentStyle){
      this.ngOnInit();
    }
    else{
      this.getColors(currentStyle);
    }
  }

  isBomHasChanges(){
    if(this.HeaderFields.IsSpecChanged || this.HeaderFields.IsModelChanged || this.HeaderFields.IsStatusChanged || this.HeaderFields.IsComponentsChanged){
      return true;
    }
    else{
      return false;
    }
  }

  getColors(selectedStyle : BomMaintenanceStyle){
    this.show = true;
    this._bomMaintenanceService.getColorsByStyleId(selectedStyle.Id,this.HeaderFields.IsInCreateState).subscribe(
      data => {
        this.HeaderFields.FilteredColors= this.HeaderFields.Colors = data;
        if(this.HeaderFields.FilteredColors.length === 1){
          this.HeaderFields.SelectedColor = this.HeaderFields.FilteredColors[0];
          this.getMfgVendors(this.HeaderFields.SelectedColor)
        }
        this.show = false;
      },
      (err: HttpErrorResponse) => {
        this._errorService.error(err);
        this.show = false;
      })
  }

  confirmationOnColorChange(currColor : BomMaintenanceColor, prevColor : BomMaintenanceColor){
    if(this.isBomHasChanges()){
      this._confirmationService.confirm({
        key: 'message',
        value: {
          message: 'BOM has unsaved data. Do you want to discard the changes?',
          continueCallBackFunction: () => this.getMfgVendors(currColor),
          cancelCallBackFunction: () => this.cancelColorChange(prevColor)
        }
      });
    }
    else{
      this.getMfgVendors(currColor);
    }
  }

  cancelColorChange(prevColor : BomMaintenanceColor){
    this.HeaderFields.SelectedColor = new BomMaintenanceColor(prevColor.Id,prevColor.Code,prevColor.Description);
  }

  getMfgVendors(selectedColor : BomMaintenanceColor){
    this.HeaderFields.SelectedColor = selectedColor;
    // this.HeaderFields.SelectedVendor = this.HeaderFields.SelectedSpec = this.HeaderFields.SelectedStatus = this.HeaderFields.selectedVersion = null;
    this.HeaderFields.Model = '';
    this.searchedFilterGridData.data = []
    this.BOMComponentGriddata.data = [];
    this.show = true;
    if(selectedColor){
    this._bomMaintenanceService.getMfgVendorsByColorId(this.HeaderFields.SelectedStyle.Id, selectedColor.Id,this.HeaderFields.IsInCreateState).subscribe(
      data => {
        this.HeaderFields.FilteredVendors = this.HeaderFields.Vendors = data;
        if(this.HeaderFields.FilteredVendors.length === 1){
          this.HeaderFields.SelectedVendor = this.HeaderFields.FilteredVendors[0];
          this.onMfgVendorChange(this.HeaderFields.SelectedVendor)
        }
        this.show = false;
      },
      (err: HttpErrorResponse) => {
        this._errorService.error(err);
        this.show = false;
      })
    }
  }

  mfgVendorChangeConfirmation(currentVendor : BomMaintenanceMfgVendor , prevVendor : BomMaintenanceMfgVendor){
    if(this.isBomHasChanges()){
      this._confirmationService.confirm({
        key: 'message',
        value: {
          message: 'BOM has unsaved data. Do you want to discard the changes?',
          continueCallBackFunction: () => this.onMfgVendorChange(currentVendor),
          cancelCallBackFunction: () => this.cancelVendorChage(prevVendor)
        }
      });
    }
    else{
      this.onMfgVendorChange(currentVendor);
    }
  }

  cancelVendorChage(prevVendor : BomMaintenanceMfgVendor){
    this.HeaderFields.SelectedVendor = new BomMaintenanceMfgVendor(prevVendor.Id,prevVendor.Code,prevVendor.Name);
  }

  onMfgVendorChange(currentVendor : BomMaintenanceMfgVendor){
    this.show = true;
    this.HeaderFields.SelectedVendor = currentVendor;
    this.searchedFilterGridData.data = [];
    this.BOMComponentGriddata.data = [];
    this.HeaderFields.IsSpecChanged = this.HeaderFields.IsModelChanged =  this.HeaderFields.IsStatusChanged = false;
    this._bomMaintenanceService.getSpecList(this.HeaderFields.SelectedStyle,this.HeaderFields.SelectedColor).subscribe(
      data =>{
        this.HeaderFields.Specs = this.HeaderFields.FilteredSpecs = data;
        this.show = false;
      },
      (err: HttpErrorResponse) => {
        this._errorService.error(err);
        this.show = false;
      }
    )
    if(this.HeaderFields.IsInCreateState){
      this.HeaderFields.Versions = this.HeaderFields.filteredVersions = [new BomVersion(0,0)]
      this.HeaderFields.selectedVersion = this.HeaderFields.Versions[0];
      this.HeaderFields.SelectedStatus = new BomMaintenanceStatus(1,'C','Created');
    }
    if(!this.HeaderFields.IsInCreateState){
      this.getSavedBomHeaderData(this.HeaderFields.SelectedStyle,this.HeaderFields.SelectedColor,this.HeaderFields.SelectedVendor,false)
    }
  }

  getSavedBomHeaderData(selectedStyle, selectedColor, selectedVendor,isCopy : boolean){
    this.show = true;
    this._bomMaintenanceService.getBomHeaderData(selectedStyle,selectedColor,selectedVendor).subscribe(
      data =>{
        //this.HeaderFields.SelectedSpec = this.HeaderFields.Specs.find(c => c.Id === data.SpecId);
        if(data){
          this.HeaderFields.Id = data.Id;
          this.HeaderFields.Model = data.Model;
          this.HeaderFields.SelectedStatus = this.HeaderFields.MainStatusArray.find(c => c.Id === data.StatusId);
          this.HeaderFields.Status = this.handleStatusDropdown(false);
          this.HeaderFields.Versions = this.HeaderFields.filteredVersions = data.Versions;
          this.HeaderFields.selectedVersion = this.HeaderFields.Versions[this.HeaderFields.Versions.length - 1];
          this.onVersionChange(this.HeaderFields.selectedVersion,isCopy);
        }
        this.show = false;
      },
      (err: HttpErrorResponse) => {
        this._errorService.error(err);
        this.show = false;
      }
    )
  }



  calculateAvgQtyWithWaste(item : BomFilteredGridData){
    item.IsModified = true;
    if(item.AverageQuantity && item.WasteFactor){
      item.AverageQuantityWithWaste =  (Number(item.AverageQuantity.replace(/,/g , "")) + (Number(item.AverageQuantity.replace(/,/g , "")) * (Number(item.WasteFactor.toString().replace(/,/g, ""))/100)))
    }
    else{
      item.AverageQuantityWithWaste = null;
    }
  } 

  confirmRemovalFromBomCompGri(item : BomFilteredGridData){
     this._confirmationService.confirm({
      key: 'message',
      value: {
        message: 'Are you sure you want to remove the record from BOM Components?',
        continueCallBackFunction: () => this.removeFromBomCompGrid(item)
      }
    });
  }

  removeFromBomCompGrid(item : BomFilteredGridData){
    this.BOMComponentGriddata.data.splice(this.BOMComponentGriddata.data.indexOf(item),1);
    var removedItem = this.searchedFilterGridData.data.find(r => item.ItemId === r.ItemId);
    if(removedItem){
      removedItem.IsInBOMCompGrid = false
    }
    this.removedRecords ++;
  }

  disabledForm(){
    if(this.HeaderFields.SelectedStyle && this.HeaderFields.SelectedColor && this.HeaderFields.SelectedVendor){
      return false;
    }
    else{
      return true;
    }
  }

  openStyleOverride(){
    this.opened = true;
  }
  ModelChanged(){
    this.HeaderFields.IsModelChanged = true;
  }

  handleStyleFilter(value : string){
    this.HeaderFields.FilteredStyles = this.HeaderFields.Styles.filter((s) => (s.Code.toUpperCase().startsWith(value.toUpperCase())));
  }

  handleColorFilter(value : string){
    this.HeaderFields.FilteredColors = this.HeaderFields.Colors.filter((s) => (s.Code.toUpperCase().startsWith(value.toUpperCase())));
  }
  handleMfgVendorFilter(value : string){
    this.HeaderFields.FilteredVendors = this.HeaderFields.Vendors.filter((s) => (s.Code.toUpperCase().startsWith(value.toUpperCase())));
  }

  handleCopyFromStyleFilter(value : string){
    this.copyBom.FromBom.FilteredStyles = this.copyBom.FromBom.Styles.filter((s) => (s.Code.toUpperCase().startsWith(value.toUpperCase())));
  }

  handleCopyFromColorFilter(value : string){
    this.copyBom.FromBom.FilteredColors = this.copyBom.FromBom.Colors.filter((s) => (s.Code.toUpperCase().startsWith(value.toUpperCase())));
  }

  handleCopyFromMfgVendorFilter(value : string){
    this.copyBom.FromBom.FilteredMfgVendors = this.copyBom.FromBom.MfgVendors.filter((s) => (s.Code.toUpperCase().startsWith(value.toUpperCase())));
  }

  handleCopyToStyleFilter(value : string){
    this.copyBom.ToBom.FilteredStyles = this.copyBom.ToBom.Styles.filter((s) => (s.Code.toUpperCase().startsWith(value.toUpperCase())));
  }

  handleCopyToColorFilter(value : string){
    this.copyBom.ToBom.FilteredColors = this.copyBom.ToBom.Colors.filter((s) => (s.Code.toUpperCase().startsWith(value.toUpperCase())));
  }

  handleCopyToMfgVendorFilter(value : string){
    this.copyBom.ToBom.FilteredMfgVendors = this.copyBom.ToBom.MfgVendors.filter((s) => (s.Code.toUpperCase().startsWith(value.toUpperCase())));
  }

  versionChangeConfirmation(currVersion : BomVersion, prevVersion : BomVersion){
      if(this.isBomHasChanges() || this.BOMComponentGriddata.data.filter(r => r.IsAdded || r.IsModified).length > 0 || this.removedRecords > 0){
        this._confirmationService.confirm({
        key: 'message',
        value: {
          message: 'Changing Version will not save the unsaved data. Are you sure you want to change the version?',
          continueCallBackFunction: () => this.onVersionChange(currVersion,false),
          cancelCallBackFunction: () => this.cancelVersionChange(prevVersion)
        }
      });
    }
    else{
      this.onVersionChange(currVersion,false)
    }
  }

  cancelVersionChange(prevVersion : BomVersion){
    this.HeaderFields.selectedVersion = new BomVersion(prevVersion.BomHdrId, prevVersion.Version);
  }

  onVersionChange(currVersion : BomVersion,isCopy : boolean) {
    //get BOM comp grid data, size oveeride data
    this.show = true;
    this.HeaderFields.selectedVersion = new BomVersion(currVersion.BomHdrId, currVersion.Version);
    this.HeaderFields.EditBom = false;
    if(this.HeaderFields.selectedVersion.Version !== this.HeaderFields.Versions[this.HeaderFields.Versions.length-1].Version){
      this.HeaderFields.IsOldVersionSelected = true;
    }
    else{
      this.HeaderFields.IsOldVersionSelected = false;
    }
    this._bomMaintenanceService.getBomComponentGridData(currVersion.BomHdrId).subscribe(
      data =>{
        this.BOMComponentGriddata.data = [];
        data.forEach(element => {
          element.AverageQuantity =  Number(element.AverageQuantity).toLocaleString();
          element.WasteFactor =  Number(element.WasteFactor).toLocaleString();
          this.BOMComponentGriddata.data.push(element);
        });
        this.removedRecords = 0;
        if(isCopy){
          this.saveBom(isCopy);
          this.cancelCopyBom();
        }
        this.show = false;
      },
      (err: HttpErrorResponse) => {
        this._errorService.error(err);
        this.show = false;
      })
  }

  editBOM(){
    this.HeaderFields.EditBom = true;
  }

  getCopyBomDetails(){
    this.show = true;
    this._bomMaintenanceService.getStyles(false).subscribe(
      data =>{
        this.copyBom.FromBom.Styles = this.copyBom.FromBom.FilteredStyles = data;
        this.show = false;
      },
      (err: HttpErrorResponse) => {
        this._errorService.error(err);
        this.show = false;
      })
    this._bomMaintenanceService.getStyles(true).subscribe(
      data =>{
        this.copyBom.ToBom.Styles = this.copyBom.ToBom.FilteredStyles = data;
        this.show = false;
      },
      (err: HttpErrorResponse) => {
        this._errorService.error(err);
        this.show = false;
      })
  }

  onCopyStyleChange(type : string){
    let isInCreateState = false;
    this.show = true;
    let bom  = this.copyBom.FromBom;
    if(type === 'To'){
      isInCreateState = true;
      bom = this.copyBom.ToBom
    }
    this._bomMaintenanceService.getColorsByStyleId(bom.SelectedStyle.Id,isInCreateState).subscribe(
      data =>{
        if(type === 'From'){
            this.copyBom.FromBom.Colors = this.copyBom.FromBom.FilteredColors = data;
            if(data.length === 1){
              this.copyBom.FromBom.SelectedColor = this.copyBom.FromBom.Colors[0];
              this.onCopyColorChange(this.copyBom.FromBom.SelectedColor,'From');
            }
        }
        if(type === 'To'){
            this.copyBom.ToBom.Colors = this.copyBom.ToBom.FilteredColors = data;
             if(data.length === 1){
              this.copyBom.ToBom.SelectedColor = this.copyBom.ToBom.Colors[0];
              this.onCopyColorChange(this.copyBom.ToBom.SelectedColor,'To');
            }
        }
        this.show = false;
      },
      (err: HttpErrorResponse) => {
        this._errorService.error(err);
        this.show = false;
      })
  }

  onCopyColorChange(selectedColor : BomMaintenanceColor,type : string){
    this.show = true;
    let isInCreateState = false;
    let bom  = this.copyBom.FromBom;
    if(type === 'To'){
      isInCreateState = true;
      bom  = this.copyBom.ToBom;
    }
    this._bomMaintenanceService.getMfgVendorsByColorId(bom.SelectedStyle.Id, selectedColor.Id,isInCreateState).subscribe(
      data =>{
        if(type === 'From'){
          this.copyBom.FromBom.MfgVendors = this.copyBom.FromBom.FilteredMfgVendors = data;
          if(data.length === 1){
            this.copyBom.FromBom.SelectedMfgVendor = this.copyBom.FromBom.MfgVendors[0];
          }
        }
        if(type === 'To'){
            this.copyBom.ToBom.MfgVendors = this.copyBom.ToBom.FilteredMfgVendors = data;
            if(data.length === 1){
            this.copyBom.ToBom.SelectedMfgVendor = this.copyBom.ToBom.MfgVendors[0];
          }
        }
        this.show = false;
      },
      (err: HttpErrorResponse) => {
        this._errorService.error(err);
        this.show = false;
      })
  }

  copyBomConfirmation(){
    this._confirmationService.confirm({
        key: 'message',
        value: {
          message: 'Do you want to Copy the BOM?',
          continueCallBackFunction: () => this.copySelectedBom()
        }
      });
  }

  copySelectedBom(){
    this.HeaderFields.IsInCreateState = true;
    this.HeaderFields.SelectedStyle = this.copyBom.ToBom.SelectedStyle;
    this.HeaderFields.SelectedColor = this.copyBom.ToBom.SelectedColor;
    this.HeaderFields.SelectedVendor = this.copyBom.ToBom.SelectedMfgVendor;
    //get header data for 'FROM' bom
    this.getSavedBomHeaderData(this.copyBom.FromBom.SelectedStyle,this.copyBom.FromBom.SelectedColor,this.copyBom.FromBom.SelectedMfgVendor,true);
    if(this.HeaderFields.IsInCreateState){
      this.HeaderFields.Versions = this.HeaderFields.filteredVersions = [new BomVersion(0,0)];
      this.HeaderFields.Model = ''
      this.HeaderFields.SelectedStatus = new BomMaintenanceStatus(1,'C','Created')
    }
    
  }

  handleStatusDropdown(isChanged : boolean){
    if(this.HeaderFields.SelectedStatus){
      if(isChanged){
        this.HeaderFields.IsStatusChanged = true;
      }
      if(!isChanged){
        if(this.HeaderFields.SelectedStatus.Code === 'C'){
           return this.HeaderFields.Status = [new BomMaintenanceStatus(1,'C','Created'),new BomMaintenanceStatus(2,'P','Production')]
          // return this.HeaderFields.Status = this.HeaderFields.MainStatusArray;
        }
        if(this.HeaderFields.SelectedStatus.Code === 'P'){
           return this.HeaderFields.Status = [new BomMaintenanceStatus(2,'P','Production'),new BomMaintenanceStatus(3,'O','Obselete')]
        }
        if(this.HeaderFields.SelectedStatus.Code === 'O'){
          return this.HeaderFields.Status = [new BomMaintenanceStatus(2,'P','Production'),new BomMaintenanceStatus(3,'O','Obselete')]
        }
      }
    }
  }

  onSpecChange(spec){
    this.HeaderFields.IsSpecChanged = true;
  }

  post(itemCode : string) {
    console.log('ITEMCODE in BOM-->'+itemCode);
    this._sessionStorage.add('bom-load', true);
    this._sessionStorage.add('bom-header', this.HeaderFields);
    this._sessionStorage.add('bom-comp-grid', this.BOMComponentGriddata);
    this.show = true;
    this._commonService.Notify({
      key: 'RMItem',
      value: { 'ItemCode' :  itemCode}
    });
    setTimeout(() => {
      this._commonService.Notify({
        key: 'RMItem',
        value: { 'ItemCode' :  itemCode}
      });
      this.show = false;
    }, 1);
   
  }

  saveConfirm(){
    let rollError = this.validateForSave();
     if(rollError){
      this._toastService.error("Kindly enter the required field(s):  " + rollError);
    }
    else{
      this._confirmationService.confirm({
        key: 'message',
        value: {
          message: 'Are you sure you want to save the BOM?',
          continueCallBackFunction: () => this.saveBom(false)
        }
      });
    }
  }

  saveBom(isCopy : boolean){
    this.show = true;
    let selectedStyle = this.HeaderFields.SelectedStyle;
    let selectedColor = this.HeaderFields.SelectedColor;
    let selectedVendor = this.HeaderFields.SelectedVendor;
    let saveObject = new BomDetail();
    saveObject.BOMHeader.StyleId = this.HeaderFields.SelectedStyle.Id;
    saveObject.BOMHeader.ColorId = this.HeaderFields.SelectedColor.Id;
    saveObject.BOMHeader.VendorId = this.HeaderFields.SelectedVendor.Id;
    saveObject.BOMHeader.SpecId = this.HeaderFields.SelectedSpec ? this.HeaderFields.SelectedSpec.Id : 0;
    saveObject.BOMHeader.Id = this.HeaderFields.Id;
    saveObject.BOMHeader.Model = this.HeaderFields.Model;
    saveObject.BOMHeader.StatusId = this.HeaderFields.SelectedStatus.Id;
    if(isCopy){
      saveObject.BOMHeader.Versions = [new BomVersion(0,0)]
      saveObject.BOMHeader.Id = 0;
      saveObject.BOMHeader.StatusId = 1;
    }
    else{
      saveObject.BOMHeader.Versions = this.HeaderFields.Versions;
    }
    saveObject.BomComponents = this.BOMComponentGriddata.data;
    saveObject.BomComponents.forEach(element => {
      if(element.IsAdded || isCopy){
        element.Id = 0;
      }
    });
    this._bomMaintenanceService.saveBOM(saveObject).subscribe(
      data => {
        this._toastService.success("BOM saved successfully")
        this.HeaderFields = new BomMaintenanceHeader();
        this.SearchData =new BomMaintenanceSearch();
        this.BOMComponentGriddata.data = []
        this.filteredGridData.data = this.searchedFilterGridData.data = []
        this.copyBom = new BomMaintenanceCopyBom();
        this.show = false;
        this.ngOnInit();
        this.HeaderFields.SelectedStyle = selectedStyle;
        this.onStyleChange(selectedStyle);
        this.HeaderFields.SelectedColor = selectedColor;
        this.getMfgVendors(selectedColor);
        this.HeaderFields.SelectedVendor = selectedVendor;
        this.onMfgVendorChange(selectedVendor);
      },
      (err: HttpErrorResponse) => {
        this._errorService.error(err);
        this.show = false;
      })
  }

  deleteConfirm(){
    this._confirmationService.confirm({
      key: 'message',
      value: {
        message: 'Are you sure you want to Delete the BOM?',
        continueCallBackFunction: () => this.deleteBOM()
      }
    });
  }

  deleteBOM(){
    this.show = true;
    let bomHeader = new BomMaintenanceHeader();
    bomHeader.Id = this.HeaderFields.Id;
    this._bomMaintenanceService.deleteBOM(bomHeader).subscribe(
      data => {
        this._toastService.success("BOM deleted successfully");
        this.HeaderFields = new BomMaintenanceHeader();
        this.SearchData =new BomMaintenanceSearch();
        this.BOMComponentGriddata.data = []
        this.filteredGridData.data = this.searchedFilterGridData.data = []
        this.copyBom = new BomMaintenanceCopyBom();
        this.show = false;
        this.ngOnInit();
      },
      (err: HttpErrorResponse) => {
        this._errorService.error(err);
        this.show = false;
      });
  }

   validateForSave(){
    let errorInfo: Array<string> = [];
    let rollError: string;
    if (!this.HeaderFields.SelectedStyle) {
      errorInfo.push("Style");
    }
    if (!this.HeaderFields.SelectedColor) {
      errorInfo.push("Color");
    }
    if (!this.HeaderFields.SelectedVendor) {
      errorInfo.push("Vendor");
    }
    // if (!this.HeaderFields.Specs) {
    //   errorInfo.push("Spec");
    // }
    if(this.HeaderFields.SelectedStatus.Code === 'P' && this.BOMComponentGriddata.data.length === 0){
      errorInfo.push("There should be at least one component when BOM is in Production");
    }
    if(this.BOMComponentGriddata.data.length > 0){
      if(this.BOMComponentGriddata.data.filter(r => (r.AverageQuantity === undefined) || (r.AverageQuantity == 0)).length > 0){
         errorInfo.push("Average Quantity for all BOM Components and should be greater than 0");
      }
      if(this.BOMComponentGriddata.data.filter(r => !r.WasteFactor.toString()).length > 0){
         errorInfo.push("Waste factor for all BOM Components");
      }
    }
    rollError = errorInfo.join(", ");
    return rollError;
  }

  disableOverride(component : BomFilteredGridData){
    if(component.TypeName === 'FABRIC'){
      return false
    }
    return true;
  }

  disableFieldsBeforeEdit(){
    let disable = true;
    if(this.HeaderFields.IsInCreateState === false && this.disabledForm() === false){
      if(this.HeaderFields.EditBom && !this.HeaderFields.IsOldVersionSelected){
        disable = false;
      }
    }
    else if(this.HeaderFields.IsInCreateState === true){
      disable = this.disabledForm();
    }
    return disable;
  }

  disableVersion(){
    let disable = false;
    if(this.HeaderFields.IsInCreateState === true){
      disable = true;
    }
    if(!this.HeaderFields.EditBom){
      disable = true;
    }
    return disable;
  }

  disableSave(){
    if( this.isBomHasChanges() || this.BOMComponentGriddata.data.filter(r => r.IsAdded || r.IsModified).length > 0 || this.removedRecords > 0 || (this.HeaderFields.IsInCreateState && (this.disabledForm() === false))){
      return false;
    }
    return true;
  }

  disableDelete(){
    let statusCode = ''
    if(this.HeaderFields.SelectedStatus){
      statusCode = this.HeaderFields.SelectedStatus.Code;
    }
    if(this.disabledForm() || this.HeaderFields.IsInCreateState || statusCode === 'P' || statusCode === 'O'){
      return true;
    }
    return false
  }

  transform(items: Array<any>, filter: {[key: string]: any }): Array<any> {
        return items.filter(item => {
                let notMatchingField = Object.keys(filter)
                                             .find(key =>item[key] !== filter[key] && filter[key] != null);
                return !notMatchingField; // true if matches all fields
        });
    }

  populateSearchDropdowns(selectedVal : string){
    this.sortparams = new SortParam(this.SearchData.SelectedAramarkItem ? this.SearchData.SelectedAramarkItem.ItemId : null,this.SearchData.SelectedDescription ? this.SearchData.SelectedDescription.ItemCode : null,this.SearchData.SelectedVendor ? this.SearchData.SelectedVendor.VendorId : null);
    let filteredData = this.transform(this.ArrayOnItemType,this.sortparams);
    let aramarkItems = [];
    let descriptions = [];
    let vendors = []
    if(this.SearchData.SelectedItemType && selectedVal === "ItemType"){
      filteredData.map(el =>{
        aramarkItems.push( new BomAramarkItem(el.ItemId,el.ItemCode,el.ItemDescritpion));
        descriptions.push( new BomDescription(el.ItemId,el.ItemCode,el.ItemDescritpion));
        vendors.push( new BomSearchVendor(el.VendorId,el.VendorCode,el.VendorName));
      })
      this.SearchData.FilteredAramarkItems = aramarkItems;
      this.SearchData.FilteredDescriptions = descriptions;
      this.SearchData.FilteredVendors = vendors;
    }
    if(this.SearchData.SelectedAramarkItem && selectedVal === 'Item'){
      this.transform(filteredData,this.sortparams).map(el =>{
         descriptions.push( new BomDescription(el.ItemId,el.ItemCode,el.ItemDescritpion));
         vendors.push( new BomSearchVendor(el.VendorId,el.VendorCode,el.VendorName));
      })
      this.SearchData.FilteredDescriptions = descriptions;
      this.SearchData.FilteredVendors = vendors;
    }
    if(this.SearchData.SelectedDescription && selectedVal === 'Description'){
      this.transform(filteredData,this.sortparams).map(el =>{
        aramarkItems.push( new BomAramarkItem(el.ItemId,el.ItemCode,el.ItemDescritpion));
        vendors.push( new BomSearchVendor(el.VendorId,el.VendorCode,el.VendorName));
      })
      this.SearchData.FilteredAramarkItems = aramarkItems;
      this.SearchData.FilteredVendors = vendors;
    }
    if(this.SearchData.SelectedVendor && selectedVal === 'Vendor'){
      this.transform(filteredData,this.sortparams).map(el =>{
       aramarkItems.push( new BomAramarkItem(el.ItemId,el.ItemCode,el.ItemDescritpion));
       descriptions.push( new BomDescription(el.ItemId,el.ItemCode,el.ItemDescritpion));
      })
      this.SearchData.FilteredAramarkItems = aramarkItems;
      this.SearchData.FilteredDescriptions = descriptions;
      this.SearchData.FilteredVendors = vendors;
    }
      // filteredData.map(el => {
      //     if(this.SearchData.SelectedItemType && selectedVal === "ItemType"){
      //       aramarkItems.push( new BomAramarkItem(el.ItemId,el.ItemCode,el.ItemDescritpion));
      //       descriptions.push( new BomDescription(el.ItemId,el.ItemCode,el.ItemDescritpion));
      //       vendors.push( new BomSearchVendor(el.VendorId,el.VendorCode,el.VendorName));
      //     }
      //     if(this.SearchData.SelectedAramarkItem && selectedVal === 'Item'){
      //       descriptions.push( new BomDescription(el.ItemId,el.ItemCode,el.ItemDescritpion));
      //       vendors.push( new BomSearchVendor(el.VendorId,el.VendorCode,el.VendorName));
      //     }
      //     if(this.SearchData.SelectedDescription && selectedVal === 'Description'){
      //       aramarkItems.push( new BomAramarkItem(el.ItemId,el.ItemCode,el.ItemDescritpion));
      //       vendors.push( new BomSearchVendor(el.VendorId,el.VendorCode,el.VendorName));
      //     }
      //     if(this.SearchData.SelectedVendor && selectedVal === 'Vendor'){
      //       aramarkItems.push( new BomAramarkItem(el.ItemId,el.ItemCode,el.ItemDescritpion));
      //       descriptions.push( new BomDescription(el.ItemId,el.ItemCode,el.ItemDescritpion));
      //     }
      // });
      // if(selectedVal === "ItemType"){
      //   this.SearchData.FilteredAramarkItems = aramarkItems;
      //   this.SearchData.FilteredDescriptions = descriptions;
      //   this.SearchData.FilteredVendors = vendors;
      // }
      // if(selectedVal === "Item"){
      //   this.SearchData.FilteredDescriptions = descriptions;
      //   this.SearchData.FilteredVendors = vendors;
      // }
      // if(selectedVal === "Description"){
      //   this.SearchData.FilteredAramarkItems = aramarkItems;
      //   this.SearchData.FilteredVendors = vendors;
      // }
      // if(selectedVal === "Vendor"){
      //   this.SearchData.FilteredAramarkItems = aramarkItems;
      //   this.SearchData.FilteredDescriptions = descriptions;
      // }
     
      if(this._sortPipe.removeDuplicates(this.SearchData.FilteredAramarkItems,'ItemId').length === 1){
        this.SearchData.SelectedAramarkItem = this.SearchData.FilteredAramarkItems.length[0];
      }
      if(this._sortPipe.removeDuplicates(this.SearchData.FilteredDescriptions,'ItemDescription').length === 1){
        this.SearchData.SelectedDescription = this.SearchData.FilteredDescriptions.length[0];
      }
      if(this._sortPipe.removeDuplicates(this.SearchData.FilteredVendors,'VendorId').length === 1){
        this.SearchData.SelectedVendor = this.SearchData.FilteredVendors.length[0];
      }
     
  }

}

class SortParam{
  constructor(
     public ItemId : number,
    public ItemDescritpion : string,
   public VendorId : number
  ) {}
}

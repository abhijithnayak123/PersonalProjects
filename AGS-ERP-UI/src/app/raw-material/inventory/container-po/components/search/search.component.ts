import { Component, OnInit } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { FabricContainer } from '../../../../receiving/models/fabric-container.model';
import { PickUpNumber } from '../../../../receiving/models/pickup-number.model';
import { PurchaseOrder } from '../../../../receiving/models/purchase-order.model';
import { VendorModel } from '../../../../receiving/models/vendor.model';
import { VendorItem } from '../../../../receiving/models/vendor-item.model';
import { AramarkItemCode } from '../../../../receiving/models/aramark-item-code.model';
import { FiberContent } from '../../../../receiving/models/fiber-content.model';
import { Search } from '../../../../receiving/models/search.model';
import { GridComponent, GridDataResult } from '@progress/kendo-angular-grid';
import { ContainerPoService } from '../../container-po.service';
import { SuccessService } from '../../../../../shared/services/success.service';
import { ErrorService } from '../../../../../shared/services/error.service';
import { ContainerMetaData } from '../../../../inventory/container-po/models/container-metadata';
import { SearchMetaData } from '../../../../inventory/container-po/models/search-metadata';
import { SearchPipe} from '../../../../../shared/pipes/search.pipe';
import { UniquePipe} from '../../../../../shared/pipes/unique.pipe';

import { SortDescriptor, orderBy } from '@progress/kendo-data-query';
import { ContainerCombo } from '../../../../inventory/container-po/models/container.combo';
import { PickupCombo } from '../../../../inventory/container-po/models/pickup.combo';
import { PoCombo } from '../../../../inventory/container-po/models/po.combo';
import { AramarkCombo } from '../../../../inventory/container-po/models/aramark.combo';


@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css'],
  providers: [ContainerPoService]
})


export class SearchComponent implements OnInit {

  selectedStatus: string;
  selectedContainer: ContainerCombo;
  selectedPickUp: PickupCombo;
  selectedPO: PoCombo;
  selectedVendor: string;
  selectedVendorItem: string;
  selectedAramarkItem: AramarkCombo;
  selectedFiberContent: string;
  containerStatus: Array<string> = [];
  dbcontainerStatus : Array<string> = [];

  searchDetails: Array<Search> = [];
  totalYards: number = 0;
  totalRolls: number = 0;
  setMasked: boolean = false;
  searchData:Array<ContainerMetaData> = [];

  //gridDataList:Array<ContainerMetaData> = [];
  
  gridDataList: GridDataResult = {data:[],total : 0}

  searchDataList:Array<ContainerMetaData> = [];
  filteredList:Array<ContainerMetaData> = [];
  
  containerNumbers: Array<ContainerCombo> = [];
  dbcontainerNumbers : Array<ContainerCombo> = [];
  pickupNumbers: Array<PickupCombo> = [];
  dbpickupNumbers : Array<PickupCombo> = [];
  poNumbers: Array<PoCombo> = [];
  dbpoNumbers : Array<PoCombo> = [];
  vendors: Array<string> = [];
  dbvendors : Array<string> = [];
  vendorItems: Array<string> = [];
  dbvendorItems : Array<string> = [];
  aramarkItems: Array<AramarkCombo> = [];
  dbaramarkItems : Array<AramarkCombo> = [];
  fiberContents: Array<string> = [];
  dbfiberContents : Array<string> = [];
  gridView: GridDataResult;

  searchParams : any;
  params: any;

  filterSort: SortDescriptor[] = [];

  logicalStatus: string ="";
  containerNbr : string = "";
  pickupNbr : string ="";
  poNbr : string = "";
  vendorName : string = "";
  vendorItem : string = "";
  aramarkItem : string = "";
  fiberContent : string = "";

  constructor(
    private _containerPoService: ContainerPoService,
    private _successService: SuccessService,
    private _errorService: ErrorService,
    private _searchPipe: SearchPipe,
    private _uniquePipe: UniquePipe
  ) {    
  }

  ngOnInit() {
    this.selectedStatus="All";
    this.populateSearchDataModel();
    this.getContainerStatus();   
    this.setMasked = false;  
  }

   populateSearchDataModel(){
    this._containerPoService.GetContainerSearchData().subscribe(
      data => {
        this.searchDataList= this.searchData = data;
        this.getAllDropdowns(this.selectedStatus);
        this.getTotalRolls();
        this.getTotalYards();
      },
      (err: HttpErrorResponse) => {
        this._errorService.error(err);
      });
  }


  //Get the Contains Status
  getContainerStatus(){
    this.containerStatus = ["All","Build","Intransit","Closed"];
  }

  clearSearchForm(){
    this.selectedStatus = "All";
    this.populateSearchDataModel();
    this.selectedContainer = undefined;
    this.selectedPickUp = undefined;
    this.selectedPO = undefined;
    this.selectedVendor = undefined;
    this.selectedVendorItem = undefined;
    this.selectedAramarkItem = undefined;
    this.selectedFiberContent = undefined;
    this.searchParams = null;
    this.searchDataList = this.searchData;
   // this.gridDataList = [];
    this.filteredList = [];
    this.resetData();
    this.getTotalRolls();
    this.getTotalYards();
    this.setMasked = false;
   
  }

  getTotalYards() {
    this.totalYards = 0;
    return this.gridDataList.data.forEach(s => {
      this.totalYards += s.TotalYards 
    })
  }

  getTotalRolls() {
    this.totalRolls = 0;
    return this.gridDataList.data.forEach(s => {
      this.totalRolls += s.TotalRolls 
    })
  }

  onStatusChange(value){
    this.setMasked = true;
    this.resetSelectedData();
    this.getAllDropdowns(this.selectedStatus);
    //this.reloadSearchData();
  }

  resetData(){
    this.containerNumbers=[];
    this.pickupNumbers = [];
    this.poNumbers=[];
    this.vendors=[];
    this.vendorItems=[];
    this.aramarkItems=[];
    this.fiberContents = [];
  }

  resetSelectedData(){
    this.selectedContainer = undefined;
    this.selectedPickUp = undefined;
    this.selectedPO = undefined;
    this.selectedVendor = undefined;
    this.selectedVendorItem = undefined;
    this.selectedAramarkItem = undefined;
    this.selectedFiberContent = undefined;
  }

  setDropdowns(){
    
    // this.containerNumbers = this._uniquePipe.transform(this.containerNumbers);
    // this.pickupNumbers = this._uniquePipe.transform(this.pickupNumbers);
    // this.poNumbers = this._uniquePipe.transform(this.poNumbers);
    // this.vendors = this._uniquePipe.transform(this.vendors);
    // this.vendorItems = this._uniquePipe.transform(this.vendorItems);
    // this.aramarkItems = this._uniquePipe.transform(this.aramarkItems);
    // this.fiberContents = this._uniquePipe.transform(this.fiberContents);
    
    if(this.containerNumbers.length == 1){
      this.selectedContainer = this.containerNumbers[0];
    }
    if(this.pickupNumbers.length == 1){
      this.selectedPickUp = this.pickupNumbers[0];
    }
    if(this.poNumbers.length == 1){
      this.selectedPO = this.poNumbers[0];
    }
    if(this.vendors.length == 1){
      this.selectedVendor = this.vendors[0];
    }
    if(this.vendorItems.length == 1){
      this.selectedVendorItem = this.vendorItems[0];
    }
    if(this.aramarkItems.length == 1){
      this.selectedAramarkItem = this.aramarkItems[0];
    }
    if(this.fiberContents.length == 1){
      this.selectedFiberContent = this.fiberContents[0];
    }
    this.containerNumbers = this.containerNumbers;
    this.pickupNumbers = this.dbpickupNumbers;
    this.poNumbers = this.dbpoNumbers;
    this.vendors = this.dbvendors;
    this.vendorItems = this.dbvendorItems;
    this.aramarkItems = this.dbaramarkItems;
    this.fiberContents = this.dbfiberContents;
    
  }

  //on status drop down changes.
  getAllDropdowns(status){
      this.resetData();
      this.searchDataList = this.searchData;
      if(this.searchDataList.length > 0){

       if(status !== "All"){ 
          this.searchDataList = this.searchDataList.filter(c => c['LogicalStatusDescription'] == status);
       }
      
       this.searchDataList.forEach( x=> {this.containerNumbers.push(new ContainerCombo(x.ContainerId,x.ContainerNumber,x.VendorName))}); 
       this.searchDataList.forEach( x=> {this.pickupNumbers.push(new PickupCombo(x.PickupNumber,x.VendorName,x.PickupLocation))}); 
       this.searchDataList.forEach( x=> {this.poNumbers.push(new PoCombo(x.PONumber,x.CreatedDate))}); 
       this.searchDataList.forEach( x=> {this.vendors.push(x.VendorName)}); 
       this.searchDataList.forEach( x=> {this.vendorItems.push(x.VendorItem)}); 
       this.searchDataList.forEach( x=> {this.aramarkItems.push(new AramarkCombo(x.AramarkItemCode,x.AramarkDescription))}); 
       this.searchDataList.forEach( x=> {this.fiberContents.push(x.FiberContent)}); 

       this.dbcontainerNumbers = this.containerNumbers;
    this.dbpickupNumbers = this.pickupNumbers;
    this.dbpoNumbers = this.poNumbers;
    this.dbvendors = this.vendors;
    this.dbvendorItems = this.vendorItems;
    this.dbaramarkItems = this.aramarkItems;
    this.dbfiberContents = this.fiberContents;
       //Bring this as a Pipe
      this.containerNumbers = this.containerNumbers.reduce( function (p,c){
          var index = p.findIndex(function(x){ return x.ContainerNbr == c.ContainerNbr});
          if(index == -1){
              p.push(c);
          }
          else{
            p[index].VendorName = 'Multiple';
          }
          return p;
           }, []);
        }
        // this.containerNumbers = this.dbcontainerNumbers;
        this.setDropdowns();    
  }

  //on container dropdown changes
  onContainerChange(value){
      if(value === undefined){
        return;
      }
     if(this.selectedContainer !== undefined){
          this.containerNbr = this.selectedContainer.ContainerNbr;
      }else{
          this.containerNbr= null;
      }
      this.pickupNumbers = [];
      this.poNumbers=[];
      this.vendors=[];
      this.vendorItems=[];
      this.aramarkItems=[];
      this.fiberContents = [];

      if(this.containerNbr !== null){
        this.searchDataList.forEach( x=> { if(x.ContainerNumber === this.containerNbr){this.pickupNumbers.push(new PickupCombo(x.PickupNumber,x.VendorName,x.PickupLocation))}}); 
        this.searchDataList.forEach( x=> { if(x.ContainerNumber === this.containerNbr){this.poNumbers.push(new PoCombo(x.PONumber,x.CreatedDate))}}); 
        this.searchDataList.forEach( x=> { if(x.ContainerNumber === this.containerNbr){this.vendors.push(x.VendorName)}}); 
        this.searchDataList.forEach( x=> { if(x.ContainerNumber === this.containerNbr){this.vendorItems.push(x.VendorItem)}}); 
        this.searchDataList.forEach( x=> { if(x.ContainerNumber === this.containerNbr){this.aramarkItems.push(new AramarkCombo(x.AramarkItemCode,x.AramarkDescription))}}); 
        this.searchDataList.forEach( x=> { if(x.ContainerNumber === this.containerNbr){this.fiberContents.push(x.FiberContent)}}); 
      }
     // this.reloadSearchData();
      this.setDropdowns();
      this.setMasked = true;
  }

  //On Pickup dropdown changes

  onPickupChange(value){

    if(value === undefined){
      return;
    }
    
    if(this.selectedPickUp !== undefined){
      this.pickupNbr = this.selectedPickUp.PickupNbr;
    }else{
      this.pickupNbr= null;
    } 
 
     this.containerNumbers=[];
     this.poNumbers=[];
     this.vendors=[];
     this.vendorItems=[];
     this.aramarkItems=[];
     this.fiberContents = [];

    this.searchDataList.forEach( x=> { if(x.PickupNumber === this.pickupNbr){this.containerNumbers.push(new ContainerCombo(x.ContainerId,x.ContainerNumber,x.VendorName))}}); 
    this.searchDataList.forEach( x=> { if(x.PickupNumber === this.pickupNbr){this.poNumbers.push(new PoCombo(x.PONumber,x.CreatedDate))}}); 
    this.searchDataList.forEach( x=> { if(x.PickupNumber === this.pickupNbr){this.vendors.push(x.VendorName)}}); 
    this.searchDataList.forEach( x=> { if(x.PickupNumber === this.pickupNbr){this.vendorItems.push(x.VendorItem)}}); 
    this.searchDataList.forEach( x=> { if(x.PickupNumber === this.pickupNbr){this.aramarkItems.push(new AramarkCombo(x.AramarkItemCode,x.AramarkDescription))}}); 
    this.searchDataList.forEach( x=> { if(x.PickupNumber === this.pickupNbr){this.fiberContents.push(x.FiberContent)}}); 
    
    this.setDropdowns();
    this.setMasked = true;
    //this.reloadSearchData();
  }

  //On PO dropdown changes
  onPOChange(value){

    if(value === undefined){
      return;
    }
     
    if(this.selectedPO !== undefined){
      this.poNbr = this.selectedPO.PoNbr;
    }else{
      this.poNbr= null;
    } 

    this.containerNumbers=[];
    this.pickupNumbers=[];
    this.vendors=[];
    this.vendorItems=[];
    this.aramarkItems=[];
    this.fiberContents = [];

   this.searchDataList.forEach( x=> { if(x.PONumber === this.poNbr){this.containerNumbers.push(new ContainerCombo(x.ContainerId,x.ContainerNumber,x.VendorName))}}); 
   this.searchDataList.forEach( x=> { if(x.PONumber === this.poNbr){this.pickupNumbers.push(new PickupCombo(x.PickupNumber,x.VendorName,x.PickupLocation))}}); 
   this.searchDataList.forEach( x=> { if(x.PONumber === this.poNbr){this.vendors.push(x.VendorName)}}); 
   this.searchDataList.forEach( x=> { if(x.PONumber === this.poNbr){this.vendorItems.push(x.VendorItem)}}); 
   this.searchDataList.forEach( x=> { if(x.PONumber === this.poNbr){this.aramarkItems.push(new AramarkCombo(x.AramarkItemCode,x.AramarkDescription))}}); 
   this.searchDataList.forEach( x=> { if(x.PONumber === this.poNbr){this.fiberContents.push(x.FiberContent)}}); 

   this.setDropdowns();
    this.setMasked = true;
   // this.reloadSearchData();
  }

  //On Vendor dropdown changes
  onVendorChange(value){
     
    if(value === undefined){
      return;
    }
   
    if(this.selectedVendor !== undefined){
      this.vendorName = this.selectedVendor;
    }else{
      this.vendorName= null;
    } 

    this.containerNumbers=[];
    this.pickupNumbers=[];
    this.poNumbers=[];
    this.vendorItems=[];
    this.aramarkItems=[];
    this.fiberContents = [];

   this.searchDataList.forEach( x=> { if(x.VendorName === this.vendorName){this.containerNumbers.push(new ContainerCombo(x.ContainerId,x.ContainerNumber,x.VendorName))}}); 
   this.searchDataList.forEach( x=> { if(x.VendorName === this.vendorName){this.pickupNumbers.push(new PickupCombo(x.PickupNumber,x.VendorName,x.PickupLocation))}}); 
   this.searchDataList.forEach( x=> { if(x.VendorName === this.vendorName){this.poNumbers.push(new PoCombo(x.PONumber,x.CreatedDate))}}); 
   this.searchDataList.forEach( x=> { if(x.VendorName === this.vendorName){this.vendorItems.push(x.VendorItem)}}); 
   this.searchDataList.forEach( x=> { if(x.VendorName === this.vendorName){this.aramarkItems.push(new AramarkCombo(x.AramarkItemCode,x.AramarkDescription))}}); 
   this.searchDataList.forEach( x=> { if(x.VendorName === this.vendorName){this.fiberContents.push(x.FiberContent)}}); 
   
   this.setDropdowns();
    this.setMasked = true;
   // this.reloadSearchData();
  }

  //On Vendor Item changes
  onVendorItemChange(value){
    if(value === undefined){
      return;
    }
    if(this.selectedVendorItem !== undefined){
      this.vendorItem = this.selectedVendorItem;
    }else{
      this.vendorItem= null;
    } 

    this.containerNumbers=[];
    this.pickupNumbers=[];
    this.poNumbers=[];
    this.vendors=[];
    this.aramarkItems=[];
    this.fiberContents = [];

   this.searchDataList.forEach( x=> { if(x.VendorItem === this.vendorItem){this.containerNumbers.push(new ContainerCombo(x.ContainerId,x.ContainerNumber,x.VendorName))}}); 
   this.searchDataList.forEach( x=> { if(x.VendorItem === this.vendorItem){this.pickupNumbers.push(new PickupCombo(x.PickupNumber,x.VendorName,x.PickupLocation))}}); 
   this.searchDataList.forEach( x=> { if(x.VendorItem === this.vendorItem){this.poNumbers.push(new PoCombo(x.PONumber,x.CreatedDate))}}); 
   this.searchDataList.forEach( x=> { if(x.VendorItem === this.vendorItem){this.vendors.push(x.VendorName)}}); 
   this.searchDataList.forEach( x=> { if(x.VendorItem === this.vendorItem){this.aramarkItems.push(new AramarkCombo(x.AramarkItemCode,x.AramarkDescription))}}); 
   this.searchDataList.forEach( x=> { if(x.VendorItem === this.vendorItem){this.fiberContents.push(x.FiberContent)}}); 

   this.setDropdowns();
    this.setMasked = true;
   // this.reloadSearchData();
  }

  //On Aramark Item changes
  onAramarkChange(value){
    if(value == undefined){
      return;
    }

    if(this.selectedAramarkItem !== undefined){
      this.aramarkItem = this.selectedAramarkItem.AramarkItemCode;
    }else{
      this.aramarkItem= null;
    } 

    this.containerNumbers=[];
    this.pickupNumbers=[];
    this.poNumbers=[];
    this.vendors=[];
    this.vendorItems=[];
    this.fiberContents = [];

    this.searchDataList.forEach( x=> { if(x.AramarkItemCode === this.aramarkItem){this.containerNumbers.push(new ContainerCombo(x.ContainerId,x.ContainerNumber,x.VendorName))}}); 
    this.searchDataList.forEach( x=> { if(x.AramarkItemCode === this.aramarkItem){this.pickupNumbers.push(new PickupCombo(x.PickupNumber,x.VendorName,x.PickupLocation))}}); 
    this.searchDataList.forEach( x=> { if(x.AramarkItemCode === this.aramarkItem){this.poNumbers.push(new PoCombo(x.PONumber,x.CreatedDate))}}); 
    this.searchDataList.forEach( x=> { if(x.AramarkItemCode === this.aramarkItem){this.vendors.push(x.VendorName)}}); 
    this.searchDataList.forEach( x=> { if(x.AramarkItemCode === this.aramarkItem){this.vendorItems.push(x.VendorItem)}}); 
    this.searchDataList.forEach( x=> { if(x.AramarkItemCode === this.aramarkItem){this.fiberContents.push(x.FiberContent)}}); 

    this.setDropdowns();
    this.setMasked = true;
    //this.reloadSearchData();
  }

  //On Fiber Content dropdown value changes
  onFiberContentChange(value){

    if(value == undefined){
      return;
    }

    if(this.selectedFiberContent !== undefined){
      this.fiberContent = this.selectedFiberContent;
    }else{
      this.fiberContent= null;
    } 

    this.containerNumbers=[];
    this.pickupNumbers=[];
    this.poNumbers=[];
    this.vendors=[];
    this.vendorItems=[];
    this.aramarkItems=[];

    this.searchDataList.forEach( x=> { if(x.FiberContent === this.fiberContent){this.containerNumbers.push(new ContainerCombo(x.ContainerId,x.ContainerNumber,x.VendorName))}}); 
    this.searchDataList.forEach( x=> { if(x.FiberContent === this.fiberContent){this.pickupNumbers.push(new PickupCombo(x.PickupNumber,x.VendorName,x.PickupLocation))}}); 
    this.searchDataList.forEach( x=> { if(x.FiberContent === this.fiberContent){this.poNumbers.push(new PoCombo(x.PONumber,x.CreatedDate))}}); 
    this.searchDataList.forEach( x=> { if(x.FiberContent === this.fiberContent){this.vendors.push(x.VendorName)}}); 
    this.searchDataList.forEach( x=> { if(x.FiberContent === this.fiberContent){this.vendorItems.push(x.VendorItem)}}); 
    this.searchDataList.forEach( x=> { if(x.FiberContent === this.fiberContent){this.aramarkItems.push(new AramarkCombo(x.AramarkItemCode,x.AramarkDescription))}}); 

    this.setDropdowns();
    this.setMasked = true;
    //this.reloadSearchData();
  }

   generateSearchParams(){
    if(this.selectedStatus === "All"){
       this.logicalStatus = null;
    }else{
       this.logicalStatus =this.selectedStatus;
    }
    
    if(this.selectedContainer !== undefined){
       this.containerNbr = this.selectedContainer.ContainerNbr;
    }else{
      this.containerNbr= null;
    }
    if(this.selectedPickUp !== undefined){
      this.pickupNbr = this.selectedPickUp.PickupNbr;
      }else{
        this.pickupNbr= null;
    }
    if(this.selectedPO !== undefined){
      this.poNbr = this.selectedPO.PoNbr;
    }else{
      this.poNbr = null;
    }
    if(this.selectedVendor !== undefined){
      this.vendorName = this.selectedVendor;
    }else{
      this.vendorName = null;
    }

    if(this.selectedVendorItem !== undefined){
      this.vendorItem = this.selectedVendorItem;
    }else{
      this.vendorItem = null;
    }
    
    if(this.selectedAramarkItem != undefined){
      this.aramarkItem = this.selectedAramarkItem.AramarkItemCode;
    }else{
      this.aramarkItem = null;
    }
   
    if(this.selectedFiberContent != undefined){
      this.fiberContent = this.selectedFiberContent;
    }else{
      this.fiberContent= null;
    }   
    this.searchParams = new SearchMetaData(this.containerNbr,this.pickupNbr,this.poNbr,this.vendorName,this.vendorItem,this.aramarkItem,this.fiberContent,this.logicalStatus);
  }

  reloadDataGrid(){    
    this.gridDataList.data = this.searchData;
    this.generateSearchParams();
    this.gridDataList.data = this._searchPipe.transform(this.gridDataList.data,this.searchParams);
    this.getTotalRolls();
    this.getTotalYards();
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

  onExportPDF(grid: GridComponent) {
    grid.saveAsPDF();
  }

  onExportExcel(grid: GridComponent) {
    grid.saveAsExcel();
  }

  handleStatusFilter(value){
    this.containerStatus = this.dbcontainerStatus.filter((s) => (s.toLocaleUpperCase()).startsWith(value.toLocaleUpperCase()));
  }
  handleContainerFilter(value){
    this.containerNumbers = this.dbcontainerNumbers.filter((s) => (s.ContainerNbr.toLocaleLowerCase().startsWith(value.toLocaleLowerCase()))); 
  }
  handlePickupFilter(value){
    this.pickupNumbers = this.dbpickupNumbers.filter((s) => (s.PickupNbr.toLocaleLowerCase().startsWith(value.toLocaleLowerCase())));     
  }
  handlePoFilter(value){
    this.poNumbers = this.dbpoNumbers.filter((s) => (s.PoNbr.toString().startsWith(value)));   
  }
  handleVendorFilter(value){
    this.vendors = this.dbvendors.filter(s => (s.toLocaleUpperCase()).startsWith(value.toLocaleUpperCase()));
  }
  handleVendorItemFilter(value){
    this.vendorItems = this.dbvendorItems.filter(s => (s.toLocaleUpperCase().startsWith(value.toLocaleUpperCase())));
  }
  handleAramarkItemFilter(value){
    this.aramarkItems = this.dbaramarkItems.filter(s =>(s.AramarkItemCode.toLocaleUpperCase().startsWith(value.toLocaleUpperCase())));
  }
  handleFibercontentFilter(value){
    this.fiberContents = this.dbfiberContents.filter(s => (s.toLocaleLowerCase().startsWith(value.toLocaleUpperCase())));
  }
}

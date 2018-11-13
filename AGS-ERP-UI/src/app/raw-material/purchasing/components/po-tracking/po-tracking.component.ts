import { Component, OnInit, group } from "@angular/core";
import { Container } from "@angular/compiler/src/i18n/i18n_ast";
import * as moment from 'moment';
import {
  process,
  GroupDescriptor,
  State,
  aggregateBy,
  SortDescriptor,
  orderBy
} from "@progress/kendo-data-query";
import { Group } from "../../../inventory/container-po/models/group";
import { ContainerBuildDetail } from "../../../inventory/container-po/models/container-build-detail";
import { AramarkItemDetails } from "../../../inventory/physical-adjustments/models/aramark-item-details.model";
import { VendorDetails } from "../../../inventory/physical-adjustments/models/vendor-details.model";
import { ContainerType } from "../../../inventory/container-po/models/containertype";
import { PurchaseOrder } from "../../../inventory/container-po/models/purchase-order";
import { PickupLocation } from "../../../inventory/container-po/models/pickup-location";
import { FabricContainer } from "../../../receiving/models/fabric-container.model";
import { PurchasingService } from "../../purchasing.service";
import { Vendor } from "../../models/vendor";
import { PONumber } from "../../models/poNumber";
import {POHdr} from "../../models/POHdr";
import { POItemNumber } from "../../models/POItemNumber";
import { POStatus } from "../../models/POStatus";
import { POFilterData } from "../../models/POFilterData";
import { HttpErrorResponse } from "@angular/common/http";
import { ErrorService } from "../../../../shared/services/error.service";
import { ToastService } from "../../../../shared/services/toast.service";
import { CommonService } from "../../../../shared/services/common.service";
import { DateInputsModule } from '@progress/kendo-angular-dateinputs';
import { GridDataResult } from "@progress/kendo-angular-grid";
import { POSearchHeader } from "../../models/POMaintenanceHeader";
import { UniquePipe } from "../../../../shared/pipes/unique.pipe";
import { transform } from "@progress/kendo-drawing/dist/es/geometry";
import { HeaderSearchData } from "../../models/HeaderSearchData";
import { MetaSearchData } from "../../models/MetaSearchData";
import { SearchPipe } from "../../../../shared/pipes/search.pipe";
import { SortPipe } from "../../../../shared/pipes/sort.pipe";
import { forEach } from "@angular/router/src/utils/collection";
import { POComboBox } from "../../models/POComboBox";
import { AramarkCombo } from "../../../inventory/container-po/models/aramark.combo";

import { LocalStorageService } from '../../../../shared/wrappers/local-storage.service';
import { SessionStorageService } from '../../../..//shared/wrappers/session-storage.service';

import { GridComponent} from '@progress/kendo-angular-grid';

@Component({
  selector: "app-po-tracking",
  templateUrl: "./po-tracking.component.html",
  styleUrls: ["./po-tracking.component.css"],
  providers: [PurchasingService]
})
export class PoTrackingComponent implements OnInit {

//Satya

dropdownList:Array<POSearchHeader> =[];


VendorsList: Array<string> = [];
dbVendors : Array<string> = [];
selectedVendor:string;
PoHdr : POHdr;
PickupList: Array<string> = [];
changePickuplist : Array<string> = [];
DBPickupList: Array<string> = [];
selectedPickup: string;

gridSessionData : Array<POSearchHeader> = [];

PoList: Array<POComboBox> = [];
changePoList : Array <POComboBox> = [];
DbPoList : Array<POComboBox> =[];
selectedPONbr: POComboBox;

AramarkList: Array<AramarkCombo> = [];
changeAramarkList: Array<AramarkCombo> = [];
DBAramarkList : Array<AramarkCombo> = [];
selectedItemNbr: AramarkCombo;

POStatus  :Array<string> =[];
selectedPOStatus : string;
 
headerData:Array<POSearchHeader> = [];
filterHeaderData:Array<POSearchHeader> = [];


FromDate:Array<Date> = [];
ToDate:Array<Date> = [];
selectedFromDate: Date = null;
POHdrId:string;
selectedToDate: Date = null;
selectedPOHdrId: number;

searchParams : any;
params: any;

currentVendor: string;
currentLocation: string;
currentPoNbr: number;
currentItem: string;
currentStatus: string;
currentFromDate: number;
currentToDate: number;
PoStatus: Array<string>=[];


searchData:Array<POSearchHeader> = [];
gridDataList: GridDataResult = {data:[],total : 0}


filterSort: SortDescriptor[] = [];
show: boolean = false;

setMasked: boolean = false;

retainedSearchData: MetaSearchData;

public events: string[] = [];
public value: Date = new Date();

constructor(
    private _errorService: ErrorService,
    private _purchasingService: PurchasingService,
    private _toastService: ToastService,
    private _commonService: CommonService,
    private _sessionStorage: SessionStorageService,
    private  _uniquePipe: UniquePipe,
    private _searchPipe:SearchPipe,
    private _sortPipe:SortPipe
) {}

  ngOnInit() {
    this.clearFilters();
    if(this._sessionStorage.get('SearchData')){
       this.setMasked = false;
       this.getSessionInfo();
    }else{       
       this.setMasked = true; 
       this.PopulateSearchData();
       this.getPoStatus();
    }
    this._sessionStorage.remove('SearchData');
 }

 getPoStatus(){
  this.PoStatus = ["Planned","Issued","Active","Shipped","Closed","Cancelled"];
 }

 clearFilters(){  
    this.selectedVendor = undefined;
    this.selectedPickup = undefined;
    this.selectedPONbr = undefined;    
    this.selectedItemNbr = undefined;
    this.selectedPOStatus = undefined;
    this.selectedFromDate = undefined;
    this.selectedToDate = undefined;

    this.currentVendor = undefined;
    this.currentLocation= undefined;
    this.currentPoNbr= undefined;
    this.currentItem = undefined;
    this.currentStatus = undefined;
    this.currentFromDate = undefined;
    this.currentToDate = undefined;

 }

/*
* Method to fetch all PO search related details.
*/
PopulateSearchData(){
this.show = true;
this._purchasingService.getPOHeaderData().subscribe(
  data =>{
    this.headerData = this.filterHeaderData = data;
    this.getAllHeaderDetails(this.headerData);
    this.show = false;
  },
  (err: HttpErrorResponse) => {
    this._errorService.error(err);
  });

}

getSessionInfo(){
  this.show = true;
  let sessionObject : POSearchHeader;
  if(this._sessionStorage.get('SearchData')){
    this.retainedSearchData = this._sessionStorage.get('SearchData');   
    this.selectedVendor = this.retainedSearchData.Vendor
    this.selectedPickup = this.retainedSearchData.PickupLoc;
    this.selectedPONbr= this.retainedSearchData.PONbr;
    this.selectedItemNbr = this.retainedSearchData.ItemNbr;
    if(this.retainedSearchData.PofromDate !== undefined){
    this.selectedFromDate = new Date(this.retainedSearchData.PofromDate);
    }
    if(this.retainedSearchData.PoToDate !== undefined){
    this.selectedToDate =  new Date(this.retainedSearchData.PoToDate);
    }
    this.selectedPOStatus = this.retainedSearchData.POStatus;
    this.VendorsList = this.retainedSearchData.vendors;
    this.PickupList = this.retainedSearchData.pickups;
    this.PoList = this.retainedSearchData.pos;
    this.AramarkList=this.retainedSearchData.aramarks;
    this.PoStatus=this.retainedSearchData.status;
    this.gridSessionData = [];
    this.gridSessionData = this.retainedSearchData.resultData;
    if(this._sessionStorage.get('Hdr_Details')){
        this.PoHdr = this._sessionStorage.get('Hdr_Details');
        if(this.selectedPOStatus){
            if(this.selectedPOStatus !== this.PoHdr.Status)
            {
              const PoNumber = this.PoHdr.POHdrId;
              this.gridDataList.data = this.gridSessionData.filter(c => c.PONbr != PoNumber);
            }else{
              this.gridDataList.data = this.retainedSearchData.resultData;
            }
        }else{
            this.gridDataList.data = this.retainedSearchData.resultData;
        }
        this._sessionStorage.remove('Hdr_Details');
   }else{
        this.gridDataList.data = this.retainedSearchData.resultData;
   } 
    this._sessionStorage.remove('SearchData');
  }
  this.show = false;
}

/*
* Populate All Search Dropdowns.
*/
getAllHeaderDetails(dataList){
 this.dropdownList= dataList;
 if( this.dropdownList.length > 0)
    {
      this.dropdownList.forEach( c =>{
        this.VendorsList.push(c.Vendor);
        this.PickupList.push(c.PickupLoc);
        this.PoList.push(new POComboBox(c.PONbr,c.OrderDate));
        this.AramarkList.push(new AramarkCombo(c.ItemNbr,c.ItemDescription));
      });
    }
  this.dbVendors = this.VendorsList;
  this.DBPickupList = this.PickupList;
  this.DbPoList =  this.PoList;
  this.DBAramarkList = this.AramarkList;
  this.setDropdowns();
  
}

//Setting Dropdowns List based ascending order.
setDropdowns(){
  this.VendorsList= this._uniquePipe.transform(this.VendorsList);
  this.PickupList= this._uniquePipe.transform(this.PickupList);
  this.PoList = this._sortPipe.removeDuplicates(this.PoList,'PONbr');
  this.AramarkList = this._sortPipe.removeDuplicates(this.AramarkList,'AramarkItemCode');
  this.POStatus=this._uniquePipe.transform(this.PoStatus);
  if(this.VendorsList.length === 1){
    this.selectedVendor = this.VendorsList[0];
  }
  if(this.PickupList.length === 1){
    this.selectedPickup = this.PickupList[0];
  }
  if(this.PoList.length === 1){
    this.selectedPONbr = this.PoList[0];
  }
  if(this.AramarkList.length === 1){
    this.selectedItemNbr = this.AramarkList[0]
  }
  if(this.POStatus.length === 1){
    this.selectedPOStatus = this.POStatus[0];
  }

  // this.VendorsList = this.dbVendors;
  // this.PickupList = this.DBPickupList;
  // this.PoList = this.DbPoList;
  // this.AramarkList = this.DBAramarkList;
}

onStatusChange(value){
  this.show=true;
  if(value === undefined){
    return;
  }
  this.getAllDropdowns(value);
  this.setDropdowns();
  this.setMasked = false;
  this.show=false;
}

getAllDropdowns(status){
  this.dropdownList = this.headerData;
  this.VendorsList=[];
  this.PickupList = [];
  this.PoList =[];
  this.AramarkList=[];

  this.dropdownList = this.dropdownList.filter(c => c['POStatus'] == status);
  if(this.dropdownList.length > 0){
    this.dropdownList.forEach( x=> { 
      this.VendorsList.push(x.Vendor);
      this.PickupList.push(x.PickupLoc);
      this.PoList.push(new POComboBox(x.PONbr,x.OrderDate));
      this.AramarkList.push(new AramarkCombo(x.ItemNbr,x.ItemDescription));    
    }); 
  }
  this.VendorsList = this.dbVendors;
  this.PickupList = this.DBPickupList;
  this.PoList = this.DbPoList;
  this.AramarkList = this.DBAramarkList;
}

onVendorChange(value){
  if(value === undefined){
    return;
  }
 if(this.selectedVendor !== undefined){
      this.currentVendor = this.selectedVendor;
  }else{
      this.currentVendor= null;
  }

  this.PickupList = [];
  this.changePickuplist = [];
  this.PoList = [];
  this.changePoList = [];
  this.AramarkList = [];
  this.changeAramarkList = [];
  this.PoStatus = [];
  
  if(this.currentVendor !== null){
    this.dropdownList.forEach( x=> {
      if(x.Vendor === this.currentVendor){
      // this.PickupList.push(x.PickupLoc);
      this.changePickuplist.push(x.PickupLoc);
      this.PickupList = this.changePickuplist;
      // this.PoList.push(new POComboBox(x.PONbr,x.OrderDate));
      this.changePoList.push(new POComboBox(x.PONbr,x.OrderDate));
      this.PoList = this.changePoList;
      // this.AramarkList.push(new AramarkCombo(x.ItemNbr,x.ItemDescription));   
      this.changeAramarkList.push(new AramarkCombo(x.ItemNbr, x.ItemDescription)); 
      this.AramarkList =  this.changeAramarkList;
      this.PoStatus.push(x.POStatus);
    }}); 
   //this.dropdownList.forEach( x=> { if(x.Vendor === this.currentVendor){this.PoList.push(new POComboBox(x.PONbr,x.OrderDate))}}); 
   //this.dropdownList.forEach( x=> { if(x.Vendor === this.currentVendor){this.AramarkList.push(new AramarkCombo(x.ItemNbr,"Filter"))}});  
  }

  this.setDropdowns();
  this.setMasked = false;

}


onLocationChange(value){
  if(value === undefined){
    return;
  }
 if(this.selectedPickup !== undefined){
      this.currentLocation = this.selectedPickup;
  }else{
      this.currentLocation= null;      
  }

  this.VendorsList = [];
  this.PoList =[];
  this.AramarkList=[];
  this.PoStatus = [];
  
  if(this.currentLocation !== null){
    this.dropdownList.forEach( x=> { if(x.PickupLoc === this.currentLocation){this.VendorsList.push(x.Vendor)}}); 
    this.dropdownList.forEach( x=> { if(x.PickupLoc === this.currentLocation){this.PoList.push(new POComboBox(x.PONbr,x.OrderDate))}}); 
    this.dropdownList.forEach( x=> { if(x.PickupLoc === this.currentLocation){this.AramarkList.push(new AramarkCombo(x.ItemNbr,x.ItemDescription))}});  
    this.dropdownList.forEach( x=> { if(x.PickupLoc === this.currentLocation){this.PoStatus.push(x.POStatus)}});
  }
  this.setDropdowns();
  this.setMasked = false;
}


onPOChange(value){
  if(value === undefined){
    return;
  }
 if(this.selectedPONbr !== undefined){
      this.currentPoNbr = this.selectedPONbr.PONbr;
  }else{
      this.currentPoNbr= null;
  }

  this.VendorsList = [];
  this.PickupList =[];
  this.AramarkList=[];
  this.PoStatus = [];
  
  if(this.currentPoNbr !== null){
    this.dropdownList.forEach( x=> { if(x.PONbr === this.currentPoNbr){this.VendorsList.push(x.Vendor)}}); 
    this.dropdownList.forEach( x=> { if(x.PONbr === this.currentPoNbr){this.PickupList.push(x.PickupLoc)}}); 
    this.dropdownList.forEach( x=> { if(x.PONbr === this.currentPoNbr){this.AramarkList.push(new AramarkCombo(x.ItemNbr,x.ItemDescription))}});
    this.dropdownList.forEach( x=> { if(x.PONbr === this.currentPoNbr){this.PoStatus.push(x.POStatus)}});
  }

  this.setDropdowns();
  this.setMasked = false;
}


onAramarkChange(value){
  if(value === undefined){
    return;
  }
 if(this.selectedItemNbr !== undefined){
      this.currentItem = this.selectedItemNbr.AramarkItemCode;
  }else{
      this.currentItem= null;
  }

  this.VendorsList = [];
  this.PickupList =[];
  this.PoList=[];
  this.PoStatus = [];
  
  if(this.currentItem !== null){
    this.dropdownList.forEach( x=> { if(x.ItemNbr === this.currentItem){this.VendorsList.push(x.Vendor)}}); 
    this.dropdownList.forEach( x=> { if(x.ItemNbr === this.currentItem){this.PickupList.push(x.PickupLoc)}}); 
    this.dropdownList.forEach( x=> { if(x.ItemNbr === this.currentItem){this.PoList.push(new POComboBox(x.PONbr,x.OrderDate))}});  
    this.dropdownList.forEach( x=> { if(x.ItemNbr === this.currentItem){this.PoStatus.push(x.POStatus)}});
  }

  this.setDropdowns();
  this.setMasked = false;
}

OnPOToChange(value){
  this.show=true;
  if(value === undefined){
    return;
  }
  if(this.selectedToDate !== undefined){
    this.currentToDate = Date.parse(moment(this.selectedToDate).format("MM/DD/YYYY"));
    this.validateDates();
   }else{
    this.currentToDate= null;
    if(this.selectedFromDate !== undefined){
      this.selectedToDate = new Date();
    }
   }
   this.setMasked = false;
}

OnPOFromChange(value){
  if(value === undefined){
    return;
  }
  if(this.selectedFromDate !== undefined){
    this.currentFromDate = Date.parse(moment(this.selectedFromDate).format("MM/DD/YYYY"));
    this.validateDates();
   }else{
    this.currentFromDate= null;
   }
   this.setMasked = false;
   
}

validateDates(){
  let fromDate: number;
  let toDate: number;
  if((this.selectedFromDate !== undefined) && (this.selectedToDate !== undefined)){
    fromDate = Date.parse(moment(this.selectedFromDate).format("MM/DD/YYYY"));    
    toDate = Date.parse(moment(this.selectedToDate).format("MM/DD/YYYY"));
    if(toDate <= fromDate){
      this._toastService.warn('PO To Date should be greater than PO From Date');
    }
   }
}


generateSearchParams(){ 

    if(this.selectedVendor !== undefined){
    this.currentVendor = this.selectedVendor;
    }else{
    this.currentVendor= null;
    }

    if(this.selectedPickup !== undefined){
    this.currentLocation = this.selectedPickup;
    }else{
        this.currentLocation= null;
    }
 
    if(this.selectedPONbr !== undefined){
      this.currentPoNbr = this.selectedPONbr.PONbr;
    }else{
          this.currentPoNbr= null;
    }

    if(this.selectedItemNbr !== undefined){
        this.currentItem = this.selectedItemNbr.AramarkItemCode;
    }else{
        this.currentItem= null;
    }

    if(this.selectedPOStatus !== undefined){
      this.currentStatus = this.selectedPOStatus;
      }else{
      this.currentStatus= null;
      }

    if(this.selectedFromDate !== undefined){
        this.currentToDate = Date.parse(moment(this.selectedToDate).format("MM/DD/YYYY"));
    }

    if(this.selectedFromDate !== undefined){
      this.currentToDate = Date.parse(moment(this.selectedToDate).format("MM/DD/YYYY"));
     }

    if(this.selectedFromDate !== undefined && this.selectedToDate === undefined){
      this.currentToDate = Date.parse(moment(new Date()).format("MM/DD/YYYY"));
      this.selectedToDate = new Date();
    }

    this.searchParams = new HeaderSearchData(this.currentVendor,this.currentLocation,this.currentPoNbr,this.currentItem,null,this.currentStatus,null);
}


loadGridData() {
    this.show = true;
    setTimeout(() => {
    this.gridDataList.data = this.filterHeaderData;
    this.generateSearchParams();
    this.gridDataList.data = this._searchPipe.transform(this.gridDataList.data,this.searchParams);

      if(this.currentFromDate === undefined && this.currentToDate !== undefined){
       let resList:Array<POSearchHeader> =[];
       //this.gridDataList.data = this.gridDataList.data.filter(x=> x['OrderDate'] <= this.currentToDate);
       this.gridDataList.data.forEach(x=> {
           if(Date.parse(moment(x.OrderDate).format("MM/DD/YYYY")) <= this.currentToDate){
              resList.push(x);
           }
       });
       this.gridDataList.data = resList;
     }
    if(this.currentFromDate !== undefined && this.currentToDate !== undefined){
      //this.gridDataList.data = this.gridDataList.data.filter(x=> (x['OrderDate'] | datePipe) >= this.currentFromDate) && (x['OrderDate'] <= this.currentToDate));
      let resList:Array<POSearchHeader> =[];
       //this.gridDataList.data = this.gridDataList.data.filter(x=> x['OrderDate'] <= this.currentToDate);
       this.gridDataList.data.forEach(x=> {
           if((Date.parse(moment(x.OrderDate).format("MM/DD/YYYY")) >= this.currentFromDate) && (Date.parse(moment(x.OrderDate).format("MM/DD/YYYY")) <= this.currentToDate)){
              resList.push(x);
           }
       });
       this.gridDataList.data = resList;
     }
     this.show= false;
    }, 4000);
     this._sessionStorage.remove('SearchData');
  }

  
  post(poHdrId) {

    if(this._sessionStorage.get('SearchData')){
       this._sessionStorage.remove('SearchData');
    }
    //Storing Po Search details to session
    this._sessionStorage.add('SearchData',new MetaSearchData(this.selectedVendor,this.selectedPickup,this.selectedPONbr,this.selectedItemNbr,this.selectedFromDate,this.selectedToDate,this.selectedPOStatus,this.VendorsList,this.PickupList,this.PoList,this.AramarkList,this.PoStatus,this.gridDataList.data));
    this.show = true;
    this._commonService.Notify({
      key: "POMaintenance",
      value: { 'PoHdrID': poHdrId}
    });
    setTimeout(() => {
      this._commonService.Notify({
        key: "POMaintenance",
        value: { 'PoHdrID': poHdrId }
      });
      this.show = false;
    }, 1);

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

  clearSearchForm() {
    this.clearFilters();
    this.PopulateSearchData();
    this.setMasked = true;
    this._sessionStorage.remove('SearchData');

  }


  formatTheDate(dateToFormat: Date) {
    let date = dateToFormat.getDate();
    let month = dateToFormat.getMonth() + 1;
    let year = dateToFormat.getFullYear();
    let formatedDate = month + "/" + date + "/" + year;
    return formatedDate;
  }

  filterDuplicates(list, value) {
    return this._sortPipe.removeDuplicates(list, value);
  }

  onExportPDF(grid: GridComponent) {
    grid.saveAsPDF();
  }

  onExportExcel(grid: GridComponent) {
    grid.saveAsExcel();
  }

   handleVendorFilter(value) {
    this.VendorsList = this.dbVendors.filter((s) => (s.toLocaleUpperCase()).startsWith(value.toLocaleUpperCase()));
  }

  handlelocationFilter(value){
    this.PickupList = this.DBPickupList.filter((s) => (s.toLocaleUpperCase()).startsWith(value.toLocaleUpperCase()));
  }

  handlePoNumbrFilter(value){
    this.PoList = this.DbPoList.filter((s) => s.PONbr.toString().startsWith(value)); 
  }
  handleAramItemFilter(value){
    this.AramarkList =  this.DBAramarkList.filter((s=>(s.AramarkItemCode.toLocaleUpperCase()).startsWith(value.toLocaleUpperCase())))
  }
}

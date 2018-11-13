import { Component, OnInit, OnDestroy } from '@angular/core';
import { GridComponent, GridDataResult } from '@progress/kendo-angular-grid';
import { SortDescriptor } from '@progress/kendo-data-query/dist/es/main';
import { orderBy } from '@progress/kendo-data-query/dist/es/array.operators';
import { HttpErrorResponse } from '@angular/common/http';
import { CommonService } from '../../../../shared/services/common.service';
import { Vendor } from '../../models/vendor';
import { FabricPlanningService } from '../../fabric-planning.service';
import { ErrorService } from '../../../../shared/services/error.service';
import { FutureWatchList } from '../../models/future-watch-list';
import { ExcelExportData } from '@progress/kendo-angular-excel-export';
import { process } from '@progress/kendo-data-query';
import { LocalStorageService } from '../../../../shared/wrappers/local-storage.service';
import { SessionStorageService } from '../../../../shared/wrappers/session-storage.service';

@Component({
  selector: 'app-future-watchlist',
  templateUrl: './future-watchlist.component.html',
  styleUrls: ['./future-watchlist.component.css'],
  providers: [FabricPlanningService]

})
export class FutureWatchlistComponent implements OnInit, OnDestroy {
  vendors: Array<Vendor>;
  selectedVendor: Vendor;
  aramarkItems: Array<FtWatchListItem> = [];
  selectedAramarkItem: FtWatchListItem;
  futureWatchListData: Array<FutureWatchList>;
  gridData: GridDataResult = {
    data: [],
    total: 0
  };
  gridAllData : GridDataResult = {
    data: [],
    total: 0
  };
  sort: SortDescriptor[] = [];
  WELabelList: Array<string>;
  WEValues: Array<string>;
  show: boolean;
  group: any[] = [];
  defaultItem: Vendor;
  vendorList:Array<Vendor>=[];
  aramarkItemList:Array<FtWatchListItem>=[];

  constructor(
    private _commonService: CommonService,
    private _fabricPlanningService: FabricPlanningService,
    private _errorService: ErrorService,
    private _sessionStorage: SessionStorageService
  ) {
    this.getGridData = this.getGridData.bind(this);
  }

  ngOnInit() {
    if(this._sessionStorage.get("future_watchlist_load")){
      this.vendors = this.vendorList = this._sessionStorage.get("future_watchlist_vendors");
      this.selectedVendor = this._sessionStorage.get("future_watchlist_vendor_selected");
      this.defaultItem = new Vendor(0, 'Select Vendor');
      this.WELabelList = new Array<string>();
      this.WEValues = new Array<string>();
      this.futureWatchListData = this.gridData.data = this.gridAllData.data = this._sessionStorage.get('fuWlGridData');
      this.WELabelList = this.gridData.data[0] ? this.gridData.data[0].WELabels : [];
      this.aramarkItemList = this.aramarkItems = this._sessionStorage.get("future_watchlist_itemList");
      this.selectedAramarkItem = this._sessionStorage.get("future_watchlist_selected_item");
      this.onAramarkItemChange(this.selectedAramarkItem);
      // this.getWatchListData(this.selectedVendor.VendorId);
      this._sessionStorage.removeAll(['future_watchlist_load', 'future_watchlist_vendors',
        'future_watchlist_selected_vendor', 'fuWlGridData','future_watchlist_itemList', 'future_watchlist_selected_item','future_watchlist_vendor_selected'
      ]);
      this.show = false;

    }
    else{
      this.getVendors();
      this.WELabelList = new Array<string>();
      this.WEValues = new Array<string>();
      this.show = false;
      this.defaultItem = new Vendor(0, 'Select Vendor');
      this.futureWatchListData = new Array<FutureWatchList>();
    }
  }

  ngOnDestroy(): void {   
    if(this.gridData.data.length > 0){
      this._sessionStorage.add('future_watchlist_load', true);
      this._sessionStorage.add('future_watchlist_vendors', this.vendors);
      this._sessionStorage.add('future_watchlist_vendor_selected', this.selectedVendor);
      this._sessionStorage.add('future_watchlist_selected_item', this.selectedAramarkItem);
      this._sessionStorage.add('fuWlGridData',this.gridAllData.data);
      this._sessionStorage.add('future_watchlist_itemList',this.aramarkItems);
    }
  }

  sortChange(sort: SortDescriptor[]): void {
    this.sort = sort;
    this.loadRollCases();
  }
  loadRollCases() {
    this.gridData = {
      data: orderBy(this.gridData.data, this.sort),
      total: this.gridData.data.length
    };
  }

  /** Get Vendors */
  getVendors() {
    const watchlistType = 'future';
    this._fabricPlanningService.getVendors(watchlistType).subscribe(
      data => {
        if (data.length > 0) {
          this.vendors = data;
          this.vendorList = this.vendors;
          if(this.vendors.length === 1){
            this.selectedVendor = this.vendors[0];
            this.onVendorChange(this.selectedVendor);
          }
        }
      },
      (err: HttpErrorResponse) => {
        this._errorService.error(err);
      }
    );
  }

  /** On change of vendor, get Future watchlist data */
  onVendorChange(selectedVendor) {
    this.selectedAramarkItem = null;
    if (selectedVendor) {
      const vendorId = selectedVendor.VendorId;
      this.getWatchListData(vendorId);
    } 
    else {
      this.futureWatchListData = this.gridData.data = this.WELabelList = this.aramarkItems = [];
      this.selectedAramarkItem = null;
      this.aramarkItemList = [];
    }
  }

  /** Get Future Watchlist data based on selected vendor */
  getWatchListData(vendorId: number) {
    this.show = true;
    this._fabricPlanningService.getFutureWatchlistData(vendorId).subscribe(
      data => {
        this.futureWatchListData = data;
        this.gridData.data = this.gridAllData.data= data;
        this.WELabelList = this.gridData.data[0] ? this.gridData.data[0].WELabels : [];
        this.populateAramarkItem();
        // if(this.selectedAramarkItem){
        //   this.onAramarkItemChange(this.selectedAramarkItem)
        // }
        this.show = false;
      },
      (err: HttpErrorResponse) => {
        this.show = false;
        this._errorService.error(err);
      }
    );
  }

  /** Populate Aramark Items Based on watchlist data of particular vendor */
  populateAramarkItem() {
    this.aramarkItems = [];
    this.futureWatchListData.forEach(x => {
      this.aramarkItems.push(new FtWatchListItem(x.ItemNumber,x.Description));
    });
    this.aramarkItemList = this.aramarkItems;
    if(this.aramarkItemList.length === 1){
      this.selectedAramarkItem = this.aramarkItemList[0];
      this.onAramarkItemChange(this.selectedAramarkItem);
    }
  }

  /** On change of Aramark Item, filter the Grid data from watchlist data */
  onAramarkItemChange(selectedAramarkItem) {
    if(selectedAramarkItem)
    {
      this.gridData.data = this.futureWatchListData.filter(x => x.ItemNumber === selectedAramarkItem.ItemNumber);
    }
    else{
      this.gridData.data = this.futureWatchListData;
    }
  }

  /** Get Column Values */
  getColumnValue(label, item) {
    const id = this.WELabelList.indexOf(label);
    const data = this.futureWatchListData.find(x => x.ItemNumber === item).WEValues[id];
    return data;
  }

  /** Export Grid data in Pdf */
  ExportToPdf(grid: GridComponent) {
    grid.saveAsPDF();
  }

  /** Export Grid data in Excel*/
  ExportToExcel(grid: GridComponent) {
    grid.saveAsExcel();
  }

  /** Post the VendorId to Service for suscription */
  post(vendorId: number, itemId: number) {
    this.show = true;
    this._commonService.Notify({
      key: 'PlanningReport',
      value: { 'vendorId': vendorId, 'itemId': itemId }
    });
    setTimeout(() => {
      this._commonService.Notify({
        key: 'PlanningReport',
        value: { 'vendorId': vendorId, 'itemId': itemId }
      });
      this.show = false;
    }, 1);
    
  }

  public getGridData(): ExcelExportData {

    const result: ExcelExportData = {
      data: process(this.generateGridExcelData(), { group: this.group, sort: [{ field: 'ItemNumber', dir: 'asc' }] }).data,
      group: this.group
    };

    return result;
  }

  generateGridExcelData() {
    let rows = this.gridData.data;
    rows.forEach(function (row) {
      for (let i = 0; i <= row.WELabels.length; i++) {
        row[row.WELabels[i]] = row.WEValues[i];
      }
    });
    return rows;
  }

  fieldColor(data, label) {
    const id = this.WELabelList.indexOf(label);
    const value = this.futureWatchListData.find(x => x.ItemNumber === data.ItemNumber).CSSList[id];
    return value;
  }

  
  handleVendorFilter(value:string) {
    this.vendorList = this.vendors.filter((s) => (s.VendorName.toLocaleUpperCase()).startsWith(value.toLocaleUpperCase()));
  }
  
  handleAramarkItemFilter(value:string) {
    this.aramarkItemList = this.aramarkItems.filter((s) => (s.ItemNumber).startsWith(value.toLocaleUpperCase()));
  }
}

export class FtWatchListItem {
 
  constructor(
    public ItemNumber : string,
    public Description : string
  ) {
  }
}


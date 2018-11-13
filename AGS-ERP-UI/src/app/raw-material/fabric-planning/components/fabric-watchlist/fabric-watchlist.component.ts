import {
  Component,
  OnInit,
  OnDestroy
} from '@angular/core';
import { CommonService } from '../../../../shared/services/common.service';
import { Subscription, Observable } from 'rxjs/Rx';
import { GridComponent, GridDataResult } from '@progress/kendo-angular-grid';
import { SortDescriptor } from '@progress/kendo-data-query/dist/es/main';
import { orderBy } from '@progress/kendo-data-query/dist/es/array.operators';
import { FabricPlanningService } from '../../fabric-planning.service';
import { HttpErrorResponse } from '@angular/common/http';
import { ErrorService } from '../../../../shared/services/error.service';
import { Vendor } from '../../models/vendor';
import { FabricWatchList } from '../../models/fabric-watch-list';
import { strictEqual } from 'assert';
import { ExcelExportData } from '@progress/kendo-angular-excel-export';
import { process } from '@progress/kendo-data-query';
import { AramarkItem } from '../../models/aramark-item';
import { SessionStorageService } from '../../../../shared/wrappers/session-storage.service';

@Component({
  selector: 'app-fabric-watchlist',
  templateUrl: './fabric-watchlist.component.html',
  styleUrls: ['./fabric-watchlist.component.css'],
  providers: [FabricPlanningService]
})
export class FabricWatchlistComponent implements OnInit,OnDestroy {
  subscription: Subscription;
  vendorList: Array<Vendor>;
  selectedVendor: Vendor;
  watchListData: Array<FabricWatchList>;
  aramarkItems: Array<FbWatchListItem>;
  aramarkItemList: Array<string>;
  selectedAramarkItem: FbWatchListItem;
  WELabelList: Array<string>;
  gridData: GridDataResult = {
    data: [],
    total: 0
  };
  gridAllData : GridDataResult = {
    data: [],
    total: 0
  };
  sort: SortDescriptor[] = [];
  WEValues: Array<string>;
  show: boolean;
  defaultItem: Vendor;
  public group: any[] = [];
  vendors:Array<Vendor> =[];
  armarkItemsList:Array<FbWatchListItem>=[];

  constructor(
    private _commonService: CommonService,
    private _fabricPlanningService: FabricPlanningService,
    private _errorService: ErrorService,
    private _sessionStorage: SessionStorageService
  ) {
    this.getGridData = this.getGridData.bind(this);
  }

  ngOnDestroy(){
    if(this.gridData.data.length > 0){
    this._sessionStorage.add('fabric_watchlist_load', true);
    this._sessionStorage.add('fabric_watchlist_vendors', this.vendors);
    this._sessionStorage.add('fabric_watchlist_selected_vendor', this.selectedVendor);
    this._sessionStorage.add('fabric_watchlist_selected_item', this.selectedAramarkItem);
    this._sessionStorage.add('fbWlGridData',this.gridAllData.data);
    this._sessionStorage.add('fabric_watchlist_itemList',this.aramarkItems);
    }
  }

  ngOnInit() {
    if(this._sessionStorage.get('fabric_watchlist_load')) {
      this.vendors = this.vendorList = this._sessionStorage.get('fabric_watchlist_vendors');
      this.selectedVendor = this._sessionStorage.get('fabric_watchlist_selected_vendor');
      this.defaultItem = new Vendor(0, 'Select Vendor');
      this.watchListData = new Array<FabricWatchList>();
      this.WELabelList = new Array<string>();
      this.WEValues = new Array<string>();
      this.gridData.data = this.watchListData = this.gridAllData.data = this._sessionStorage.get('fbWlGridData');
      this.WELabelList = this.gridData.data[0] ? this.gridData.data[0].WELabels : [];
      this.aramarkItems = this.armarkItemsList = this._sessionStorage.get('fabric_watchlist_itemList');
      this.selectedAramarkItem = this._sessionStorage.get('fabric_watchlist_selected_item');
      this.onAramarkItemChange(this.selectedAramarkItem);
      this._sessionStorage.removeAll(['fabric_watchlist_load', 'fabric_watchlist_vendors_list',
        'fabric_watchlist_vendors', 'fabric_watchlist_selected_vendor', 'fabric_watchlist_selected_item', 'fbWlGridData',
        'fabric_watchlist_itemList'
      ]);
      this.show = false;
    }
    else {
      this.getVendors();
      this.WELabelList = new Array<string>();
      this.WEValues = new Array<string>();
      this.show = false;
      this.defaultItem = new Vendor(0, 'Select Vendor');
      this.watchListData = new Array<FabricWatchList>();
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

  ExportToPdf(grid: GridComponent) {
    grid.saveAsPDF();
  }

  ExportToExcel(grid: GridComponent) {
    grid.saveAsExcel();
  }

  getVendors() {
    const watchlistType = 'fabric';
    this._fabricPlanningService.getVendors(watchlistType).subscribe(
      data => {
        if (data.length > 0) {
          this.vendorList = data;
          this.vendors = this.vendorList;
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

  getWatchListData(vendorId: number) {
    this.show = true;
    this._fabricPlanningService.getFabricWatchListData(vendorId).subscribe(
      data => {
        this.watchListData = this.gridData.data = this.gridAllData.data = data;
        this.WELabelList = this.gridData.data[0] ? this.gridData.data[0].WELabels : [];
        this.populateAramarkItem();
        if(this.selectedAramarkItem){
          this.onAramarkItemChange(this.selectedAramarkItem);
        }
        this.show = false;
      },
      (err: HttpErrorResponse) => {
        this.show = false;
        this._errorService.error(err);
      }
    );
  }

  onVendorChange(selectedVendor) {
    this.selectedAramarkItem = null;
    if (selectedVendor) {
      const vednorId = selectedVendor.VendorId;
      this.getWatchListData(vednorId);
    } else {
      this.watchListData = this.gridData.data = this.WELabelList = this.aramarkItems = [];
      this.armarkItemsList = this.aramarkItems;
      this.selectedAramarkItem = null;
    }
  }

  populateAramarkItem() {
    this.aramarkItems = new Array<FbWatchListItem>();
    this.watchListData.forEach(x => {
      this.aramarkItems.push(new FbWatchListItem(x.ItemNumber,x.Description));
    });
    this.armarkItemsList = this.aramarkItems;
    if(this.armarkItemsList.length === 1){
      this.selectedAramarkItem = this.armarkItemsList[0];
      this.onAramarkItemChange(this.selectedAramarkItem);
    }
  }

  onAramarkItemChange(selectedAramarkItem) {
    if(selectedAramarkItem)
    {
      this.gridData.data = this.watchListData.filter(x => x.ItemNumber === selectedAramarkItem.ItemNumber);
    }
    else {
      this.gridData.data = this.watchListData;
    }
  }

  getColumnValue(label, item) {
    const id = this.WELabelList.indexOf(label);
    const data = this.watchListData.find(x => x.ItemNumber === item).WEValues[id];
    return data;
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
    const value = this.watchListData.find(x => x.ItemNumber === data.ItemNumber).CSSList[id];
    return value;
  }

  handleVendorFilter(value:string) {
    this.vendors = this.vendorList.filter((s) => (s.VendorName.toLocaleUpperCase()).startsWith(value.toLocaleUpperCase()));
  }
  
  handleAramarkItemFilter(value:string) {
    this.armarkItemsList = this.aramarkItems.filter((s) => (s.ItemNumber.toLocaleUpperCase()).startsWith(value.toLocaleUpperCase()));
  }
}

export class FbWatchListItem {
 
  constructor(
    public ItemNumber : string,
    public Description : string
  ) {
  }
}

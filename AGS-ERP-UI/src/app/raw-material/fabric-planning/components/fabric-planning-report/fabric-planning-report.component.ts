import { Component, OnInit, Input } from '@angular/core';
import { RowClassArgs, GridComponent } from '@progress/kendo-angular-grid';
import { FabricPlanningService } from '../../fabric-planning.service';
import { HttpErrorResponse } from '@angular/common/http';
import { ErrorService } from '../../../../shared/services/error.service';
import { PlanningReport } from '../../models/planning-report';
import { OnDestroy } from '@angular/core/src/metadata/lifecycle_hooks';
import { CommonService } from '../../../../shared/services/common.service';
import { Vendor } from '../../models/vendor';
import { AramarkItem } from '../../models/aramark-item';
import { PlanningGridComponent } from "../planning-grid/planning-grid.component";

@Component({
  selector: 'app-fabric-planning-report',
  templateUrl: './fabric-planning-report.component.html',
  styleUrls: ['./fabric-planning-report.component.css'],
  providers: [FabricPlanningService]
})
export class FabricPlanningReportComponent implements OnInit, OnDestroy {
  @Input() vendorId: number;
  @Input() itemId: number;
  gridReportData: Array<PlanningReport> = [];
  gridData: any[];
  vendorList: Array<Vendor>;
  selectedVendor: Vendor;
  aramarkItems: Array<AramarkItem>;
  aramarkItemList: Array<AramarkItem>;
  selectedAramarkItem: AramarkItem;
  show: boolean;
  defaultItem: Vendor;
  vendors:Array<Vendor> =[];
  aramarkItemsList:Array<AramarkItem> =[];

  headers: Array<string> = ['Week End Date'];
    public headerCells: any = {
        background: '#ff0000',
        textAlign: 'center'
    };

  constructor(
    private _fabricPlanningService: FabricPlanningService,
    private _errorService: ErrorService,
    private _commonService: CommonService
  ) { }

  ngOnInit() {
    this.defaultItem = new Vendor(0, 'Select Vendor');
    this.getVendors();
  }

  ExportToPdf(grid: GridComponent) {
    grid.saveAsPDF();
  }

  ExportToExcel(grid: GridComponent) {
    grid.saveAsExcel();
  }

  getGridData(vendorId, itemId) {
    this.show = true;
      this._fabricPlanningService.getFabricPlanningReportData(vendorId, itemId).subscribe(
        data => {
          if (data) {
            this.gridReportData = data;
            for (let reportData of this.gridReportData){
              this.headers = this.headers.concat(reportData.WEDateLabel);
            }
          }
          this.show = false;
        },
        (err: HttpErrorResponse) => {
          this.show = false;
          this._errorService.error(err);
        }
      );
    }

  getVendors() {
    this.show = true;
    const watchlistType = 'fabric';
    this._fabricPlanningService.getVendors(watchlistType).subscribe(
      data => {
        if (data.length > 0) {
          this.vendorList = data;
          this.vendors = this.vendorList;
          this.populateAramarkItem();
          if(this.vendors.length === 1){
            this.selectedVendor = this.vendors[0];
            this.onVendorChange(this.selectedVendor);
          }
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
    if (selectedVendor) {
      const vendorId = selectedVendor.VendorId;
      this.gridData = [];
      this.selectedAramarkItem = null;
      this.gridReportData = [];
      this.getGridData(vendorId, -1);
      this.getAramarkItemList(vendorId);
    } else {
      this.gridReportData = [];
      this.selectedAramarkItem = null;
      this.aramarkItemList = this.aramarkItems = [];
      this.aramarkItemsList = [];
    }
  }

  populateAramarkItem() {
    this._fabricPlanningService.getAramarkItems().subscribe(
      data => {
        if (data.length > 0) {
          this.aramarkItemList = data;
          this.populateGrid();
          if(this.aramarkItemList.length === 1){
            this.selectedAramarkItem = this.aramarkItemList[0];
            this.onAramarkItemChange(this.selectedAramarkItem);
          }
        }
      },
      (err: HttpErrorResponse) => {
        this._errorService.error(err);
      }
    );
  }

  getAramarkItemList(vendorId: number) {
    if (vendorId === -1) {
      this.aramarkItems = this.aramarkItemList;
      this.aramarkItemsList = this.aramarkItems;
    } else {
      this.aramarkItems = this.aramarkItemList.filter(x => x.VendorId === vendorId);
      this.aramarkItemsList = this.aramarkItems; 
    }
    if(this.aramarkItemsList.length === 1){
      this.selectedAramarkItem = this.aramarkItemsList[0];
      this.onAramarkItemChange(this.selectedAramarkItem);
    }
  }

  onAramarkItemChange(selectedAramarkItem) {
    if(selectedAramarkItem) {
      this.getGridData(this.selectedVendor.VendorId, this.selectedAramarkItem.Id);
    }
    else {
      this.onVendorChange(this.selectedVendor);
    }
  }
  populateGrid() {
    if (this.vendorId !== 0 && this.itemId !== 0) {
      this.getGridData(this.vendorId, this.itemId);
      this.selectedVendor = this.vendors.find(x => x.VendorId === this.vendorId);
      this.selectedAramarkItem = this.aramarkItemList.find(x => x.Id === this.itemId);
      this.getAramarkItemList(this.vendorId);      
      let _aramarkItem = this.aramarkItemList.find(c => c.Id === this.itemId);
      this.selectedVendor = this.vendorList.find(c => c.VendorId === _aramarkItem.VendorId);
    }
  }
  ngOnDestroy() {
    this.vendorId = this.itemId = 0;
    this.selectedVendor = null;
    this.selectedAramarkItem = null;
    this._commonService.Notify({
      key: 'destroyReport',
      value: { 'vendorId': 0, 'itemId': 0 }
    });
  }

  getColumnValue(data, column) {
    return data[column];
  }

  rowColor(context: RowClassArgs) {

    switch (context.dataItem['Week End Date']) {
      case 'Forecast Requirement':
      case 'Forecast Requirement End Inventory':
      case 'Forecast Requirement WOS':
        return {
          foreCast: true
        };
      case 'Historical Average Requirement':
      case 'Historical Requirement End Inventory':
      case 'Historical Requirement WOS':
        return {
          history: true
        };
      default: return {
        default: true
      };
    }
  }

  handleVendorFilter(value:string) {
    this.vendors = this.vendorList.filter((s) => (s.VendorName.toLocaleUpperCase()).startsWith(value.toLocaleUpperCase()));
  }
  
  handleAramarkItemFilter(value:string) {
    this.aramarkItemsList = this.aramarkItems.filter((s) => (s.ItemNumber.toLocaleUpperCase()).startsWith(value.toLocaleUpperCase()));
  }
}

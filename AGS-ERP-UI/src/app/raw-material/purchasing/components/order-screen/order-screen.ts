import { Component, OnInit, ViewEncapsulation } from "@angular/core";
import { GridComponent, GridDataResult } from "@progress/kendo-angular-grid";
import { SortDescriptor } from "@progress/kendo-data-query/dist/es/main";
import { orderBy } from "@progress/kendo-data-query/dist/es/array.operators";
import { HttpErrorResponse } from "@angular/common/http";
import { ErrorService } from "../../../../shared/services/error.service";
import { ConfirmationService } from "../../../../shared/services/confirmation.service";
import { ToastService } from "../../../../shared/services/toast.service";
import { Vendor } from "../../models/vendor";
import { FabricPlanningService } from "../../../fabric-planning/fabric-planning.service";
import { AlertService } from "../../../../shared/services/alert.service";
import { FabricOrder1 } from "../../models/fabric-order1";
import { RowClassArgs } from '@progress/kendo-angular-grid';
import { SessionStorageService } from "../../../../shared/wrappers/session-storage.service";
import { PurchasingService } from "../../purchasing.service";

@Component({
  selector: "app-order-screen",
  templateUrl: "./order-screen.html",
  styleUrls: ["./order-screen.css"],
  providers: [FabricPlanningService,PurchasingService]
})
export class OrderScreenComponent implements OnInit {
  public value: Date = new Date();

  constructor(
    private _fabricPlanningService: FabricPlanningService,    
    private _purchasingService: PurchasingService,
    private _errorService: ErrorService,
    private _toastService: ToastService,
    private _confirmationService: ConfirmationService,
    private _alertService: AlertService
  ) { }

  vendors: Array<Vendor>;
  selectedVendor: Vendor;
  requestedDate: Date = null;
  deliveryDate: Date = null;
  orderGridData: GridDataResult = {
    data: [],
    total: 0
  };
  isSystemYdsChecked: boolean = false;
  sort: SortDescriptor[] = [];
  show: boolean;
  totalOrderCost: number;
  checkAll: boolean = false;
  totalselected: number = 0;
  totalselectedRolls: number = 0;
  totalRolls: number = 0;
  vendorList: Array<Vendor> = [];
  IsSelected: boolean = false;
  orderCost: number;
  totalOrderYards: number;
  openedCommentbox: boolean;
  message = '';
  additionalDetails = '';
  
  defaultvendor: Vendor;
  ngOnInit() {
    this.totalOrderCost = 0;
    this.openedCommentbox = false;
    this.getVendors();
    
    this.defaultvendor = new Vendor(0, 'Select Vendor');
  }

  public rowCallback(context: RowClassArgs) {
    var isSelected = context.dataItem.IsSelected;
    return {
      selected: isSelected
    };
  }

  Total() {
    this.selectedTotalOrderCost;
    this.selectedTotalOrderYards;
  }
  selectedTotalOrderCost() {
    this.totalOrderCost = 0;
    this.orderGridData.data.filter(c => c.IsSelected === true).forEach(d => {
      this.totalOrderCost += d.OrderCost;
    });
    return this.totalOrderCost;
  }
  selectedTotalOrderYards() {
    this.totalOrderYards = 0;
    this.orderGridData.data.filter(c => c.IsSelected === true).forEach(d => {
      this.totalOrderYards += Number(d.OrderYds.replace(/,/g,''));
    });
    return this.totalOrderYards;
  }
  selectedTotalRolls() {
    return this.orderGridData.data.filter(c => c.IsSelected === true).length;
  }
  /**
   * Get Vendors
   */
  getVendors() {
    const watchlistType = "fabric";
    this.show = true;
    this._purchasingService.getVendors(watchlistType).subscribe(
      data => {
        if (data.length > 0) {
          this.vendors = data;
          this.vendors.shift();
          this.vendorList = this.vendors;
          this.show = false;
        }
      },
      (err: HttpErrorResponse) => {
        this.show = false;
        this._errorService.error(err);
      }
    );
  }

  /**
   * Get Items for 'Order Grid' to Place Order
   * @param selectedVendor
   */
  onVendorChange(selectedVendor: Vendor) {
    let vendorId = selectedVendor.VendorId;
    this.IsSelected = false;
    this.show = true;
    this._purchasingService.getFabricOrder(vendorId).subscribe(
      data => {
        data.forEach(element => {
          element.OrderYds=Number(element.OrderYds).toLocaleString();
        });
        if (data.length > 0 && this.isSystemYdsChecked === false) {
          this.orderGridData.data = data;
          this.requestedDate = new Date(data[0].RequestedDate);
          this.deliveryDate = new Date(data[0].DeliveryDate);
          this.totalRolls = data.length;
        }
        else if (data.length > 0 && this.isSystemYdsChecked == true) {
          this.orderGridData.data = data.filter(c => c.SystemYds > 0);
          this.requestedDate = new Date(data[0].RequestedDate);
          this.deliveryDate = new Date(data[0].DeliveryDate);
          this.totalRolls = this.orderGridData.data.length;
        }
        else {
          this.orderGridData.data = [];
          this.requestedDate = null;
          this.deliveryDate = null;
        }
        this.show = false;
      },
      (err: HttpErrorResponse) => {
        this.show = false;
        this._errorService.error(err);
      }
    );
  }

  /**
   * Enable/Disable the 'Place Order' button based on Item Selection in grid
   * @returns boolean
   */
  enablePlaceOrder() {
    return this.orderGridData.data.some(c => c.IsSelected === true);
  }

  /**
   * Get the type of sort & Field to sort
   * @param sort
   */
  sortChange(sort: SortDescriptor[]): void {
    this.sort = sort;
    this.loadRollCases();
  }

  /**
   * Sort the data and assign to grid
   */
  loadRollCases() {
    this.orderGridData = {
      data: orderBy(this.orderGridData.data, this.sort),
      total: this.orderGridData.data.length
    };
  }

  /**
   * Export Grid data in Pdf
   * @param grid
   */
  ExportToPdf(grid: GridComponent) {
    grid.saveAsPDF();
  }

  /**
   * Export Grid data in Excel
   * @param grid
   */
  ExportToExcel(grid: GridComponent) {
    grid.saveAsExcel();
  }

  confirmPlaceOrder() {
    if (this.orderGridData.data.some(c => c.IsSelected === true)) {
      this.openedCommentbox = true;
      this.additionalDetails = '';
      this.message = 'Please confirm to Place order for ' +
        this.orderGridData.data.filter(p => p.IsSelected === true).length +
        ' selected Item(s) of Total Cost  $' +
        Number(this.totalOrderCost.toFixed(2)).toLocaleString() + ' and Total Order Yards ' + Number(this.totalOrderYards.toFixed(2)).toLocaleString();

      // this._confirmationService.confirm({
      //   key: "message",
      //   value: {
      //     message:
      //       "Please confirm to Place order for " +
      //       this.orderGridData.data.filter(p => p.IsSelected === true).length +
      //       " selected Item(s) of Total Cost  $" + Number(this.totalOrderCost.toFixed(2)).toLocaleString() +
      // " and Total Order Yards " + this.totalOrderYards,
      //     continueCallBackFunction: () => this.OnPlaceOrder()
      //   }
      // });
    }
  }
  OnPlaceOrder() {
    let selectedRolls = this.orderGridData.data.filter(
      o => o.IsSelected === true
    );
    if (selectedRolls.length > 0) {
      selectedRolls.forEach(s => {
        s.ActionType = 10;
        s.AdditionalDetails = this.additionalDetails;
      });
      this.openedCommentbox = false;
      this._purchasingService.placeOrder(selectedRolls).subscribe(
        data => {
          if (data) {
            this._toastService.success("Order Is Placed Successfully");
            this.onRefresh();
          }
        },
        (err: HttpErrorResponse) => {
          this.openedCommentbox = false;
          this._toastService.error(
            "Sorry, Couldn't Place Order. Something went wrong"
          );
        }
      );
    }
  }

  onRefresh() {
    this.onVendorChange(this.selectedVendor);
  }
  OnOrderYrdsChange() {
    let row = this.orderGridData.data.filter(c => c.IsSelected == true);
    this.totalselected = 0;
    row.forEach(r => {
      if (r.OrderYds == 0) {
        this.totalselected += 1;
      }
    });
    if (this.totalselected > 0) {
      // this.callAlert("Please enter valid Order Yds.Order Yds should be greater than zero");
      this._toastService.warn('Please enter valid Order Yds.Order Yds should be greater than zero');
    } else {
      this.confirmPlaceOrder();
    }
  }
  // callAlert(msg: string) {
  //   this._alertService.alert({
  //     key: "alertMessage",
  //     value: msg
  //   });
  // }
  handleVendorFilter(value) {
    this.vendorList = this.vendors.filter((s) => (s.VendorName.toLocaleUpperCase()).startsWith(value.toLocaleUpperCase()));
  }
  getConfirmationOnChange(selectedVendor: Vendor) {
    let row = this.orderGridData.data.filter(c => c.IsSelected == true);
    if (row.length > 0) {
      this._confirmationService.confirm(
        {
          key: 'message',
          value: {
            message: 'This Page contain unsaved data. Would you like to perform navigation?<br> Press "OK" to continue or "Cancel" to abort the navigation',
            continueCallBackFunction: () => this.onVendorChange(selectedVendor),
          }
        }
      );
    } else {
      this.onVendorChange(selectedVendor);
    }
  }
  calculateOrderCost(row: FabricOrder1) {
    if (row.IsSelected === true) {
      row.OrderCost = Number(row.POPrice * Number(row.OrderYds.replace(/,/g,'')));
    }
  }
  closeConfirmbox() {
    this.openedCommentbox = false;
  }
}

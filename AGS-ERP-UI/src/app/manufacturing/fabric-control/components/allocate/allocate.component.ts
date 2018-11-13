import { Component, OnInit } from '@angular/core';
import { AllocateModel } from '../../models/allocate.model';
import { FabricControlService } from '../../fabric-control.service';
import { FabricControlModel } from '../../models/fabricControl.model';
import { CutOrder } from '../../models/cut-order.model';
import { HttpErrorResponse } from '@angular/common/http';
import { AllocateFilterRollCase } from '../../models/allocateFilterRollCase.model';
import { Lot } from '../../models/lot.model';
import { AllocateResult } from '../../models/allocateResult.model';
import { ControlModel } from "../../models/fabric-control.model";
import { SortDescriptor } from '@progress/kendo-data-query/dist/es/main';
import { GridDataResult } from '@progress/kendo-angular-grid';
import { orderBy } from '@progress/kendo-data-query/dist/es/array.operators';
import { SuccessService } from '../../../../shared/services/success.service';
import { ErrorService } from '../../../../shared/services/error.service';
import { ConfirmationService } from '../../../../shared/services/confirmation.service';
import { AlertService } from '../../../../shared/services/alert.service';
import { ToastService } from '../../../../shared/services/toast.service';
import { Allocation } from '../../models/allocation.model';
import { Vendor } from '../../models/vendor.model';

@Component({
  selector: 'app-allocate',
  templateUrl: './allocate.component.html',
  styleUrls: ['./allocate.component.css'],
  providers: [FabricControlService]
})
export class AllocateComponent implements OnInit {
  fabricControlData: FabricControlModel;
  allocationTypes: Array<Allocation>;
  cutOrderList: Array<CutOrder> = [];
  selectedCutOrder: CutOrder;
  filterGridData: Array<AllocateFilterRollCase> = [];
  // resultGridData: Array<AllocateResult> = [];
  resultGridData: Array<ControlModel> = [];
  sewPlant: string;
  cutPlant: string;
  style: string;
  styleDesc: string;
  color: string;
  cutQuantity: string;
  fiscalWeekNumber: string;
  cutDate: string;
  coo: string;
  // currentSelectionGrid: Array<AllocateResult> = [];
  currentSelectionGrid: Array<ControlModel> = [];
  show: boolean;
  selected: string = null;
  isAllocatedChecked: boolean;
  rowNumber: number;
  isAllocatedRoll: boolean;
  public filterSort: SortDescriptor[] = [];
  public filterGridView: GridDataResult = { data: [], total: 0 }
  public resultSort: SortDescriptor[] = [{ 'field': 'LotNumber', 'dir': 'desc' }];
  public resultGridView: GridDataResult = { data: [], total: 0 }
  public currentSelectionSort: SortDescriptor[] = [];
  public currentSelectionGridView: GridDataResult = { data: [], total: 0 }
  public dropdownlist: any;
  cutOrders: Array<CutOrder> = [];
  LotNumberList:Array<Lot> =[];
  vendorList:Array<Vendor>=[];

  constructor(
    private _fabricControlService: FabricControlService,
    private _successService: SuccessService,
    private _errorService: ErrorService,
    private _confirmationService: ConfirmationService,
    private _alertService: AlertService,
    private _toastService: ToastService,
  ) { }

  ngOnInit() {
    this.getUnAllocatedCutOrders();
    this.allocationTypes = this._fabricControlService.getAllocationTypes();
    this.fabricControlData =
      new FabricControlModel(null, null, null, null, null, null, null, null, null, null, null, null, null, null, [], null);
  }

  /**
     * Get the cut orders which are unallocated.
    */
  getUnAllocatedCutOrders() {
    this.show = true;
    this._fabricControlService.getCutUnAllocatedOrders().subscribe(
      data => {
        this.filterGridData = [];
        this.resultGridData = [];
        this.currentSelectionGrid = [];
        this.cutOrderList = [];
        this.sewPlant = null;
        this.cutPlant = null;
        this.style = null;
        this.styleDesc = null;
        this.color = null;
        this.cutQuantity = null;
        this.fiscalWeekNumber = null;
        this.cutDate = null;
        this.selectedCutOrder = null;
        this.cutOrderList = data;
        this.selectedCutOrder = undefined;
        this.filterGridView.data = [];
        this.resultGridView.data = [];
        this.currentSelectionGridView.data = [];
        if (this.cutOrderList.length == 1) {
          this.selectedCutOrder = this.cutOrderList[0];
          this.onCutOrderChange(this.cutOrderList[0]);
        }
        this.cutOrders = this.cutOrderList;
        this.show = false;
      },
      (err: HttpErrorResponse) => {
        this.show = false;
        this._errorService.error(err);
      });
  }

  /**
   * Notify the user when selected different cut order
   * @param cutOrder (selected cutOrder)
   */
  getConfirmationOnChangeOfCutOrder(cutOrder: Event) {
      if (this.currentSelectionGridView.data.length > 0 && cutOrder !== undefined) {
        this._confirmationService.confirm(
          {
            key: 'message',
            value: {
              message: 'This Page contain unsaved data. Would you like to perform navigation?<br> Press "OK" to continue or "Cancel" to abort the navigation',
              continueCallBackFunction: () => this.onCutOrderChange(cutOrder),
              cancelCallBackFunction: () => this.onCutOrderChangeCancel()
            }
          }
        );
      } else {
        this.onCutOrderChange(cutOrder);
      }      
    }

  onCutOrderChangeCancel() {
    let selectedCutOrder: CutOrder = this.selectedCutOrder;
    this.selectedCutOrder = new CutOrder(null, null, selectedCutOrder.CutOrder, selectedCutOrder.SewPlant, selectedCutOrder.CutPlant, selectedCutOrder.Style, selectedCutOrder.StyleDesc, null, null, null, null);
    //this.selectedCutOrder = new CutOrder(47783, 47783, "65513V", "MONC", "MONC", "GS0099", "S/S Oxford Shirt- Male banded", "LTBL", 4, "2017-11-10T00:00:00", "2017-11-10T00:00:00");
  }
  /**
   * Get Data for 'Filter grid' and assign the data for fileds respected to respective cut order in header section
   * @param cutOrder (selected cutOrder
   */
  onCutOrderChange(cutOrder) {
    if(cutOrder !== undefined)
    {
      this.filterGridView.data = [];
      this.resultSort = [{ 'field': 'LotNumber', 'dir': 'desc' }];
      this.selectedCutOrder = cutOrder;
      if (cutOrder) {
        this.rowNumber = undefined;
        this.show = true;
        this._fabricControlService.getFilterData(cutOrder.CutHbrId).subscribe(
          data => {
            this.sewPlant = cutOrder.SewPlant;
            this.cutPlant = cutOrder.CutPlant;
            this.style = cutOrder.Style;
            this.styleDesc = cutOrder.StyleDesc;
            this.color = cutOrder.Color;
            this.cutQuantity = String(cutOrder.CutQuantity);
            this.fiscalWeekNumber = cutOrder.FiscalWeek;
            this.cutDate = cutOrder.CutDate;
            this.filterGridData = data;
            this.filterGridData.forEach(rollCase => {
              rollCase.Available = null;
              rollCase.LotNumbers = [];
              rollCase.IsSelected = false;
              if (rollCase.Vendors.length == 1) {
                rollCase.Vendor = rollCase.Vendors[0];
                this.onVendorChange(rollCase.Vendors[0], rollCase, this.filterGridData.indexOf(rollCase));
              }
              this.vendorList = rollCase.Vendors;
              rollCase.BOM = rollCase.BOMYards;
              if (this.isAllocatedChecked) {
                rollCase.Marker = rollCase.MarkerYards;
                if (rollCase.Marker === 0) {
                  rollCase.IsMarkerEnabled = false;
                }
                else {
                  rollCase.IsMarkerEnabled = true;
                }
              }
              this.filterGridView.data = this.filterGridData;
            });
            this.resultGridData = [];
            this.currentSelectionGrid = [];
            this.resultGridView.data = [];
            this.currentSelectionGridView.data = [];
            if (this.isAllocatedChecked) {
              this._fabricControlService.getAllocatedRollsForCurrentGrid(cutOrder.CutHbrId).subscribe(
                dataForCurrentGrid => {
                  dataForCurrentGrid.forEach(element => {
                    if (element.Status === 'Allocated' || element.Status === 'Pulled') {
                      this.isAllocatedRoll = true;
                      this.currentSelectionGrid.push(element);
                      this.currentSelectionGridView.data = this.currentSelectionGrid;
                    }
                    dataForCurrentGrid.forEach(s => {
                      let allocatedSum = this.calculateSumOfAllocatedInCurrentSelectionGrid(s);
                      let filterRoll = this.filterGridView.data.find(r => r.AramarkItem == s.ItemCode);
                      filterRoll.AllocatedQty = allocatedSum;
                      filterRoll.MarkerDiff = filterRoll.Marker - filterRoll.AllocatedQty;
                      filterRoll.BOMDiff = filterRoll.BOM - filterRoll.AllocatedQty;
                    });
                  });
                },
                (err: HttpErrorResponse) => {
                  this.show = false;
                  this._errorService.error(err);
                }
              )
            }
            this.show = false;
          },
          (err: HttpErrorResponse) => {
            this.show = false;
            this._errorService.error(err);
          });
      }      
    }
  }

  updateBOMDiff(rollCase) {
    if (rollCase.BOM) {
      rollCase.BOMDiff = rollCase.BOM - (rollCase.Allocated || 0);
    }
  }

  updateMarkerDiff(rollcase) {
    if (rollcase.Allocated) {
      rollcase.MarkerDiff = rollcase.Marker - (rollcase.Allocated || 0)
    }
  }

  /**
   * Populate data for 'Lot' dropdown based on selected vendor for the rollcase
   * @param vendor (selected vendor)
   * @param rollCase (roll case for which vendor has selected)
   */
  onVendorChange(vendor, rollCase, rowIndex) {
    if(vendor!==undefined)
    {
      rollCase.CountryOfOrigin = vendor.COO;     
      rollCase.VendorSiteId = vendor.VendorSiteId;
      rollCase.VendorSite = vendor.VendorSite;
      rollCase.LotNumbers = [];
      if (vendor.LotNumbers.length > 1) {
        let avSum = 0;
        vendor.LotNumbers.forEach(element => {
          avSum = avSum + element.Available;
          this.LotNumberList = vendor.LotNumbers;
        });
        rollCase.LotNumbers.push(new Lot('<All>', avSum, null, null, null));
        rollCase.LotNumbers = rollCase.LotNumbers.concat(vendor.LotNumbers);
        rollCase.DefaultLot = rollCase.LotNumbers.find(l => l.LotNumber == '<All>');
        this.onLotChange(rollCase.DefaultLot, rollCase, this.filterGridView.data.indexOf(rollCase));
        this.LotNumberList = rollCase.LotNumbers;
      }
      if (vendor.LotNumbers.length === 1) {
        rollCase.LotNumbers = vendor.LotNumbers;
        rollCase.DefaultLot = vendor.LotNumbers[0];
        this.LotNumberList = rollCase.LotNumbers;
        this.onLotChange(rollCase.DefaultLot, rollCase, this.filterGridView.data.indexOf(rollCase));
      }
  }
  }

  onLotChange(lot, rollCase, rowNum) {
    if(lot!==undefined)
    {
      rollCase.LotNumber = lot.LotNumber;
      rollCase.Available = lot.Available;
      rollCase.SelectedLots = [];
      this.setValuesForSelectedLotArry(rollCase);
      if (rowNum == this.rowNumber) {
        this.getUnAllocatedResultData(rollCase, );
      }
    }
  }

  getResultData(event: Event, rollCase, rowNum) {
    const errorInfo: Array<string> = [];
    let rollError: string;
    if (!rollCase.VendorSiteId) {
      errorInfo.push('Vendor');
    }
    if (rollCase.Marker === undefined || rollCase.Marker < 1) {
      errorInfo.push('Marker should be greater than zero');
    }
    rollError = errorInfo.join(', ');
    if (rollError) {
      this._toastService.error('Kindly enter the required field(s):  ' + rollError);
      event.preventDefault();
    } else {
      this.rowNumber = rowNum;
      this.setValuesForSelectedLotArry(rollCase);
      this.getUnAllocatedResultData(rollCase);
    }
  }

  setValuesForSelectedLotArry(rollCase) {
    rollCase.SelectedLots = [];
    if (rollCase.LotNumber === '<All>') {
      rollCase.SelectedLots.push('-1');
    }
    else {
      rollCase.SelectedLots.push(rollCase.LotNumber);
    }
  }

  getUnAllocatedResultData(rollCase) {
    this.show = true;
    this._fabricControlService.getAllocatedResultData(rollCase).subscribe(
      data => {
        data.forEach(element => {
          if (element.Consignment) {
            element.ConsignmentField = 'Yes';
          }
          if (!element.Consignment) {
            element.ConsignmentField = 'No';
          }
          if (this.checkTheRollInCurrentSelectionGrid(element)) {
            element.IsAddedToCurrentSelection = true;
          }
          element.MarkerQty = rollCase.Marker;
          this.resultGridData.push(element);
          this.resultGridView.data = this.resultGridData;
        });
        this.resultGridData = data;
        this.resultGridView.data = this.resultGridData;
        this.show = false;
      },
      (err: HttpErrorResponse) => {
        this.show = false;
        this._errorService.error(err);
      }
    );
  }

  checkTheRollInCurrentSelectionGrid(roll: ControlModel) {
    let rollInCurrentGrid = this.currentSelectionGridView.data.find(r => r.RollCaseId == roll.RollCaseId);
    if (rollInCurrentGrid) {
      return true;
    }
    else {
      return false;
    }
  }

  enableDeAllocate() {
    return this.currentSelectionGridView.data.some(c => c.IsSelected === true);
  }

  enableAllocate() {
    return this.currentSelectionGridView.data.some(r => r.Status === undefined);
  }
  onSelectChangeAllocatedYards(rollCase) {
    if (rollCase.IsSelected) {
      let number: number = Number(rollCase.AvailableYards);
      rollCase.AllocationQty = number.toLocaleString();
    }
    else {
      rollCase.AllocationQty = 0;
    }

  }

  populateCurrentSelectionGrid(resultRollCase: ControlModel) {
    let allocatedyards = resultRollCase.AllocationQty.toString().replace(/,/g, "");
    if (Number(allocatedyards) === 0) {
      this._toastService.error("Allocated yards should be greater than zero");
    }
    if (Number(allocatedyards) > resultRollCase.AvailableYards) {
      this._toastService.error("Allocated yards cannot be greater than Available yards");
    }
    if (Number(allocatedyards) > 0 && Number(allocatedyards) <= resultRollCase.AvailableYards) {
      resultRollCase.AllocationQty = Number(allocatedyards);
      this.currentSelectionGrid.push(resultRollCase);
      this.currentSelectionGridView.data = this.currentSelectionGrid;
      resultRollCase.IsAddedToCurrentSelection = true;
      let allocatedSum = this.calculateSumOfAllocatedInCurrentSelectionGrid(resultRollCase);
      let filterRoll = this.filterGridView.data.find(r => r.AramarkItem == resultRollCase.ItemCode);
      filterRoll.AllocatedQty = allocatedSum;
      filterRoll.MarkerDiff = filterRoll.Marker - filterRoll.AllocatedQty;
      filterRoll.BOMDiff = filterRoll.BOM - filterRoll.AllocatedQty;
    }

  }

  calculateSumOfAllocatedInCurrentSelectionGrid(rollCase) {
    let sumAllocated = 0;
    let currentSelectionRolls = this.currentSelectionGridView.data.filter(r => r.ItemCode == rollCase.ItemCode);
    currentSelectionRolls.forEach(element => {
      sumAllocated = Number(sumAllocated) + Number(element.AllocationQty);
    });
    return sumAllocated;
  }

  confirmRemovalFromSelectionGrid(rollCase) {
    this._confirmationService.confirm({
      key: 'message',
      value: {
        message: 'Are you sure you want to remove roll from current selection?',
        continueCallBackFunction: () => this.removeCurrentSelectionFromGrid(rollCase)
      }
    });
  }


  removeCurrentSelectionFromGrid(rollCase) {
    const index = this.currentSelectionGridView.data.indexOf(rollCase);
    this.currentSelectionGridView.data.splice(index, 1);
    const resultRoll = this.resultGridView.data.find(r => r.RollCaseId == rollCase.RollCaseId);
    if (resultRoll) {
      resultRoll.IsAddedToCurrentSelection = false;
      resultRoll.AllocationQty = 0;
      resultRoll.IsSelected = false;
    }
    const allocatedSum = this.calculateSumOfAllocatedInCurrentSelectionGrid(rollCase);
    let filterRoll = this.filterGridView.data.find(r => r.AramarkItem == rollCase.ItemCode);
    if (this.currentSelectionGridView.data.length > 0) {
      filterRoll.AllocatedQty = allocatedSum;
      filterRoll.MarkerDiff = filterRoll.Marker - filterRoll.AllocatedQty;
      filterRoll.BOMDiff = filterRoll.BOM - filterRoll.AllocatedQty;
    }
    if (allocatedSum == 0) {
      filterRoll.AllocatedQty = undefined;
      filterRoll.MarkerDiff = undefined;
      filterRoll.BOMDiff = undefined;
    }
  }

  onAllocatedYardsChange(rollCase) {
    if (Number(rollCase.AllocationQty) < 1) {
      this._toastService.error('Allocated should be greater than zero');
    }
    else {
      if (rollCase.AllocationQty > rollCase.AvailableYards) {
        this._toastService.error('Allocated cannot be greater than Available yards');
      }
    }
  }

  getTotalAllocatedYards() {
    let totalAllocatedYards = 0;
    if (this.currentSelectionGridView.data.length > 0) {
      this.currentSelectionGridView.data.forEach(c => {
        let tot = c.AllocationQty.toString().replace(/,/g, "");
        totalAllocatedYards += Number(tot);
      });
    }
    return totalAllocatedYards;
  }

  onCheckAll(event) {
    this.fabricControlData.FabricRollCases.forEach(c => c.IsSelected = event.target.checked);
  }

  toggleAllocatedCheckBox() {
    this.isAllocatedChecked = !this.isAllocatedChecked;
  }

  confirmationToDiscardChanges(event) {
    if (this.isAllocatedChecked === false || this.isAllocatedChecked === undefined) {
      const count = this.currentSelectionGridView.data.length;
      if (count > 0) {
        this._confirmationService.confirm({
          key: 'message',
          value: {
            message: 'This Page contain unsaved data. Would you like to perform navigation?<br> Press "OK" to continue or "Cancel" to abort the navigation',
            continueCallBackFunction: () => this.showAllocatedCutOrders(),
            cancelCallBackFunction: () => this.toggleAllocatedCheckBox()
          }
        });
      } else {
        this.showAllocatedCutOrders();
      }
    }
    if (this.isAllocatedChecked === true) {
      const count = this.currentSelectionGrid.length;
      if (count > 0) {
        this._confirmationService.confirm({
          key: 'message',
          value: {
            message: 'This Page contains unsaved data. Would you like to perform navigation?<br> Press "OK" to continue or "Cancel" to abort the navigation',
            continueCallBackFunction: () => this.getUnAllocatedCutOrders(),
            cancelCallBackFunction: () => this.toggleAllocatedCheckBox()
          }
        });
      } else {
        this.getUnAllocatedCutOrders();
      }
    }
  }

  DiscardChange(event: Event) {
    event.preventDefault;
  }
  showAllocatedCutOrders() {
    this.show = true;
    this._fabricControlService.getAllocatedCutOrders().subscribe(
      data => {
        this.filterGridData = [];
        this.resultGridData = [];
        this.currentSelectionGrid = [];
        this.cutOrderList = [];
        this.sewPlant = null;
        this.cutPlant = null;
        this.style = null;
        this.styleDesc = null;
        this.color = null;
        this.cutQuantity = null;
        this.fiscalWeekNumber = null;
        this.cutDate = null;
        this.selectedCutOrder = null;
        this.cutOrderList = data;
        this.show = false;
        this.filterGridView.data = [];
        this.resultGridView.data = [];
        this.currentSelectionGridView.data = [];
        this.cutOrders = this.cutOrderList;
      },
      (err: HttpErrorResponse) => {
        this.show = false;
        this._errorService.error(err);
      }
    );

  }

  onAllocate(roll) {
    let a = this.currentSelectionGridView.data.filter(r => r.Status === undefined);
    // const count = this.currentSelectionGrid.length;
    if (a.length > 0) {
      const count = a.length
      this._confirmationService.confirm({
        key: 'message',
        value: {
          message: 'Please confirm the allocation of ' + count + ' roll(s).',
          continueCallBackFunction: () => this.allocateRolls()
        }
      });
    }
    else {
      this._toastService.error("No rolls to Allocate");
    }
  }

  allocateRolls() {
    let rollsToAllocate: Array<ControlModel> = [];
    let unAllocatedRolls = this.currentSelectionGridView.data.filter(r => r.Status === undefined);
    unAllocatedRolls.forEach(element => {
      let filterRoll = this.filterGridView.data.find(r => r.AramarkItem == element.ItemCode);
      let allocateRollObj: ControlModel = new ControlModel();
      allocateRollObj = element;
      allocateRollObj.ActionType = 10;
      allocateRollObj.RMVendorSiteId = element.VendorSiteId;
      // allocateRollObj.MarkerYeild = filterRoll.Marker;
      // allocateRollObj.COO = filterRoll.CountryOfOrigin;
      // allocateRollObj.CutHDRId = element.CutHDRId;
      // allocateRollObj.CutItemId = element.CutItemId;
      // allocateRollObj.RollCaseId = element.RollCaseId;
      // allocateRollObj.LotNumber = element.LotNumber;
      // allocateRollObj.AllocationQty = filterRoll.AllocatedQty;
      // allocateRollObj.RMVendorId = 
      // allocateRollObj.RMVendorSiteId = 
      rollsToAllocate.push(allocateRollObj);
    });
    this._fabricControlService.AllocateTheCutOrders(rollsToAllocate).subscribe(
      data => {
        let isAllocated = data;
        if (isAllocated) {
          this._toastService.success('Rolls are Allocated Successfully');
          this.currentSelectionGrid = [];
          this.resultGridData = [];
          this.filterGridData = [];
          if (this.isAllocatedChecked) {
            this.showAllocatedCutOrders();
          }
          else {
            this.getUnAllocatedCutOrders();
          }
          this.sewPlant = null;
          this.cutPlant = null;
          this.style = null;
          this.styleDesc = null;
          this.color = null;
          this.cutQuantity = null;
          this.fiscalWeekNumber = null;
          this.cutDate = null;
          this.filterGridView.data = [];
          this.resultGridView.data = [];
          this.currentSelectionGridView.data = [];
        }
        else {
          this._toastService.error("Sorry, Couldn't Allocate rolls. Something went wrong")
        }
      },
      (err: HttpErrorResponse) => {
        let errDesc = err.error;
        if (typeof errDesc === "string") {
          errDesc = JSON.parse(errDesc);
        }
        this._errorService.error(err);
      }
    );

  }

  onDeAllocate() {
    let a = this.currentSelectionGridView.data.filter(r => r.Status === undefined);
    if (a.length > 0) {
      this._toastService.error("Please Complete Allocation First")
    }
    else {
      const count = this.currentSelectionGridView.data.filter(r => r.Status === 'Allocated' && r.IsSelected === true).length;
      if (count > 0) {
        this._confirmationService.confirm({
          key: 'message',
          value: {
            message: 'Please confirm the de-allocation of ' + count + ' roll(s).',
            continueCallBackFunction: () => this.deAllocateRolls()
          }
        });
      }
      else {
        this._toastService.error("No Rolls to De-Allocate");
      }
    }
  }

  deAllocateRolls() {
    let deAllocationRolls = this.currentSelectionGridView.data.filter(r => r.Status === 'Allocated' && r.IsSelected === true);
    let rollsToDeAllocate = [];
    deAllocationRolls.forEach(element => {
      let deAllocateRollObj: ControlModel = new ControlModel();
      deAllocateRollObj = element;
      deAllocateRollObj.ActionType = 30;
      rollsToDeAllocate.push(deAllocateRollObj);
    });
    this._fabricControlService.DeAllocateTheCutOrders(rollsToDeAllocate).subscribe(
      data => {
        let isDeAllocated = data;
        if (isDeAllocated) {
          this._toastService.success('Rolls are De-Allocated Successfully');
          this.currentSelectionGrid = [];
          this.resultGridData = [];
          this.filterGridData = [];
          this.showAllocatedCutOrders();
          this.sewPlant = null;
          this.cutPlant = null;
          this.style = null;
          this.styleDesc = null;
          this.color = null;
          this.cutQuantity = null;
          this.fiscalWeekNumber = null;
          this.cutDate = null;
          this.filterGridView.data = [];
          this.resultGridView.data = [];
          this.currentSelectionGridView.data = [];
        }
        else {
          this._toastService.error("Sorry, Couldn't DeAllocate rolls. Something went wrong")
        }
      },
      (err: HttpErrorResponse) => {
        let errDesc = err.error;
        if (typeof errDesc === "string") {
          errDesc = JSON.parse(errDesc);
        }
        this._errorService.error(err);
      }
    );
  }

  onChangeAllocationType(allocationType: Allocation, allocate: AllocateModel) {
    allocate.AllocatonType = allocationType;
    if (allocate.AllocatonType.Id === 1) {
      allocate.Allocated = allocate.Yards;
    }
  }

  getexpandmode(event) {
    var target = event.currentTarget;
    var expandmode = target.attributes.class.value;
    if (expandmode.search('k-state-expanded') == -1) {
      this.selected = 'active';
    } else {
      this.selected = null;
    }

  };

  callback(event: Event) {
    event.preventDefault();
  }

  filterSortChange(sort: SortDescriptor[]): void {
    this.filterSort = sort;
    this.sortFilterDataRolls();
  }
  sortFilterDataRolls() {
    this.filterGridView = {
      data: orderBy(this.filterGridView.data, this.filterSort),
      total: this.filterGridView.data.length
    };
  }
  resultSortChange(sort: SortDescriptor[]): void {
    this.resultSort = sort;
    this.sortResultDataRolls();
  }
  sortResultDataRolls() {
    this.resultGridView = {
      data: orderBy(this.resultGridView.data, this.resultSort),
      total: this.resultGridView.data.length
    };
  }
  currentSelectionSortChange(sort: SortDescriptor[]): void {
    this.currentSelectionSort = sort;
    this.sortCurrentSelectionDataRolls();
  }
  sortCurrentSelectionDataRolls() {
    this.currentSelectionGridView = {
      data: orderBy(this.currentSelectionGridView.data, this.currentSelectionSort),
      total: this.currentSelectionGridView.data.length
    };
  }
  public onClose(event: any) {
    // (close)="onClose($event)" // add text to call onClose function
    event.preventDefault();
    //Close the list if the component is no longer focused
    setTimeout(() => {
      if (!this.dropdownlist.wrapper.nativeElement.contains(document.activeElement)) {
        this.dropdownlist.toggle(false);
      }
    });
  }
  handleFilter(value: string) {
    this.cutOrders = this.cutOrderList.filter((s) => (s.CutOrder).startsWith(value.toLocaleUpperCase()));
  }

  handleLotNumberFilter(value:string,row:AllocateFilterRollCase){
    row.LotNumbers = this.LotNumberList.filter(s=>s.LotNumber.startsWith(value));
  }

  handleVendorFilter(value:string,row:AllocateFilterRollCase){
    row.Vendors = this.vendorList.filter(v=>v.Name.toLocaleUpperCase().startsWith(value.toLocaleUpperCase()));
  }
}

import { Component,OnInit,Input,Output,EventEmitter } from '@angular/core';
import { OPTDropDownComponent,DropDownModel } from "../../../../../shared/components/opt-combobox/opteamix-dropdown.component";
 


import { MaintainRolls } from "../../models/maintain-rolls";
import { PhysicalAdjustmentsService } from "../../physical-adjustments.service";
import { RollCasesModel } from "../../models/roll-case.model";
import { FarbricDetailsModel } from "../../models/fabric-details.model";
import { LocalStorageService } from "../../../../../shared/wrappers/local-storage.service";
import { VendorDetails } from "../../models/vendor-details.model";
import { ReasonDetails } from "../../models/reason-details.model";
import { ColorDetails } from "../../models/color-details.model";
import { AramarkItemDetails } from "../../models/aramark-item-details.model";
import { InventoryControl } from "../../models/inventory-control.model";
import { BinLocationsModel } from "../../models/bin-locations.model";
import { LocationsModel } from "../../models/locations.model";
import { HttpErrorResponse } from "@angular/common/http";
import { ErrorModel } from "../../../../../shared/models/error.model";
import { ErrorService } from "../../../../../shared/services/error.service";
import { ConfirmationService } from "../../../../../shared/services/confirmation.service";
import { SuccessService } from "../../../../../shared/services/success.service";
import { AlertService } from "../../../../../shared/services/alert.service";
import { GridComponent, GridDataResult } from "@progress/kendo-angular-grid";
import { SortDescriptor } from "@progress/kendo-data-query/dist/es/main";
import { orderBy } from "@progress/kendo-data-query/dist/es/array.operators";
import { ToastService } from '../../../../../shared/services/toast.service';


@Component({
  selector: "app-maintain-rolls",
  templateUrl: "./maintain-rolls.component.html",
  styleUrls: ["./maintain-rolls.component.css"],
  providers: [PhysicalAdjustmentsService, RollCasesModel]
})
export class MaintainRollsComponent implements OnInit {
  show = false;
  constructor(
    private _errorService: ErrorService,
    private _physicalAdjustmentsService: PhysicalAdjustmentsService,
    private _rollCase: RollCasesModel,
    private _localStorage: LocalStorageService,
    private _confirmationService: ConfirmationService,
    private _successService: SuccessService,
    private _alertService: AlertService,
    private _toastService: ToastService
  ) { }

  error: ErrorModel;
  public opened: boolean = false;
  public locations: Array<LocationsModel> = [];
  public binLocations: Array<BinLocationsModel>;
  public destinationBinLocations: Array<BinLocationsModel> = [];
  public rollCases: Array<RollCasesModel> = [];
  public fabricData: Array<FarbricDetailsModel> = [];
  public vendorData: Array<VendorDetails> = [];
  public colorData: Array<ColorDetails> = [];
  public aramarkItemData: Array<AramarkItemDetails> = [];
  //public reasonData: Array<ReasonDetails> = [];
  public reasonDataAdd: Array<ReasonDetails> = [];
  public reasonDataDelupd: Array<ReasonDetails> = [];
  public selectedLocation: LocationsModel;
  public selectedBinLocation: BinLocationsModel;
  public selectedDestinationBinLocation: BinLocationsModel;
  public DestinationBinFullMessage: string;
  public sort: SortDescriptor[] = [];
  public gridView: GridDataResult = {data: [],total : 0};
  aramarkList:Array<AramarkItemDetails>=[];
  destinationBinLocationList: Array<BinLocationsModel> = [];

  ngOnInit() {
    this.getLocations();
   
  }

  // Get the Locations for 'Location' dropdown data
  getLocations() {
    this._physicalAdjustmentsService.getLocation().subscribe(
      data => {
        if (data.length === 1) {
          this.selectedLocation = data[0];
          this.onLocationChange(data[0]);
        }
        this.locations = data;
      },
      (err: HttpErrorResponse) => {
        this._errorService.error(err);
      }
    );
  }

  onLocationChange(selectedLocation) {
    this.getBinLocations(selectedLocation.Id);
  }

  // Get the BinLocation for 'Bin' dropdown data
  getBinLocations(locationId: number) {
    this._physicalAdjustmentsService.getBinLocation(locationId).subscribe(
      data => {
        this.binLocations = data;        
        this.getFabricData();
      },
      (err: HttpErrorResponse) => {
        this._errorService.error(err)
      }
    );
  }

  // Get the RollCases On change of Bin
  onBinLocationChange(selectedBin) {
    this.selectedBinLocation = selectedBin;
    if(selectedBin) {
      this.getRollCases(selectedBin.Id);
    }
    else {
      this.rollCases = [];
      this.loadRollCases();
    }
  }

  // Refresh the Grid Data
  onRefresh() {
    this.getColors();
    this.getReasonDetails(1);
    this.getRollCases(this.selectedBinLocation.Id);
  }

  // Get the Rollcases for grid based on selected Location & Bin
  getRollCases(binLocationId: number) {
    this.show = true;
    this._physicalAdjustmentsService.getRollCases(binLocationId).subscribe(
      data => {
        data.forEach(c => {
          c.AramarkItemList = this.aramarkItemData;
          c.AramarkItemCode = this.aramarkItemData.find(d => d.ItemCode === c.AramarkItemCode);
          c.VendorList = this.getVendorsByItemId(c.AramarkItemCode.Id);
          c.ColorCode = this.colorData.find(e => e.ColorCode == c.ColorCode);
          c.Vendor = this.vendorData.find(e => e.Id === c.VendorId);
          c.IsValid = true;
          c.OnHand = c.OnHand.toLocaleString();
          c.Defective = c.Defective.toLocaleString();
          c.ColorDetailsList = this.colorData;
        });
        this.rollCases = data;
        this.rollCases = this.sortRollCases(this.rollCases);
        this.show = false;
        this.loadRollCases();
      },
      (err: HttpErrorResponse) => {
        this.show = false;
        this._errorService.error(err);
      }
    );
    this.show = false
  }

  // Get the Fabric data
  getFabricData() {
    let fabricData = this._localStorage.get("fabric_items");
    if (fabricData) {
      if(typeof fabricData === "string"){
        this.fabricData = JSON.parse(fabricData);
      }
      this.getColors();
      this.getReasonDetails(1);
    } else {
      this._physicalAdjustmentsService.getFabricdetails().subscribe(
        data => {
          this.fabricData = data;
          this._localStorage.add("fabric_items", JSON.stringify(data));
          this.getColors();
          this.getReasonDetails(1);
        },
        (err: HttpErrorResponse) => {
          this._errorService.error(err);
        }
      );
    }
  }
  // Get the Colors for 'Color' dropdown
  getColors() {
    let colorData = this._localStorage.get("color_data");
    if (colorData) {
      if(typeof colorData === "string"){
      this.colorData = JSON.parse(colorData);
      }
      this.getAramarkItems();
      this.getVendorDetails();
    }
    else {
      let colorIds: string[] = [];
      this.fabricData.forEach(c => {
        if (colorIds.indexOf(c.ColorId) < 0) {
          colorIds.push(c.ColorId);
        }
      });
      colorIds.forEach(c => {
        let fabric = this.fabricData.find(d => d.ColorId == c);
        let colorInfo = fabric.ColorCode + " - " + fabric.Color;
        this.colorData.push(new ColorDetails(fabric.ColorId, fabric.Color, fabric.ColorCode, colorInfo));      
      });
      this._localStorage.add("color_data", JSON.stringify(this.colorData));
      this.getAramarkItems();
      this.getVendorDetails();
    }
  }

  // Get the relevant data for 'Aramark Item' dropdown On change of color
  onColorChange(selectedColor: ColorDetails, rollCase: RollCasesModel) {
    if(selectedColor !==undefined)
    {
      rollCase.AramarkItemCode = null;
      rollCase.FiberContent = null;
      rollCase.Vendor = null;
      rollCase.VendorItem = null;
      rollCase.Width = null;
      if(selectedColor)
      {
        rollCase.ColorCode = selectedColor;
        rollCase.AramarkItemList = new Array<AramarkItemDetails>();
        this.fabricData.filter(c => c.ColorId == selectedColor.ColorId).forEach(c => {
          rollCase.AramarkItemList.push(new AramarkItemDetails(c.Id, c.ItemCode, c.ItemName, c.Width, c.FiberContent));
        });
      }
      if (rollCase.AramarkItemList.length === 1) {
        rollCase.AramarkItemCode = rollCase.AramarkItemList[0];      
        this.onAramarkItemCodeChange(rollCase.AramarkItemCode, rollCase);
      }      
    }
  }

  //Get all data for 'Aramark Item' for drop down
  getAramarkItems() {
    this.fabricData.forEach(c => {
      this.aramarkItemData.push(new AramarkItemDetails(c.Id, c.ItemCode, c.ItemName, c.Width, c.FiberContent));
    });
    this.aramarkList = this.aramarkItemData;
  }

  //Get data for 'Vendor' drop down
  getVendorDetails() {
    let vendors = this._localStorage.get("vendor_data");
    if (vendors) {
      if(typeof vendors === "string"){
        this.vendorData = JSON.parse(vendors);
      }
      //this.getReasonDetails();
    } else {
      this._physicalAdjustmentsService.getVendorDetails().subscribe(
        data => {
          let vendors: Array<VendorDetails> = [];
          data.forEach(element => {
            element.VendorDetail = element.Code + " - " + element.Name;
            vendors.push(element);
          });
          this._localStorage.add("vendor_data", JSON.stringify(vendors));
          //this.getReasonDetails();
        },
        (err: HttpErrorResponse) => {
          this._errorService.error(err)
        }
      );
    }
  }

  //Assign proper data to Vendor related fileds on change of Vendor
  onVendorChange(selectedVendor: VendorDetails, rollCase: RollCasesModel) {
    if(selectedVendor!==undefined)
    {
      rollCase.VendorItem = selectedVendor.VendorItemNumber;
      rollCase.CountryOfOrigin = selectedVendor.CountryOfOrigin;
      rollCase.Vendor = selectedVendor;}
  }

  //Assign proper data to few Columns on change of Vendor
  onAramarkItemCodeChange(selectedAramarkItem: AramarkItemDetails, rollCase: RollCasesModel) {
    rollCase.Vendor = null;
    if(selectedAramarkItem!==undefined)
    {
      rollCase.FiberContent = selectedAramarkItem.FiberContent;
      rollCase.ColorCode = this.getColorByItemId(selectedAramarkItem.Id);
      rollCase.AramarkItemCode = this.aramarkList.find(e => e.ItemCode == selectedAramarkItem.ItemCode);
      rollCase.Width = selectedAramarkItem.Width;
      rollCase.VendorList = this.getVendorsByItemId(selectedAramarkItem.Id);
      this.vendorData = rollCase.VendorList;
    }
    if (rollCase.VendorList.length === 1) {
      rollCase.Vendor = rollCase.VendorList[0];
      this.onVendorChange(rollCase.Vendor, rollCase);
    }
  }

  // Get the Color Based on 'Aramark Item Id'
  getColorByItemId(itemId: number): ColorDetails {
    let fabriItems = this._localStorage.get("fabric_items");
    if (fabriItems) {
      if(typeof fabriItems === "string"){
      fabriItems = JSON.parse(fabriItems);
      }
      let i = fabriItems.filter(c => c.Id == itemId)[0];
      return new ColorDetails(i.ColorId, i.Color, i.ColorCode, i.ColorCode + " - " + i.Color);
    }
  }

  // Get the Vendors Based on 'Aramark Item Id'
  getVendorsByItemId(aramarkItemId: number): Array<VendorDetails> {
    let vendorData = this._localStorage.get("vendor_data");
    if (vendorData) {
      if(typeof vendorData === "string"){
        vendorData = JSON.parse(vendorData);
      }
      return vendorData.filter(c => c.ItemId === aramarkItemId);
    }
  }

  // Get the Reason Details for 'Reason Code' drop down
  getReasonDetails(reasontype:number) {
    let reasons;
    if(reasontype === 3){
      reasons = this._localStorage.get("reason_data_add");
    }
    else{
      reasons = this._localStorage.get("reason_data_delupd");
    }
    if (reasons) {
      if(typeof reasons === "string"){
        if(reasontype === 3){
          this.reasonDataAdd = JSON.parse(reasons);    
        }
        else{
          this.reasonDataDelupd = JSON.parse(reasons);
        }
      }
    }
    else {
      this._physicalAdjustmentsService.getResonDetails(reasontype).subscribe(
        data => {
          
          if(reasontype===3){
            this.reasonDataAdd = data;
            this._localStorage.add("reason_data_add", JSON.stringify(data))
          }
          else{
            this.reasonDataDelupd = data;
            this._localStorage.add("reason_data_delupd", JSON.stringify(data))
          }
        },
        (err: HttpErrorResponse) => {
          this._errorService.error(err);
        }
      );
    }
  }

  OnReasonChange(selectedreason: ReasonDetails, rollCase: RollCasesModel) {
    rollCase.ReasonCodeId = selectedreason;
    if (rollCase.IsAdded !== true) {
      rollCase.IsModified = true;
    }
  }

  // Add new Row to grid
  onAdd() {
    this.getColors();
    this.getReasonDetails(3);
    let roll = this._physicalAdjustmentsService.onAdd();
    roll.AramarkItemList = this.aramarkItemData;
    this.handleColorFilter("",roll);
    this.rollCases.push(roll);
    this.gridView.data = this.rollCases;
  }

  saveConfirm() {
    let selectedRollCases = this.rollCases.filter(r => r.IsSelected === true);
    if (selectedRollCases.length > 0) {
      // validate the roll cases before saving
      selectedRollCases.forEach(c => this.validateRollCase(c));
      if (selectedRollCases.some(c => c.IsValid === false)) {
        return;
      }
      this._confirmationService.confirm(
        {
          key: 'message',
          value: { 
            message: 'Do you want to add/update roll(s) to Inventory?', 
            continueCallBackFunction: () => this.onSave() }
        });
    }
  }

  transferConfirm() {
    if (this.selectedDestinationBinLocation != undefined) {
      this._confirmationService.confirm(
        {
          key: 'message',
          value: {
            message: 'Do you want to Transfer roll(s) from ' + this.selectedBinLocation.AreaCode +
            ' to new bin location' + this.selectedDestinationBinLocation.AreaCode +
            ' ?', 
            continueCallBackFunction: () => this.transferToBin()
          }
        });
    }
  }

  // Confirmation For Deleting
  deleteConfirm() {
    let selectedRows = this.rollCases.filter(r => r.IsSelected === true);
    if (selectedRows.some(r => r.Allocated > 0)) {
      //this.callAlert('Allocated roll(s) cannot be deleted');
      this._toastService.warn('Allocated roll(s) cannot be deleted');
      return false;
    }
    let newRows = this.rollCases.filter(r => r.IsAdded === true && r.IsSelected === true);
    if (newRows.length > 0) {
      newRows.forEach(element => {
        let a = this.rollCases.indexOf(element);
        if (a != -1) {
          this.rollCases.splice(a, 1);
        }
      });
      return;
    }
    if (selectedRows.length > 0) {
      if (selectedRows.some(r => r.ReasonCodeId === undefined || r.ReasonCodeId === 0)) {
        //this.callAlert('Please select reason code to delete the roll case');
        this._toastService.warn("Please select reason code to delete the roll case");
        return;
      }
      this._confirmationService.confirm(
        {
          key: 'message',
          value: { 
            message: 'Do you want to delete roll(s) from inventory?'+'<br><br>'+'Note: Rolls added in the previous fiscal month cannot be removed from inventory,  their quantity information will be updated to Zero', 
            continueCallBackFunction: () => this.onDelete() 
          }
        });
    }
  }

  // Delete the Row/Rows from the grid
  onDelete() {
    let selectedRows = this.rollCases.filter(r => r.IsSelected === true);
    if (selectedRows.length > 0) {
      selectedRows = this.rollCases.filter(r => r.IsSelected === true && r.IsAdded === false);
      selectedRows.forEach(c => {
        c.ReasonCodeId = c.ReasonCodeId !== undefined ? c.ReasonCodeId.Id : null;
      });
      this._physicalAdjustmentsService.onDelete(selectedRows).subscribe(
        data => {
          let numOfRowsDeleted = this.rollCases.filter(r => r.IsSelected === true);
          this.rollCases = this.rollCases.filter(r => r.IsSelected !== true);
          this.gridView.data = this.rollCases;
          // this._successService.success({
          //   key: 'successMessage',
          //   value: numOfRowsDeleted.length + ' RollCase(s) deleted succesfully.'
          // });

          this._toastService.success( numOfRowsDeleted.length + ' RollCase(s) deleted succesfully.' );
        },
        (err: HttpErrorResponse) => {
          const rollCasesToDelete = this.rollCases.filter(r => r.IsSelected === true);
          let rollcaseIds: Array<any> = [];
          let errDesc = err.error;
          let failedIds: Array<any> = [];
          rollCasesToDelete.forEach(element => {
            rollcaseIds.push(Number(element.RollCaseId));
          });
          if(typeof errDesc === "string"){
            errDesc = JSON.parse(errDesc);
          }
          if (errDesc && errDesc.ErrorData !== undefined && errDesc.ErrorData !== null) {
             errDesc.ErrorData.split(',').forEach(element => 
              {
                failedIds.push(element);
              });
            rollcaseIds.forEach(element =>{
              if(failedIds.indexOf(element) < 0){
                let deletedRollCase = rollCasesToDelete.find(r => r.RollCaseId == element)
                this.rollCases.splice(this.rollCases.indexOf(deletedRollCase),1)
                this.gridView.data = this.rollCases;
              }
            })
          }
          this._errorService.error(err);
        }
      );
    }
  }

  // Copy the Selected row
  onCopy() {
    if (this.rollCases.filter(c => c.IsSelected === true).length > 0) {
      if (this.rollCases.filter(c => c.IsSelected === true).length > 1) {
        //this.callAlert('Please select only one roll to copy')
        this._toastService.warn('Please select only one roll to copy');
        return;
      }
      else {
        let selectedRow = this.rollCases.find(c => c.IsSelected === true);
        selectedRow.IsSelected = false;
        let selectedRowIndex = this.rollCases.indexOf(selectedRow);
        let newRow = new RollCasesModel();
        newRow.Id = 0;
        newRow.IsAdded = true;
        newRow.ColorCode = selectedRow.ColorCode;
        newRow.AramarkItemCode = selectedRow.AramarkItemCode;
        newRow.FiberContent = selectedRow.FiberContent;
        newRow.Vendor = selectedRow.Vendor;
        newRow.VendorId = selectedRow.VendorId;
        newRow.VendorItem = selectedRow.VendorItem;
        newRow.CountryOfOrigin = selectedRow.CountryOfOrigin;
        newRow.LotNumber = selectedRow.LotNumber;
        newRow.ColorDetailsList = this.colorData;
        newRow.AramarkItemList = this.aramarkItemData;
        newRow.VendorList = selectedRow.VendorList;
        newRow.OnHand = '0';
        newRow.Allocated = 0;
        newRow.Available = 0;
        newRow.Defective = '0';
        newRow.CountryOfOrigin = selectedRow.CountryOfOrigin;
        newRow.Width = selectedRow.Width;
        newRow.IsValid = true;
        newRow.IsSelected = true;
        this.rollCases.push(newRow);
        this.gridView.data = this.rollCases;
      }
    }
  }

  // make All rows as 'selected' when main CheckBox is 'Checked'
  onSelectAll(event) {
    this.gridView.data.forEach(c => c.IsSelected = event.target.checked);
  }

  isAllSelected(){
    return this.gridView.data.length > 0 && this.gridView.data.every(c => c.IsSelected === true);
  }

  // Get Total 'On Hand Yards' in the grid
  getTotalOnHandYard() {
    let totalOnHandYards = 0;
    this.rollCases.forEach(element => {
    let oh = element.OnHand.toString().replace(/,/g , "");
      totalOnHandYards += Number(oh);
    });
    return totalOnHandYards;
  }

  // Get Total 'Available Yards' in the grid
  getTotalAvailableYards() {
    let totalAvaliablYards = 0;
    this.rollCases.forEach(element => {
    let available = element.Available.toString().replace(/,/g , "");
      totalAvaliablYards += Number(available);
    });
    return totalAvaliablYards;
  }

  // Get Total 'Allocated Yards' in the grid
  getTotalAllocatedYards() {
    let totalAllocatedYards = 0;
    this.rollCases.forEach(element => {
    let allocated = element.Allocated.toString().replace(/,/g , "");
      totalAllocatedYards += Number(allocated);
    });
    return totalAllocatedYards;
  }

  availableYards(rollCase: RollCasesModel) {
    return Number(rollCase.OnHand) - Number(rollCase.Defective)
  }

  //Change 'Available' filed on change of 'OnHand'
  onOHChange(row: RollCasesModel) {
    let oh = row.OnHand.toString().replace(/,/g , "");
    let defectve = row.Defective.toString().replace(/,/g , "");
    let allocate = row.Allocated.toString().replace(/,/g , "");
    row.Available = Number(oh) - Number(defectve) - Number(allocate);
    if (row.IsAdded !== true) {
      row.IsModified = true;
    }
  }

  onDefectiveChange(row: RollCasesModel) {
    if (Number(row.Defective) > Number(row.OnHand)) {
      //this.callAlert('Value entered for Defective can not be greater than On Hand');
      this._toastService.warn('Value entered for Defective can not be greater than On Hand');
      return false;
    }
    row.Available = Number(row.OnHand) - Number(row.Defective) - row.Allocated;
    if (row.IsAdded !== true) {
      row.IsModified = true;
    }
  }

  // Get 'Bin' data to transfer
  onTransfer() {
    if (this.rollCases.some(c => c.IsAdded === true || c.IsModified === true)) {
      // this._alertService.alert({
      //   key: 'alertMessage',
      //   value: 'Please save the modified data before transfer the roll(s)'
      // });
      this._toastService.warn('Please save the modified data before transfer the roll(s)')
      return;
    }
    if (this.rollCases.filter(r => r.IsSelected === true).length > 0) {
      this.opened = true;
      this.DestinationBinFullMessage = '';
      this.selectedDestinationBinLocation = null;
      this.destinationBinLocations = this.binLocations.filter(b => b.Id !== this.selectedBinLocation.Id);
      this.destinationBinLocationList = this.destinationBinLocations;
    }
  }

  // Close the transfer popup
  close() {
    this.opened = false;
  }

  // Save the Grid Data
  onSave() {
  let rollCases = this.rollCases.filter(r => r.IsSelected === true);
      if (rollCases.length > 0) {
        let inventory: InventoryControl = new InventoryControl();
        inventory.LocationId = this.selectedLocation.Id;
        inventory.SourceBinId = this.selectedBinLocation.Id;
        inventory.RollCases = new Array<RollCasesModel>();
        rollCases.forEach(c => {
          let rollCase = new RollCasesModel();
          if (c.Id > 0) {
            rollCase.IsModified = true;
          }
          else if (c.Id == 0 || c.Id == undefined) {
            rollCase.IsAdded = true;
          }
          rollCase.RollCaseId = c.RollCaseId;
          rollCase.OnHand = c.OnHand;
          rollCase.ItemId = c.AramarkItemCode === null ? null : c.AramarkItemCode.Id;
          rollCase.LotNumber = c.LotNumber;
          rollCase.VendorId = c.Vendor === null ? null : c.Vendor.Id;
          rollCase.ReasonCodeId = c.ReasonCodeId.Id;
          rollCase.Id = c.Id;
          rollCase.Defective = c.Defective;
          inventory.RollCases.push(rollCase);
        });
        let IsSaved = this._physicalAdjustmentsService.saveRollCaseGridData(inventory).subscribe(
          data => {
            let addValMsg: string;
            let modValMsg: string;
            let addedRows = this.rollCases.filter(r => r.IsSelected && r.IsAdded);
            let modifiedRows = this.rollCases.filter(r => r.IsSelected && r.IsModified);
            if (addedRows.length > 0) {
              addValMsg = addedRows.length + ' : Roll(s) Added in Inventory Master'
            }
            if (modifiedRows.length > 0) {
              modValMsg = modifiedRows.length + ' : Roll(s) Modified in Inventory Master'
            }
            let val: string;
            if (modifiedRows.length === 0) {
              val = addValMsg;
            }
            if (addedRows.length === 0) {
              val = modValMsg;
            }
            if (modifiedRows.length > 0 && addedRows.length > 0) {
              val = addValMsg + '<br>' + modValMsg;
            }
            this.onRefresh();
            // this._successService.success({
            //   key: 'successMessage',
            //   value: val
            // });
            this._toastService.success(val);
          },
          (err: HttpErrorResponse) => {
            let rollCasesToSave = this.rollCases.filter(r => r.IsSelected === true);
            let selectedRollcaseIds: Array<any> = [];
            let failedRollCaseIds: Array<any> = [];
            rollCasesToSave.forEach(element => {
              selectedRollcaseIds.push(element.RollCaseId);
            });
            let errDesc = err.error;
            if(typeof err.error === "string"){            
            errDesc = JSON.parse(err.error);
            }
            if (errDesc && errDesc.ErrorData !== undefined && errDesc.ErrorData !== null ) {
              errDesc.ErrorData.split(',').forEach(element => {
                failedRollCaseIds.push(element);
              });
              selectedRollcaseIds.forEach(sId => {
                if (failedRollCaseIds.indexOf(sId) < 0) {
                  this.rollCases.forEach(r => {
                    if (r.RollCaseId == sId) {
                      r.IsAdded = false;
                      r.IsModified = false;
                      r.IsSelected = false;
                    }
                  });
                }
              });
            }
            this._errorService.error(err);
          });
      }
  }

  // Transfer the Roll case(s) to destination Bin
  transferToBin() {
    const transferRollCases = this.rollCases.filter(r => r.IsSelected === true);
    const inventory: InventoryControl = new InventoryControl();
    inventory.LocationId = this.selectedLocation.Id;
    inventory.SourceBinId = this.selectedBinLocation.Id;
    inventory.DestinationBinId = this.selectedDestinationBinLocation.Id;
    inventory.RollCases = new Array<RollCasesModel>();
    transferRollCases.forEach(c => {
      const rollCase = new RollCasesModel();
      rollCase.RollCaseId = c.RollCaseId;
      rollCase.OnHand = c.OnHand;
      rollCase.ItemId = c.AramarkItemCode.Id;
      rollCase.LotNumber = c.LotNumber;
      rollCase.VendorId = c.Vendor.VendorItemId;
      rollCase.Id = c.Id;
      inventory.RollCases.push(rollCase);
    });
    this._physicalAdjustmentsService.transferRollCases(inventory).subscribe(
      data => {
        transferRollCases.forEach(element => {
          const index = this.rollCases.indexOf(element);
          if (index >= 0) {
            this.rollCases.splice(index, 1);
            this.gridView.data = this.rollCases;
          }
        });
        this._toastService.success((transferRollCases.length + ' Roll(s) transfered from ' +
            this.selectedBinLocation.AreaCode + ' to new bin location ' + this.selectedDestinationBinLocation.AreaCode));
      },
      (err: HttpErrorResponse) => {
        this._errorService.error(err);
      }
    );
  }

  /*
    this method helps to enable or disable
    the save, copy, delete, transfer functionalities
  */
  enableActions() {
    return this.rollCases.some(c => c.IsSelected === true);
  }

  ExportToPdf(grid: GridComponent) {
    grid.saveAsPDF();
  }

  ExportToExcel(grid: GridComponent) {
    grid.saveAsExcel();
  }

  validateRollCase(rollCase: RollCasesModel) {
    let errorInfo: Array<string> = [];
    let rollError: string;
    if (!rollCase.RollCaseId) {
      errorInfo.push("Roll Case #");
    }
    if (!rollCase.ColorCode) {
      errorInfo.push("Color");
    }
    if (!rollCase.AramarkItemCode) {
      errorInfo.push("Aramark Item");
    }
    if (!rollCase.Vendor) {
      errorInfo.push("Vendor");
    }
    if (!rollCase.LotNumber) {
      errorInfo.push("Lot #");
    }
    if (Number(rollCase.OnHand) === 0) {
      errorInfo.push("On Hand");
    }
    if (!String(rollCase.Defective)) {
      errorInfo.push("Defective");
    } 
    if (!rollCase.ReasonCodeId) {
      errorInfo.push("Reason Code");
    }   
    rollError = errorInfo.join(", ");
    if (rollError) {
      this._toastService.warn("Kindly enter the required field(s):  " + rollError);
      rollCase.IsValid = false;
    }
    else {
      rollCase.IsValid = true;
    }
  }

  modifyRow(rollCase: RollCasesModel) {
    if (rollCase.IsAdded !== true) {
      rollCase.IsModified = true;
    }
  }

  onDestinationBinChange(selectedDestinationBin: BinLocationsModel) {
    if(selectedDestinationBin!==undefined)
    {
      this.selectedDestinationBinLocation = selectedDestinationBin;
      this._physicalAdjustmentsService.getRollCases(selectedDestinationBin.Id).subscribe(
        data => {
          if (data.length > 0) {
            this.DestinationBinFullMessage = 'Destination contains one or more rolls'
          }
          else {
            this.DestinationBinFullMessage = '';
          }
        }
      );
    } 
  }
  sortRollCases(rollCaseArray: Array<RollCasesModel>) {
    rollCaseArray = rollCaseArray.sort((r1, r2) => {
      if (Number(r1.RollCaseId) > Number(r2.RollCaseId)) {
        return 1;
      }
      else if (Number(r1.RollCaseId) < Number(r2.RollCaseId)) {
        return -1;
      }
      return 0;
    });
    return rollCaseArray;
  }
  enableTransfer() {
    if (this.selectedDestinationBinLocation !== null) {
    if(this.selectedDestinationBinLocation !==undefined)
    {
      if (this.selectedDestinationBinLocation.Id === null) {
        return false;
      }
      else
        return true;
    }
    }
  }

  sortChange(sort: SortDescriptor[]): void {
    this.sort = sort;
    this.loadRollCases();
  }
  
  loadRollCases() {
    this.gridView = {
      data: orderBy(this.rollCases, this.sort),
      total: this.rollCases.length
    };
  }

  onSelect(item:DropDownModel){

  }
  
  handleColorFilter(value, row:RollCasesModel) {
    if(value){
      row.ColorDetailsList = this.colorData.filter(s => {
        if(s.Color){
          return s.Color.toLocaleUpperCase().startsWith(value.toLocaleUpperCase());
        }
        else{
          return;
        }
      });
    }
    else{
      row.ColorDetailsList = this.colorData;
    }
  }

  handleAramarkItemFilter(value:string, row: RollCasesModel) {
    if(value)
    {
      row.AramarkItemList = this.aramarkItemData.filter((s) => (s.ItemCode.toLocaleUpperCase()).startsWith(value.toLocaleUpperCase()));
    }
    else
    {
      row.AramarkItemList =this.aramarkItemData;
    }
  }
  
  handleVendorFilter(value:string, row: RollCasesModel) {
    let vendors :Array<VendorDetails>=[];
    for(let v of this.vendorData)
    {
      vendors.push(v);
    }    
    if(value === undefined || value === "")
    {
      row.VendorList =vendors;
    }
    else
    {
      row.VendorList = vendors.filter((s) => (s.Code).startsWith(value));
    }
  }  
}

import { Injectable } from "@angular/core";
import { MaintainRolls } from "./models/maintain-rolls";
import { LocationsModel } from "./models/locations.model";
import { HttpService } from "../../../shared/wrappers/http.service";
import { BinLocationsModel } from "./models/bin-locations.model";
import { RollCasesModel } from "./models/roll-case.model";
import { FarbricDetailsModel } from "./models/fabric-details.model";
import { VendorDetails } from "./models/vendor-details.model";
import { ReasonDetails } from "./models/reason-details.model";
@Injectable()
export class PhysicalAdjustmentsService {
  constructor(private _httpService: HttpService) {}

  maintainRollsGrid: Array<MaintainRolls>;
  rollcases:Array<RollCasesModel>;
  public allocated: number = 0;
  public available: number = 0;
  public onhand: number = 0;
  public totalRoll;
  public index;
  public IsNew: true;
  
  onAdd(): any {
    let roll = new RollCasesModel();
    roll.IsAdded = true;
    roll.IsSelected = true;
    roll.Id = 0;
    roll.OnHand = '0';
    roll.Allocated = 0;
    roll.Defective = '0';
    roll.Available = 0;
    roll.IsValid = true;
    return roll;
  }

  onDelete(rowsToDelete : RollCasesModel[]){
    return this._httpService.post('inventory/deleterolls', rowsToDelete)
    .map(response =>{
      return response
    })
  }

  onCopy(): any {
    if (this.maintainRollsGrid.filter(c => c.IsSelected === true).length > 1) {
      alert("Not able to copy more than one row");
      return;
    } else {
      this.maintainRollsGrid.filter(c => c.IsSelected === true).forEach(x => {
        this.index = this.maintainRollsGrid.lastIndexOf(x);
      });
      if (this.maintainRollsGrid.length - 1 === this.index) {
        var selectedItem = this.maintainRollsGrid.filter(
          c => c.IsSelected === true
        );
        var a = selectedItem[0];
        var copiedRow = new MaintainRolls(
          false,
          a.RollCaseId,
          a.AramarkItemCode,
          a.Color,
          a.FiberContent,
          a.VendorItem,
          a.Vendor,
          a.Lot,
          a.COO,
          a.RollWidth,
          a.OnHand,
          a.Allocated,
          a.Available,
          0,
          a.ReasonCode
        );

        return this.maintainRollsGrid.push(copiedRow);
      } else {
        var selectedItem = this.maintainRollsGrid.filter(
          c => c.IsSelected === true
        );
        let i = this.index;
        var a = selectedItem[0];
        var copiedRow = new MaintainRolls(
          a.IsSelected,
          a.RollCaseId,
          a.AramarkItemCode,
          a.Color,
          a.FiberContent,
          a.VendorItem,
          a.Vendor,
          a.Lot,
          a.COO,
          a.RollWidth,
          a.OnHand,
          a.Allocated,
          a.Available,
          0,
          a.ReasonCode
        );

        return this.maintainRollsGrid.splice(i + 1, 1, copiedRow);
      }
    }
  }

  getLocation() {
    let locations = this._httpService
      .get<LocationsModel[]>("inventory/locations")
      .map(response => {
        return response;
      });
    return locations;
  }

  getBinLocation(locationId: number) {
    return this._httpService
      .get<BinLocationsModel[]>("inventory/binlocations/" + locationId)
      .map(response => {
        return response;
      });
  }

  getRollCases(binLocationId: number) {
    return this._httpService
      .get<RollCasesModel[]>("inventory/rollcases/" + binLocationId)
      .map(response => {
        return response;
      });
  }

  getFabricdetails() {
    return this._httpService
    .get<FarbricDetailsModel[]>("inventory/fabricdetails")
    .map(response => {
      return response;
    });
  }

  getVendorDetails(){
    return this._httpService
    .get<VendorDetails[]>("inventory/vendors")
    .map(response => {
      return response;
    });
  }

  getResonDetails(reasonTypeID: number){
    return this._httpService
    .get<ReasonDetails[]>("inventory/reasoncodes/" + reasonTypeID)
    .map(response => {
      return response;
    });
  }

  saveRollCaseGridData(rollcases : any){
   return this._httpService.post('inventory/saverolls', rollcases)
    .map(response =>{
      return response;
    });
  }

  transferRollCases(rollcases : any){
   return this._httpService.post('inventory/transferrolls', rollcases)
    .map(response =>{
      return response;
    });
  }

}

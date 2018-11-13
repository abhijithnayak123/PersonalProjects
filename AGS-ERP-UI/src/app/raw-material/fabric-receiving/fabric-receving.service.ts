import { Injectable } from "@angular/core";
import * as moment from 'moment';
import { Roll } from "../receiving/models/roll.model";
import { Receipt } from "../receiving/models/receipt.model";
import { VendorModel } from "../receiving/models/vendor.model";

@Injectable()
export class FabricRecevingService {
  gridData: Array<Roll>;
  public totalRoll;
  public index;
  public date;
    
  constructor() {}
  //Adding the row
  onAdd(): any {
    return new Roll(null,null,null,null,null,null,null,null,null,null,null,false,false);
  }

  //Copying the row
  onCopy(): any {
    if (this.gridData.filter(c => c.IsSelected === true).length > 1) {
      alert("Not able to copy more than one row");
      return;
    } else {
      this.gridData.filter(c => c.IsSelected === true).forEach(x => {
        this.index = this.gridData.lastIndexOf(x);
      });
      if (this.gridData.length - 1 === this.index) {
        var selectedItem = this.gridData.filter(c => c.IsSelected === true);
        var a = selectedItem[0];
        var copiedRow = new Roll(null,a.PORelease,a.PO,null,a.AramarkItemCode,
                            a.VendorItemCode,a.Color,a.Yards,a.Width,a.Lot,
                            a.FiberContent,a.IsUsable,false);

        return this.gridData.push(copiedRow);
      } else {
        var selectedItem = this.gridData.filter(c => c.IsSelected === true);
        let i = this.index;
        var a = selectedItem[0];
        var copiedRow = new Roll(a.RollNumber,a.PORelease,a.PO,a.BinLocation,
                                  a.AramarkItemCode,a.VendorItemCode,a.Color,a.Yards,
                                  a.Width,a.Lot,a.FiberContent,a.IsUsable,false);
        return this.gridData.splice(i + 1, 1, copiedRow);
      }
    }
  }

  // delete the row
  onDelete(): any {
    if (this.gridData.filter(c => c.IsSelected === true).length === this.gridData.length) {
      alert("Cannot delete all the rows");
      return this.gridData;
    } else {
      this.gridData.filter(c => c.IsSelected === true).forEach(x => {
        alert("Your going to delete a row/rows")
        //make a call to api.
        const index: number = this.gridData.indexOf(x);
        if (index !== -1) {
          this.gridData.splice(index, 1);
        }
      });
      return this.gridData;
    }
  }

  onCheckAll(event) {
    return this.gridData.forEach(c => (c.IsSelected = event.target.checked));
  }
  //Get Grid Data
  getGriddata(): any {
    return (this.gridData = [
      new Roll(16907,16907,16907,"101-001-001","FW326FL0240650","DANISH NAVY","999",
                100,12,"10","65/35 POLYESTOR/ COTTON",true,false),
      new Roll(16908,17042,16908,"102-001-001","FW335AA0100610","SWRW TAN 70775","999",
                101, 13,"11","65/35 POLYESTOR/ COTTON",true,false),
      new Roll(16909,17042,16909,"102-001-001","FW335AA0100610","SWRW TAN 70775","999",
                101,13,"11","65/35 POLYESTOR/ COTTON",true,false),
      new Roll(16910,17042,16910,"102-001-001","FW335AA0100610","SWRW TAN 70775","999",
                101,13,"11","65/35 POLYESTOR/ COTTON",true,false),
      new Roll(16909,17042,16911,"102-001-001","FW335AA0100610","SWRW TAN 70775","999",
                101,13,"11","65/35 POLYESTOR/ COTTON",true,false)
    ]);
  }

  //Get Receipt Id
  getReceiptIds(): Array<Receipt> {
    return [];
  }

  //Get Vendo Id
  getVendorIds(): Array<VendorModel> {
    return VENDOR;
  }
  //Get Total Rolls
  getTotalRolls(): any {
    return (this.totalRoll = this.gridData.length);
  }

  getdate()
  {
    this.date = moment(new Date()).format("MM/DD/YYYY");
    return this.date
  }


}

export const VENDOR: VendorModel[] = [
  { Id: 1, Description: "S - 6 3/4" },
  { Id: 2, Description: "M - 7 1/4" },
  { Id: 3, Description: "L - 7 1/8" },
  { Id: 4, Description: "XL - 7 5/8" }
];

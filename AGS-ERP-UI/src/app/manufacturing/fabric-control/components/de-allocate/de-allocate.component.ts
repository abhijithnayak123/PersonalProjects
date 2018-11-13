import { Component, OnInit } from '@angular/core';
import { AllocateModel } from "../../models/allocate.model";
import { FabricControlService } from "../../fabric-control.service";
import { FabricControlModel } from '../../models/fabricControl.model';
import { CutOrder } from '../../models/cut-order.model';
import { SuccessService } from '../../../../shared/services/success.service';
import { ConfirmationService } from '../../../../shared/services/confirmation.service';
import { ToastService } from '../../../../shared/services/toast.service';

@Component({
  selector: 'app-de-allocate',
  templateUrl: './de-allocate.component.html',
  styleUrls: ['./de-allocate.component.css']
})
export class DeAllocateComponent implements OnInit {
  fabricControlData: FabricControlModel;
  cutOrderList: Array<CutOrder>;
  constructor(
    private _fabricControlService: FabricControlService,
    private _successService: SuccessService,
    private _confirmationService: ConfirmationService,
    private _toastService: ToastService
  ) { }

  ngOnInit() {
    this.getCutOrders();
    this.fabricControlData = new FabricControlModel(null, null, null, null, null, null, null, null, null, null, null, null, null, null, [],null);

  }

  getCutOrders() {
    this.cutOrderList = this._fabricControlService.getCutOrdersForDeAllocate();
  }

  onCutOrderChange(cutOrder: string) {
    this._fabricControlService.getDeAllocateHeaderData(cutOrder).subscribe(data => {
      this.fabricControlData = data;
      this.fabricControlData.CutDate = this._fabricControlService.getDate(this.fabricControlData.CutDate);
      this.fabricControlData.StyleDescription = "Twill work Pant";
      this.fabricControlData.FiscalWeekNumber = this._fabricControlService.getDate(this.fabricControlData.CutDate);
      this.fabricControlData.ActualCutQty = 1500;
      this.fabricControlData.FabricRollCases.forEach(a=>{
        a.AllocatedDate = this._fabricControlService.getDate(this.fabricControlData.CutDate);
        a.AllocatedBy = "Aramark";
        a.VendorItem = "S/3078-5952";        
      })
    });
  }
  onCheckAll(event) {
    this.fabricControlData.FabricRollCases.forEach(c => c.IsSelected = event.target.checked);
  }
  canEnableDeAllocate() {
    return this.fabricControlData.FabricRollCases.some(a => a.IsSelected === true);
  }

  getTotalDeAllocatedYards() {
    let totalSelectedYards = 0;
    this.fabricControlData.FabricRollCases.filter(c => c.IsSelected === true).forEach(c => {
      totalSelectedYards += c.Allocated
    });
    return totalSelectedYards;
  }

  confirmDeAllocate() {
    if (this.fabricControlData.FabricRollCases.some(c => c.IsSelected === true)) {
      this._confirmationService.confirm({
        key: 'message',
        value: { 
          message: 'Please confirm to de-allocate ' + this.fabricControlData.FabricRollCases.filter(d => d.IsSelected === true).length + ' selected rolls.', 
          continueCallBackFunction: () => this.onDeAllocate() }
      });
    }
  }

  onDeAllocate() {
    let status = this._fabricControlService.onDeAllocate(this.fabricControlData);
    if (status) {
      // this._successService.success({
      //   key: 'successMessage',
      //   value: "Selected rolls are de-allocated successfully."
      // });
      this._toastService.success('Selected rolls are de-allocated successfully.');
    }
  }
}

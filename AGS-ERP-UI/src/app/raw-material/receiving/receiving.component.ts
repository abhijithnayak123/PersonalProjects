import { Component, OnInit, ViewChild } from '@angular/core';
import { TabStripComponent } from '@progress/kendo-angular-layout/dist/es/tabstrip/tabstrip.component';
import { Subscription } from 'rxjs/Rx';
import { CommonService } from '../../shared/services/common.service';
@Component({
  selector: 'app-receiving',
  templateUrl: './receiving.component.html',
  styleUrls: ['./receiving.component.css']
})
export class ReceivingComponent implements OnInit {
  @ViewChild('tabstrip') public tabstrip: TabStripComponent;
  subscription: Subscription;
  receiptNumber: string;
  poNumber: string;
  disableTab: boolean;
  constructor(
    private _commonService: CommonService
  ) { }

  ngOnInit() {
    this.receiptNumber = '';
    this.poNumber = '';
    this.disableTab = true;
    this.subscription = this._commonService.notifyObservable.subscribe(
      res => {
        if (res.hasOwnProperty('key') && res.key === 'PlanningReport') {
          this.receiptNumber = res.value.ReceiptNumber;
          this.poNumber = res.value.PONumber;
          this.disableTab = false;
          Promise.resolve(null).then(() => this.tabstrip.selectTab(2));
        }
        if (res.hasOwnProperty('key') && res.key === 'destroyLookUpDetail') {
          this.receiptNumber = res.value.ReceiptNumber;
          this.disableTab = true;
          this.poNumber = '';
        }
      });
  }

}

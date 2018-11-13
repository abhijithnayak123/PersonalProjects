import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonService } from '../../shared/services/common.service';
import { TabStripComponent } from '@progress/kendo-angular-layout/dist/es/tabstrip/tabstrip.component';
import { Subscription } from "rxjs/Rx";
import { SessionStorageService } from "../../shared/wrappers/session-storage.service";

@Component({
  selector: 'app-master',
  templateUrl: './master.component.html',
  styleUrls: ['./master.component.css']
})
export class MasterComponent implements OnInit {
  @ViewChild('tabstrip') public tabstrip: TabStripComponent;
  itemCode : string='';
  subscription: Subscription;
   constructor(
    private _commonService: CommonService,
    private _sessionStorage: SessionStorageService
  ) {}

  ngOnInit() {
    this.itemCode = ''
    this.subscription = this._commonService.notifyObservable.subscribe(
      res => {
        if (res.hasOwnProperty('key') && res.key === 'RMItem') {
          console.log('VALUE-->'+res.value.ItemCode);
          this.itemCode = res.value.ItemCode;
          Promise.resolve(null).then(() => this.tabstrip.selectTab(1));
        }
        if (res.hasOwnProperty('key') && res.key === 'destroyItem') {
          console.log('VALUE'+res.value.ItemCode);
          this.itemCode = res.value.ItemCode;
        }
      });
  }

}

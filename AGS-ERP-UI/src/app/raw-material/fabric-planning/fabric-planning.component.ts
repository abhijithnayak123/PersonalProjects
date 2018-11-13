import { Component, OnInit, ViewChild ,OnDestroy} from '@angular/core';
import { CommonService } from '../../shared/services/common.service';
import { Subscription } from 'rxjs/Rx';
import { TabStripComponent } from '@progress/kendo-angular-layout/dist/es/tabstrip/tabstrip.component';
import { SessionStorageService } from "../../shared/wrappers/session-storage.service";

@Component({
  selector: 'app-fabric-planning',
  templateUrl: './fabric-planning.component.html',
  styleUrls: ['./fabric-planning.component.css']
})
export class FabricPlanningComponent implements OnInit,OnDestroy {
  @ViewChild('tabstrip') public tabstrip: TabStripComponent;
  vendorId: number;
  itemId: number;
  subscription: Subscription;
   constructor(
    private _commonService: CommonService,
    private _sessionStorage: SessionStorageService
  ) {}

  ngOnInit() {
    this.vendorId = this.itemId = 0;
    this.subscription = this._commonService.notifyObservable.subscribe(
      res => {
        if (res.hasOwnProperty('key') && res.key === 'PlanningReport') {
          this.vendorId = res.value.vendorId;
          this.itemId = res.value.itemId;
          Promise.resolve(null).then(() => this.tabstrip.selectTab(2));
        }
        if (res.hasOwnProperty('key') && res.key === 'destroyReport') {
          this.vendorId = res.value.vendorId;
          this.itemId = res.value.itemId;
        }
      });
  }

  ngOnDestroy(){
    this._sessionStorage.removeAll(['fabric_watchlist_load', 'fabric_watchlist_vendors_list',
        'fabric_watchlist_vendors', 'fabric_watchlist_selected_vendor', 'fabric_watchlist_selected_item', 'fbWlGridData',
        'fabric_watchlist_itemList','fabric_watchlist_filtered_items','future_watchlist_load', 'future_watchlist_vendors',
        'future_watchlist_selected_vendor', 'fuWlGridData','future_watchlist_itemList', 'future_watchlist_selected_item','future_watchlist_vendor_selected'
    ]);
  }

}

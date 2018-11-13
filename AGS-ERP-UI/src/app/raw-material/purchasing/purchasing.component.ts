import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonService } from '../../shared/services/common.service';
import { Subscription } from 'rxjs';
import { TabStripComponent } from '@progress/kendo-angular-layout/dist/es/tabstrip/tabstrip.component';
import { OnDestroy } from '@angular/core/src/metadata/lifecycle_hooks';
import { LocalStorageService } from '../../shared/wrappers/local-storage.service';
import { SessionStorageService } from '../../shared/wrappers/session-storage.service';

@Component({
  selector: 'app-purchasing',
  templateUrl: './purchasing.component.html',
  styleUrls: ['./purchasing.component.css']
})
export class PurchasingComponent implements OnInit,OnDestroy {
  @ViewChild('tabstrip') public tabstrip: TabStripComponent;
  disableTab: boolean;
  PO_HdrId: number = 0;
  poHdrId: string ='';
  subscription: Subscription;
  constructor( 
    private _commonService: CommonService,
    private _sessionStorage: SessionStorageService
  ) { }

  ngOnInit() {
    this.disableTab = true;
    this.subscription = this._commonService.notifyObservable.subscribe(
      res => {
        if (res.hasOwnProperty('key') && res.key === 'POMaintenance') {
          this.poHdrId = res.value.PoHdrID;
          this.disableTab = false;
          Promise.resolve(null).then(() => this.tabstrip.selectTab(2));
        }
        if (res.hasOwnProperty('key') && res.key === 'destroyPOMaintenance') {
          this.poHdrId = res.value.PoHdrID;
          this.disableTab = true;
        }
        if (res.hasOwnProperty('key') && res.key === 'POTab') {
          this.disableTab = false;
          Promise.resolve(null).then(() => this.tabstrip.selectTab(1));
        }
      });
  }

  ngOnDestroy(){
      this._sessionStorage.remove('SearchData');
  }

}

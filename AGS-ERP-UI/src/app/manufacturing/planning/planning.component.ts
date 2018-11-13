import { Component, OnInit,ViewChild } from '@angular/core';
import { TabStripComponent } from '@progress/kendo-angular-layout/dist/es/tabstrip/tabstrip.component';
import { CommonService } from '../../shared/services/common.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-planning',
  templateUrl: './planning.component.html',
  styleUrls: ['./planning.component.css']
})
export class PlanningComponent implements OnInit {
  @ViewChild('tabstrip') public tabstrip: TabStripComponent;  
  subscription: Subscription;

  constructor(
    private _commonService: CommonService
  ) { }

  ngOnInit() {
    this.subscription = this._commonService.notifyObservable.subscribe(
      res => {
        if (res.hasOwnProperty('key') && res.key === 'Capacity') {
          Promise.resolve(null).then(() => this.tabstrip.selectTab(0));
        }
        if (res.hasOwnProperty('key') && res.key === 'FG Future Watchlist') {
        }
      });
  }

}

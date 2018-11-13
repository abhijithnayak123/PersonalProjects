import { Component, OnInit, Input } from '@angular/core';
import { Subscription } from 'rxjs/Subscription';
import { CommonService } from '../../services/common.service';

@Component({
  selector: 'app-spinner',
  templateUrl: './spinner.component.html',
  styleUrls: ['./spinner.component.css']
})
export class SpinnerComponent implements OnInit {
  @Input() show: boolean;
  _subscription: Subscription;
  constructor(
    private _commonService: CommonService
  ) { }

  ngOnInit() {
    this._subscription = this._commonService.notifyObservable.subscribe(
      res => {
        if (res.hasOwnProperty('key') && res.key === 'showSpinner') {
          this.show = res.value;
        }
      });
  }
}

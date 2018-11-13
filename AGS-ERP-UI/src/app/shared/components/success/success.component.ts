import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs/Subscription';
import { SuccessService } from '../../services/success.service';

@Component({
  selector: 'app-success',
  templateUrl: './success.component.html',
  styleUrls: ['./success.component.css']
})
export class SuccessComponent implements OnInit {
  opened = false;
  successMessage: string;
  _subscription: Subscription;
  constructor(
    private _successService: SuccessService
  ) { }

  ngOnInit() {
    this._subscription = this._successService.notifyObservable.subscribe(
      res => {
        if (res.hasOwnProperty('key') && res.key === 'successMessage') {
          this.opened = true;
          this.successMessage = res.value;
        }
      });
  }

  close() {
    this.opened = false;
  }
}

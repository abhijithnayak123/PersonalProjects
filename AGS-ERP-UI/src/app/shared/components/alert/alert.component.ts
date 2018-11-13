import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs/Subscription';
import { AlertService } from "../../services/alert.service";

@Component({
  selector: 'app-alert',
  templateUrl: './alert.component.html',
  styleUrls: ['./alert.component.css']
})

export class AlertComponent implements OnInit {
  opened = false;
  alertMessage: string;
  _subscription: Subscription;
  constructor(
    private _alertService: AlertService
  ) { }

  ngOnInit() {
    this._subscription = this._alertService.notifyObservable.subscribe(
      res => {
        if (res.hasOwnProperty('key') && res.key === 'alertMessage') {
          this.opened = true;
          this.alertMessage = res.value;
        }
      });
  }

  close() {
    this.opened = false;
  }
}
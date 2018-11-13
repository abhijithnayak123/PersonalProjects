import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs/Rx';
import { ConfirmationService } from '../../services/confirmation.service';

@Component({
  selector: 'app-confirmation',
  templateUrl: './confirmation.component.html',
  styleUrls: ['./confirmation.component.css']
})
export class ConfirmationComponent implements OnInit, OnDestroy {
  message: string;
  subscription: Subscription;
  onContinueCallBackFunction: any;
  onCancelCallBackFunction: any;
  opened = false;

  constructor(private _confirmationService: ConfirmationService) { }

  ngOnInit() {
    this.subscription = this._confirmationService.notifyObservable.subscribe(
      res => {
        if (res.hasOwnProperty('key') && res.key === 'message') {
          this.opened = true;
          this.message = res.value.message;
          this.onContinueCallBackFunction = res.value.continueCallBackFunction;
          this.onCancelCallBackFunction = res.value.cancelCallBackFunction;
        }
      });
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  continue() {
    this.opened = false;
    this.onContinueCallBackFunction();
  }

  public cancle() {
    this.opened = false;
    if(this.onCancelCallBackFunction){
      this.onCancelCallBackFunction();
    }
  }  
  close() {
    this.cancle();
  }
}

import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs/Subscription';

import { ErrorService } from "../../services/error.service";
import { ErrorModel } from "../../models/error.model";
import { ErrorMessages } from "../../exceptions/error-messages_en";

@Component({
  selector: 'app-error',
  templateUrl: './error.component.html',
  styleUrls: ['./error.component.css']
})

export class ErrorComponent implements OnInit, OnDestroy {

  message : string;
  subscription : Subscription;
  public opened = false;
  public error: ErrorModel;
  constructor(private errorService: ErrorService) { }
  ngOnInit(): void {
    this.subscription = this.errorService.getError().subscribe(
      data => {
        if (data.error instanceof Error) {
        } else {
          this.opened = true;
          let errDesc = data.error;
          if(typeof data.error === "string"){         
           errDesc = JSON.parse(data.error);
          }  
          this.error = new ErrorModel();
          this.error.ErrorCode = errDesc.ErrorCode;
          this.error.ErrorDescription = errDesc.ErrorDescription;
          this.error.ErrorData = this.getErrorMessage(errDesc.ErrorCode).replace('{0}',errDesc.ErrorData);
        }
      }
    );
  }
  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
  public close() {
    this.opened = false;
  }

  getErrorMessage(errorCode: string){
    let errorMessage = ErrorMessages[errorCode];
    if(errorMessage){
      return errorMessage;
    }
    else{
      return "Error in application, please try again"
    }
  }
}

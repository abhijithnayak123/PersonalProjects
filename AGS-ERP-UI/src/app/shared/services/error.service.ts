import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Subject } from 'rxjs/Subject';

import { HttpErrorResponse } from "@angular/common/http";
@Injectable()
export class ErrorService {
    private errorRaised = new Subject<HttpErrorResponse>();
    constructor(){}
    error(err: HttpErrorResponse) {
        this.errorRaised.next(err);
    }

    getError() : Observable<HttpErrorResponse> {
      return this.errorRaised.asObservable();
    }
}
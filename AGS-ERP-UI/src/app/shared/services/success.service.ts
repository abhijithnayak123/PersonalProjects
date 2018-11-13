import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { Subject } from 'rxjs/Subject';

@Injectable()
export class SuccessService {
  private notify = new Subject<any>();
  notifyObservable = this.notify.asObservable();

  constructor() { }

  public success(data: any) {
    if (data) {
      this.notify.next(data);
    }
  }
}

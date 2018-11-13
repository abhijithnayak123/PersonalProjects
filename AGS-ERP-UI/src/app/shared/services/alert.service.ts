import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { Subject } from 'rxjs/Subject';

@Injectable()
export class AlertService {

  private notify = new Subject<any>();
  notifyObservable = this.notify.asObservable();

  constructor() { }

  public alert(data: any) {
    if (data) {
      this.notify.next(data);
    }
  }
}

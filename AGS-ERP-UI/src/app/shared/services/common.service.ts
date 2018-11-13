import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { Subject } from 'rxjs/Subject';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';

@Injectable()
export class CommonService {

  private notify = new Subject<any>();
  notifyObservable = this.notify.asObservable();

  constructor() { }

  public Notify(data: any) {
    if (data) {
      this.notify.next(data);
    }
  }

}

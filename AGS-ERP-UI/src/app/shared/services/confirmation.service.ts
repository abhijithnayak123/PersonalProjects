import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { Subject } from 'rxjs/Subject';

@Injectable()
export class ConfirmationService {
  private notify = new Subject<any>();
  notifyObservable = this.notify.asObservable();

  constructor() { }

  public confirm(data: any) {
    if (data) {
      this.notify.next(data);
    }
  }
}

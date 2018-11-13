import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { Subject } from 'rxjs/Subject';
import { AlertType } from '../models/alerttype.model';

@Injectable()
export class ToastService {
    private subject = new Subject<any>();
    constructor() {
    }

    getAlert(): Observable<any> {
        return this.subject.asObservable();
    }

    success(message: string) {
        this.alert(AlertType.Success, message);
    }

    error(message: string) {
        this.alert(AlertType.Error, message);
    }

    info(message: string) {
        this.alert(AlertType.Info, message);
    }

    warn(message: string) {
        this.alert(AlertType.Warning, message);
    }

    alert(type: AlertType, message: string) {
        this.subject.next({ key: 'toastMessage', value: { type: type, message: message } });
    }

}

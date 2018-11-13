import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { Subject } from 'rxjs/Subject';
import { IBreadcrumb } from "../../common/models/breadcrumb.model";

@Injectable()
export class BreadcrumbService {
    private breadCrumbsubscribe = new BehaviorSubject<any>('');
    constructor(){}
    getLocalBreadcrumb(breadCrumb: any) {
      this.breadCrumbsubscribe.next(breadCrumb);
    }

    getBreadcrumbs() : Observable<any> {
      return this.breadCrumbsubscribe.asObservable();
    }

}

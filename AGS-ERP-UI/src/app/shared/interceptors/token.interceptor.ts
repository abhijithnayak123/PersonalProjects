import { 
    HttpInterceptor, 
    HttpRequest, 
    HttpHandler, 
    HttpSentEvent, 
    HttpHeaderResponse,
    HttpProgressEvent, 
    HttpResponse,
    HttpUserEvent} from '@angular/common/http'
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { LocalStorageService } from '../wrappers/local-storage.service';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {
    
    constructor(
        private _localStorageService: LocalStorageService
    ){
    }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpSentEvent | HttpHeaderResponse | HttpProgressEvent | HttpResponse<any> | HttpUserEvent<any>> {
        
        let loggedUser = this._localStorageService.get('ags_erp_user_previlage');
        let loggedUserId: string = '';
        if(loggedUser){
            loggedUserId = JSON.parse(loggedUser).UserName;
        }
        req = req.clone({
            setHeaders: {
              user_id: loggedUserId
            }
          });
          return next.handle(req);
    }

}
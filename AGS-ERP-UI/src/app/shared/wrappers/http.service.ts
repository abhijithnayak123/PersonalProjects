import { Injectable } from "@angular/core";
import { Http, RequestOptions } from "@angular/http";
import { environment } from "./../../../environments/environment";
import { Observable } from "rxjs/Observable";
import { HttpClient, HttpErrorResponse } from "@angular/common/http";

import { BaseModel } from "../models/base.model";

@Injectable()
export class HttpService {
  private base_url = environment.apiUrl;
  private options = RequestOptions;

  constructor(private _http: HttpClient) {}

  //GET 
  get<BaseModel>(api: string) {
    return this._http.get<BaseModel>(this.base_url + api);
  }

  //POST 
  post<BaseModel>(api: string, model: any) {
    return this._http.post<BaseModel>(this.base_url + api, model);
  }

  //DELETE 
  delete<BaseModel>(api: string, value: any) {
    return this._http.delete(this.base_url + api + "/" + value);
  }

  private extractResult(res: Response) {
    let body = res.json();
    return body || {};
  }

  private handleError(err: any) {
    let errMessage = err.message ? err.message : "server error!";
    return Observable.throw(errMessage);
  }
}

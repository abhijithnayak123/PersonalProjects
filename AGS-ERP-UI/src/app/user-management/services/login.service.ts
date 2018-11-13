import { Component, OnInit, Injectable } from "@angular/core";
import { HttpService } from "../../shared/wrappers/http.service";
import { HttpClient } from "@angular/common/http";

import { LoginModel } from "../models/login.model";
import { UserModel } from "../../shared/models/user.model";
import { ErrorModel } from "../../shared/models/error.model";
import { LocalStorageService } from "../../shared/wrappers/local-storage.service";

@Injectable()
export class LoginService implements OnInit {

  constructor(
    private localStorageService : LocalStorageService,
    private _http: HttpService,
  ) {}
  postResult: LoginModel;
  error: ErrorModel;
  
  ngOnInit(): void {
    this.postResult = new LoginModel();
    this.error = new ErrorModel();
  }

  //Posting the data to service
  logindata(login: LoginModel) {
     return this._http.post<LoginModel>("user/login", login).map(response=>{return response});
  }  
}

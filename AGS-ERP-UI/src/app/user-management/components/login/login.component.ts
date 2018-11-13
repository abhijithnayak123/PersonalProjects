import { Component, OnInit } from "@angular/core";
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { HttpErrorResponse } from "@angular/common/http";
import { Router } from "@angular/router";

import { LoginService } from "../../services/login.service";
import { ErrorModel } from "../../../shared/models/error.model";
import { ErrorService } from "../../../shared/services/error.service";
import { HttpService } from "../../../shared/wrappers/http.service";
import { LocalStorageService } from "../../../shared/wrappers/local-storage.service";
import { LoginModel } from "../../models/login.model";
import { UserPrevilagesModel } from "../../models/user-previlages.model";

@Component({
  templateUrl: "./login.component.html",
  styleUrls: ["./login.component.css"],
  providers: [UserPrevilagesModel]
})
export class LoginComponent implements OnInit {
  constructor(
    private _loginService: LoginService,
    private _errorService: ErrorService,
    private _localStorageService: LocalStorageService,
    private _userprevilages: UserPrevilagesModel,
    private _router: Router
  ) {
  }
  signInForm: FormGroup;
  error: ErrorModel;
  mail: string;
  show: boolean;
  ngOnInit() {
    this.show = false;
    this.signInForm = this.createSignInForm();
    this.error = new ErrorModel();
    localStorage.removeItem('breadcrumbs');
  }

  createSignInForm(): FormGroup {
    let loginModel: LoginModel = new LoginModel();
    let login = this._localStorageService.get("ags_erp_user_login");
    if (login) {
      loginModel = JSON.parse(login);
    }
    let signInForm = new FormGroup({
      username: new FormControl(loginModel.username, [Validators.required]),
      password: new FormControl(loginModel.password, [Validators.required]),
      rememberMe: new FormControl(loginModel.rememberMe)
    });
    return signInForm;
  }

  submit() {
    if (this.signInForm.valid) {
      this.show = true;
      let loginModel = this.signInForm.value;
      this._loginService.logindata(loginModel).subscribe(
        data => {
          this.userData(data);
          if (loginModel.rememberMe) {
            this._localStorageService.add(
              "ags_erp_user_login",
              JSON.stringify(loginModel)
            );
          } else {
            this._localStorageService.remove("ags_erp_user_login");
          }
          this.show = false;
          this._router.navigate(['/']);
        },
        (err: HttpErrorResponse) => {
          this.show = false;
          this._errorService.error(err);
        }
      );
    }
  }
  
  userData(resp) {
    this._userprevilages.FirstName = resp.FirstName;
    this._userprevilages.LastName = resp.LastName;
    this._userprevilages.FullName = resp.FullName;
    this._userprevilages.UserName = resp.UserName;
    this._userprevilages.ShortName = resp.RoleUsers[0].Role.ShortName;
    this._userprevilages.RolePermissions =
      resp.RoleUsers[0].Role.RolePermissions;
    this._localStorageService.add(
      "ags_erp_user_previlage",
      JSON.stringify(this._userprevilages)
    );
    return this._userprevilages;
  }

  

}

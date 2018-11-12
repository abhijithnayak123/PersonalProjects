import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { UserInfo } from './../../data-elements/user-info';
import { CandidateRegistrationService } from './../../services/candidate-registration.service';
import { MdlDialogService, MdlDialogReference } from '@angular-mdl/core';
import { AlertPopupComponent } from './../common/alert-popup/alert-popup.component';
import { ConfirmPopupComponent } from './../common/confirm-popup/confirm-popup.component';
import { DisplayPopup } from './../../data-elements/display-popup';

@Component({
  selector: 'app-admin-login',
  templateUrl: './admin-login.component.html',
  styleUrls: ['./admin-login.component.css'],
  providers: [CandidateRegistrationService]
})
export class AdminLoginComponent implements OnInit {

  _userInfo = new UserInfo();
  _isLoading = false;

  constructor(private _candRegService: CandidateRegistrationService, private _router: Router, private _dialog: MdlDialogService) { }

  ngOnInit() {
  }

  Login() {
    this._isLoading = true;
    this._candRegService.authenticate(this._userInfo)
      .catch(e => {
        this._isLoading = false;
        alert(e.message);
        return null;
      })
      .subscribe(resp => this.onAuthenticate(resp));
  }

  onAuthenticate(resp) {
    console.log(resp);
    if (resp != null && resp === 'Success') {
      sessionStorage.setItem('userName', this._userInfo.UserName);
      this._router.navigate(['adminflow/admincandidateregistration']);
    } else {
      const _displaypopup = new DisplayPopup();
      _displaypopup.Title = 'Error';
      _displaypopup.Message = resp;
      _displaypopup.ButtonText = 'OK';
      sessionStorage.setItem('Display-PopUp', JSON.stringify({
        DisplayPopup: _displaypopup
      }));
      const pDialog = this._dialog.showCustomDialog({
        component: AlertPopupComponent,
        isModal: true,
        styles: { 'width': '300px' },
        clickOutsideToClose: true,
        enterTransitionDuration: 400,
        leaveTransitionDuration: 400
      });
      pDialog.subscribe((dialogReference: MdlDialogReference) => {
      });
      this._isLoading = false;
    }
  }
}

import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { CandidateDashboardService } from './../../../services/candidate-dashboard.service';
import { CandidateStatus } from './../../../data-elements/candidate-status';
import { transition, trigger, state, style, animate, group, keyframes, sequence } from '@angular/animations';
import { MdlDialogService, MdlDialogReference } from '@angular-mdl/core';
import { AlertPopupComponent } from './../../common/alert-popup/alert-popup.component';
import { DisplayPopup } from './../../../data-elements/display-popup';

@Component({
  selector: 'app-generic-popup',
  templateUrl: './generic-popup.component.html',
  styleUrls: ['./generic-popup.component.css'],
  animations: [
    trigger('dialog', [
      transition('void => *', [
        style({ transform: 'scale3d(.3, .3, .3)' }),
        animate(100)
      ]),
      transition('* => void', [
        animate(100, style({ transform: 'scale3d(.0, .0, .0)' }))
      ])
    ])
  ],
  providers: [CandidateDashboardService, MdlDialogService]
})
export class GenericPopupComponent implements OnInit {

  _candStatus = new CandidateStatus();

  constructor(private _candDash: CandidateDashboardService, private _router: Router,
    private dialog: MdlDialogReference, private _dialog: MdlDialogService) { }

  ngOnInit() {
  }

  public close() {
    this.dialog.hide();
  }

  public declined() {
    const _context = JSON.parse(sessionStorage.getItem('Candidate_Token'));
    this._candStatus.Token = _context.Token;
    this._candStatus.IsActive = false;
    this._candStatus.IsNewCandidate = false;
    console.log(this._candStatus.DeclineReason);
    if (this._candStatus.DeclineReason === undefined || this._candStatus.DeclineReason === '') {
      this.showAlertPopUp('Error', 'Please enter a reson', 'OK');
    } else {
      this._candDash.updateReason(this._candStatus)
        .catch(e => {
          alert(e.message);
          return null;
        })
        .subscribe(resp => this.onAccept(resp));
    }
  }
  onAccept(resp) {
    this.dialog.hide();
    if (resp !== null && resp !== '') {
      this._router.navigate(['candidatelogin']);
    }
  }

  showAlertPopUp(title, strmessage, buttonTxt) {
    this.displayPopUpMessage(title, strmessage, buttonTxt);
    const pDialog = this._dialog.showCustomDialog({
      component: AlertPopupComponent,
      isModal: true,
      styles: {'width': '300px'},
      clickOutsideToClose: true,
      enterTransitionDuration: 400,
      leaveTransitionDuration: 400
    });
    pDialog.subscribe( (dialogReference: MdlDialogReference) => {
    });
  }

  displayPopUpMessage(title, strmessage, buttonTxt) {
    const _displaypopup = new DisplayPopup();
    _displaypopup.Title = title;
    _displaypopup.Message = strmessage;
    _displaypopup.ButtonText = buttonTxt;
    sessionStorage.setItem('Display-PopUp', JSON.stringify({
      DisplayPopup: _displaypopup
    }));
  }
}

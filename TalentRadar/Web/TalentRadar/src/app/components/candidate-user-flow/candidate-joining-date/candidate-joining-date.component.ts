import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MdlDialogService, MdlDialogReference } from '@angular-mdl/core';
import { GenericPopupComponent } from './../../common/generic-popup/generic-popup.component';
import { CandidateDetails } from './../../../data-elements/candidate-details';
import { CandidateDashboardService } from './../../../services/candidate-dashboard.service';
import { CandidateStatus } from './../../../data-elements/candidate-status';

@Component({
  selector: 'app-candidate-joining-date',
  templateUrl: './candidate-joining-date.component.html',
  styleUrls: ['./candidate-joining-date.component.css'],
  providers: [CandidateDashboardService, MdlDialogService],
})
export class CandidateJoiningDateComponent implements OnInit {

  _candDetails = new CandidateDetails();
  _candStatus = new CandidateStatus();
  _isLoading = false;
  constructor(private _candDash: CandidateDashboardService, private _router: Router, private _dialog: MdlDialogService) { }

  ngOnInit() {
  }

  showPopUp() {
    const pDialog = this._dialog.showCustomDialog({
      component: GenericPopupComponent,
      isModal: true,
      styles: { 'width': '350px' },
      clickOutsideToClose: true,
      enterTransitionDuration: 400,
      leaveTransitionDuration: 400
    });
    pDialog.subscribe((dialogReference: MdlDialogReference) => {
    });
  }

  acceptJoiningDate() {
    this._isLoading = true;
    const _context = JSON.parse(sessionStorage.getItem('Candidate_Token'));
    this._candStatus.Token = _context.Token;
    this._candStatus.DeclineReason = '';
    this._candStatus.IsActive = true;
    this._candStatus.IsNewCandidate = false;
    this._candDash.updateReason(this._candStatus)
      .catch(e => {
        alert(e.message);
        return null;
      })
      .subscribe(resp => this.onAccept(resp));


  }
  onAccept(resp) {
    if (resp != null && resp !== '') {
      this._router.navigate(['candidateflow/candidatedashboard']);
    }
  }

}

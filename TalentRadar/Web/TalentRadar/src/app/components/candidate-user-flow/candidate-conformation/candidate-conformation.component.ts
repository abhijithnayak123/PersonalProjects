import { Component, OnInit, ViewChild, Directive } from '@angular/core';
import { Router } from '@angular/router';
import { CandidateDetails } from './../../../data-elements/candidate-details';
import { CandidateDashboardService } from './../../../services/candidate-dashboard.service';
import { CandidateStatus } from './../../../data-elements/candidate-status';
import { MdlDialogService, MdlDialogReference } from '@angular-mdl/core';
import { GenericPopupComponent } from './../../common/generic-popup/generic-popup.component';

@Component({
  selector: 'app-candidate-conformation',
  templateUrl: './candidate-conformation.component.html',
  styleUrls: ['./candidate-conformation.component.css'],
  providers: [CandidateDashboardService, MdlDialogService],
})

export class CandidateConformationComponent implements OnInit {

  _candDetails = new CandidateDetails();
  _candStatus = new CandidateStatus();
  _isLoading = false;

  constructor(private _candDash: CandidateDashboardService, private _router: Router, private _dialog: MdlDialogService) { }

  ngOnInit() {
    const _context = JSON.parse(sessionStorage.getItem('Candidate_Token'));
    this._candDash.getCandidateBasedOnToken(_context.Token)
      .catch(e => {
        alert(e.message);
        return null;
      })
      .subscribe(resp => this.onGet(resp));


  }

  showPopUp() {
    const pDialog = this._dialog.showCustomDialog({
      component: GenericPopupComponent,
      isModal: true,
      styles: {'width': '350px'},
      clickOutsideToClose: true,
      enterTransitionDuration: 400,
      leaveTransitionDuration: 400
    });
    pDialog.subscribe( (dialogReference: MdlDialogReference) => {
    });
  }


  onGet(resp) {
    if (resp !== null && resp !== '') {
      if (resp.IsNewCandidate) {
      this._candDetails.FirstName = resp.FirstName;
      this._candDetails.LastName = resp.LastName;
      this._candDetails.PositionName = resp.PositionName;
      this._candDetails.JobDescription = resp.JobDescription;
      } else {
        this._router.navigate(['CandidateFlow/CandidateDashboard']);
      }
    }
  }

  acceptConformation() {
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

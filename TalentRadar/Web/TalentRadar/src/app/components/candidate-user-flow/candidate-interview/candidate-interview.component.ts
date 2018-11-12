import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { CandidateDashboardService } from './../../../services/candidate-dashboard.service';
import { CandidateDetails } from './../../../data-elements/candidate-details';
import { MdlDialogService, MdlDialogReference } from '@angular-mdl/core';
import { GenericPopupComponent } from './../../common/generic-popup/generic-popup.component';

@Component({
  selector: 'app-candidate-interview',
  templateUrl: './candidate-interview.component.html',
  styleUrls: ['./candidate-interview.component.css'],
  providers: [CandidateDashboardService]
})
export class CandidateInterviewComponent implements OnInit {

  _candDetails = new CandidateDetails();

  constructor(private _candDash: CandidateDashboardService, private _dialog: MdlDialogService) { }

  ngOnInit() {

    const _context = JSON.parse(sessionStorage.getItem('Candidate_Token'));
    this._candDash.getCandidateBasedOnToken(_context.Token)
      .catch(e => {
        alert(e.message);
        return null;
      })
      .subscribe(resp => this.onGet(resp));

  }

  onGet(resp) {
    this._candDetails.FirstName = resp.FirstName;
    this._candDetails.LastName = resp.LastName;
    this._candDetails.PositionName = resp.PositionName;
    this._candDetails.JobDescription = resp.JobDescription;
    this._candDetails.InterviewDate = resp.InterviewDate;
    this._candDetails.InterviewTime = resp.InterviewTime;
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

}

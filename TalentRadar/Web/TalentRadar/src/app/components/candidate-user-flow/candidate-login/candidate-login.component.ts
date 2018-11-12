import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { CandidateLogin } from './../../../data-elements/candidate-login';
import { CandidateDetails } from './../../../data-elements/candidate-details';
import { CandidateDashboardService } from './../../../services/candidate-dashboard.service';
import { MdlDialogService } from '@angular-mdl/core';

@Component({
  selector: 'app-candidate-login',
  templateUrl: './candidate-login.component.html',
  styleUrls: ['./candidate-login.component.css'],
  providers: [CandidateDashboardService]
})
export class CandidateLoginComponent implements OnInit {

  _isLoading= false;
  _candlogin = new CandidateLogin();
  _candDetails = new CandidateDetails();

  constructor(private _candDash: CandidateDashboardService, private _router: Router, private _dialog: MdlDialogService) { }

  ngOnInit() {

  }

  Login() {
    this._isLoading = true;
    this._candDash.getCandidateDetails(this._candlogin)
      .catch(e => {
        this._isLoading = false;
        this._dialog.alert(e.message, 'OK', 'Error');
        return null;
      })
      .subscribe(resp => this.onGetCandidateDetails(resp));
  }

  onGetCandidateDetails(resp) {
    if (resp != null && resp !== '') {
      this._candDetails.IsNewCandidate = resp.IsNewCandidate;
      this._candDetails.IsActive = resp.IsActive;
      sessionStorage.setItem('Candidate_Token', JSON.stringify({
             Token: resp.Token
      }));
      if (this._candDetails.IsNewCandidate) {
        this._router.navigate(['candidateconformation']);
      } else if (!this._candDetails.IsNewCandidate && this._candDetails.IsActive) {
        this._router.navigate(['candidateflow/candidatedashboard']);
      } else {
        this._router.navigate(['candidatelogin']);
      }
      this._isLoading = false;
    } else {
      this._dialog.alert('Please provide valid email and mobile number', 'OK', 'Message');
    }
  }
}

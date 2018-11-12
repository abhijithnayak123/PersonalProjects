import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { Notification } from './../../data-elements/notification';
import { InterviewStage } from './../../data-elements/interview-stage';
import { CandidateStatus } from './../../data-elements/candidate-status-enum';
import { CandidateNotification } from './../../data-elements/candidate-notification';
import { CandidateDashboardService } from './../../services/candidate-dashboard.service';

@Component({
  selector: 'app-candidate-notification',
  templateUrl: './candidate-notification.component.html',
  styleUrls: ['./candidate-notification.component.css'],
  providers: [CandidateDashboardService],
})
export class CandidateNotificationComponent implements OnInit {
  _candNotification = new CandidateNotification();
  _notifications: Notification[] = [];
  _stage: InterviewStage = new InterviewStage();
  _isLoading: Boolean = false;
  constructor(private _candDash: CandidateDashboardService, private _router: Router) { }
  ngOnInit() {
    const _context = JSON.parse(sessionStorage.getItem('Candidate_Token'));
    this._candDash.getCandidateNotification(_context.Token)
            .catch(e => {
                alert(e.message);
                return null;
            })
            .subscribe(resp => this.onGet(resp));
  }
  onGet(resp) {
    if (resp !== null && resp !== '') {
      this._notifications = resp.Notifications;
      this._candNotification.CandidateStatus = resp.CandidateStatus;
      this._candNotification.Notifications = this._notifications;
    }
  }

  redirect() {
    this._isLoading = true;
    if (this._candNotification.CandidateStatus === 'Shortlisted') {
      this._router.navigate(['candidateflow/candidateshortlist']);
    } else if (this._candNotification.CandidateStatus === 'Interview') {
      this._router.navigate(['candidateflow/candidateinterview']);
    } else if (this._candNotification.CandidateStatus === 'Offer') {
      this._router.navigate(['candidateflow/candidateoffer']);
    } else if (this._candNotification.CandidateStatus === 'OnBoarding') {
      this._router.navigate(['candidateflow/candidatejoiningdate']);
    }
    this._isLoading = false;
  }

}

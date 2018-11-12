import { Component, OnInit } from '@angular/core';
import { Recruiter } from './../../data-elements/recruiter';
import { CandidateDashboardService } from './../../services/candidate-dashboard.service';

@Component({
  selector: 'app-recruiter-details',
  templateUrl: './recruiter-details.component.html',
  styleUrls: ['./recruiter-details.component.css'],
  providers: [CandidateDashboardService],
})
export class RecruiterDetailsComponent implements OnInit {

  _recruiter: Recruiter = new Recruiter();

  constructor(private _candDash: CandidateDashboardService) { }

  ngOnInit() {
     const _context = JSON.parse(sessionStorage.getItem('Candidate_Token'));
     this._candDash.getRecuirterDetails(_context.Token)
     .catch(e => {
        alert(e.message);
        return null;
      })
      .subscribe(resp => this.onGet(resp));
  }
  onGet(resp) {
    if (resp != null && resp !== '') {
      this._recruiter.FullName = resp.FullName;
      this._recruiter.MobileNo = resp.MobileNo;
      this._recruiter.Email = resp.Email;
    }
  }
}

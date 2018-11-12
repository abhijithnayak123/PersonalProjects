import { Component, OnInit } from '@angular/core';
import { CandidateDashboardService } from './../../../services/candidate-dashboard.service';
import { CandidateDetails } from './../../../data-elements/candidate-details';

@Component({
    selector: 'app-candidate-shortlist',
    templateUrl: './candidate-shortlist.component.html',
    styleUrls: ['./candidate-shortlist.component.css'],
    providers: [CandidateDashboardService]
})
export class CandidateShortlistComponent implements OnInit {

    _candDetails = new CandidateDetails();
    constructor(private _candDash: CandidateDashboardService) { }

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
    }

}

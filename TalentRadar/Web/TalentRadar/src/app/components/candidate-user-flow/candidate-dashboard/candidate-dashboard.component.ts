import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { CandidateDashboardService } from './../../../services/candidate-dashboard.service';
import { CandidateDetails } from './../../../data-elements/candidate-details';
import { CandidateStatus } from './../../../data-elements/candidate-status-enum';
import { transition, trigger, state, style, animate, group, keyframes, sequence } from '@angular/animations';

@Component({
    selector: 'app-candidate-dashboard',
    templateUrl: './candidate-dashboard.component.html',
    styleUrls: ['./candidate-dashboard.component.css'],
    providers: [CandidateDashboardService],
    animations: [
        trigger('destop-screen', [
            state('1', style({ marginTop: '-24%', marginLeft: '35%', })),
            transition('void => 1', [
                animate('2s', keyframes([
                    style({ opacity: 1,  offset: 0.3 }),
                    style({ opacity: 1, marginTop: '-24%', marginLeft: '35%', offset: 1.0 })
                ]))
            ]),
            state('2', style({ marginTop: '-95%', marginLeft: '30%', })),
            transition('1 => 2', [
                animate('2s', keyframes([
                    style({ opacity: 1,  offset: 0.3 }),
                    style({ opacity: 1, marginTop: '-95%', marginLeft: '30%', offset: 1.0 })
                ]))
            ]),
            state('3', style({ marginTop: '-165%', marginLeft: '45%', })),
            transition('1 => 3', [
                animate('1.5s', keyframes([
                    style({ opacity: 1, marginTop: '-24%', marginLeft: '35%', offset: 0.1 }),
                    style({ opacity: 1, marginTop: '-95%', marginLeft: '30%', offset: 0.6 }),
                    style({ opacity: 1, marginTop: '-165%', marginLeft: '45%', offset: 1.0 }),
                ]))
            ]),
            state('4', style({ marginTop: '-240%', marginLeft: '10%', })),
            transition('1 => 4', [
                animate('4s', keyframes([
                    style({ opacity: 1, marginTop: '-24%', marginLeft: '35%', offset: 0.1 }),
                    style({ opacity: 1, marginTop: '-95%', marginLeft: '30%', offset: 0.3 }),
                    style({ opacity: 1, marginTop: '-165%', marginLeft: '45%', offset: 0.6 }),
                    style({ opacity: 1, marginTop: '-240%', marginLeft: '10%', offset: 1.0 })
                ]))
            ]),
        ]),
        trigger('tablet-screen', [
            state('1', style({ marginTop: '-24%', marginLeft: '40%', })),
            transition('void => 1', [
                animate('3s', keyframes([
                    style({ opacity: 1,  offset: 0.6 }),
                    style({ opacity: 1, marginTop: '-24%', marginLeft: '40%', offset: 1.0 })
                ]))
            ]),
            state('2', style({ marginTop: '-95%', marginLeft: '23%', })),
            transition('1 => 2', [
                animate('2s', keyframes([
                    style({ opacity: 1,  offset: 0.3 }),
                    style({ opacity: 1, marginTop: '-95%', marginLeft: '23%', offset: 1.0 })
                ]))
            ]),
            state('3', style({ marginTop: '-160%', marginLeft: '50%', })),
            transition('1 => 3', [
                animate('1.5s', keyframes([
                    style({ opacity: 1, marginTop: '-24%', marginLeft: '40%', offset: 0.1 }),
                    style({ opacity: 1, marginTop: '-95%', marginLeft: '23%', offset: 0.6 }),
                    style({ opacity: 1, marginTop: '-160%', marginLeft: '50%', offset: 1.0 }),
                ]))
            ]),
            state('4', style({ marginTop: '-230%', marginLeft: '10%', })),
            transition('1 => 4', [
                animate('4s', keyframes([
                    style({ opacity: 1, marginTop: '-24%', marginLeft: '40%', offset: 0.1 }),
                    style({ opacity: 1, marginTop: '-95%', marginLeft: '23%', offset: 0.3 }),
                    style({ opacity: 1, marginTop: '-160%', marginLeft: '50%', offset: 0.6 }),
                    style({ opacity: 1, marginTop: '-230%', marginLeft: '10%', offset: 1.0 })
                ]))
            ]),
        ]),
        trigger('ipad-screen', [
            state('1', style({ marginTop: '-23%', marginLeft: '35%', })),
            transition('void => 1', [
                animate('2s', keyframes([
                    style({ opacity: 1,  offset: 0.3 }),
                    style({ opacity: 1, marginTop: '-23%', marginLeft: '35%', offset: 1.0 })
                ]))
            ]),
            state('2', style({ marginTop: '-85%', marginLeft: '30%', })),
            transition('1 => 2', [
                animate('2s', keyframes([
                    style({ opacity: 1,  offset: 0.3 }),
                    style({ opacity: 1, marginTop: '-85%', marginLeft: '30%', offset: 1.0 })
                ]))
            ]),
            state('3', style({ marginTop: '-150%', marginLeft: '45%', })),
            transition('1 => 3', [
                animate('1.5s', keyframes([
                    style({ opacity: 1, marginTop: '-23%', marginLeft: '35%', offset: 0.1 }),
                    style({ opacity: 1, marginTop: '-85%', marginLeft: '30%', offset: 0.6 }),
                    style({ opacity: 1, marginTop: '-150%', marginLeft: '45%', offset: 1.0 }),
                ]))
            ]),
            state('4', style({ marginTop: '-220%', marginLeft: '10%', })),
            transition('1 => 4', [
                animate('4s', keyframes([
                    style({ opacity: 1, marginTop: '-23%', marginLeft: '35%', offset: 0.1 }),
                    style({ opacity: 1, marginTop: '-85%', marginLeft: '30%', offset: 0.3 }),
                    style({ opacity: 1, marginTop: '-150%', marginLeft: '45%', offset: 0.6 }),
                    style({ opacity: 1, marginTop: '-220%', marginLeft: '10%', offset: 1.0 })
                ]))
            ]),
        ]),
        trigger('smartphone-screen', [
            state('1', style({ marginTop: '-23%', marginLeft: '33%', })),
            transition('void => 1', [
                animate('2s', keyframes([
                    style({ opacity: 1,  offset: 0.3 }),
                    style({ opacity: 1, marginTop: '-23%', marginLeft: '33%', offset: 1.0 })
                ]))
            ]),
            state('2', style({ marginTop: '-80%', marginLeft: '24%', })),
            transition('1 => 2', [
                animate('2s', keyframes([
                    style({ opacity: 1,  offset: 0.3 }),
                    style({ opacity: 1, marginTop: '-80%', marginLeft: '24%', offset: 1.0 })
                ]))
            ]),
            state('3', style({ marginTop: '-135%', marginLeft: '44%', })),
            transition('1 => 3', [
                animate('1.5s', keyframes([
                    style({ opacity: 1, marginTop: '-23%', marginLeft: '33%', offset: 0.1 }),
                    style({ opacity: 1, marginTop: '-80%', marginLeft: '24%', offset: 0.6 }),
                    style({ opacity: 1, marginTop: '-135%', marginLeft: '44%', offset: 1.0 }),
                ]))
            ]),
            state('4', style({ marginTop: '-190%', marginLeft: '10%', })),
            transition('1 => 4', [
                animate('4s', keyframes([
                    style({ opacity: 1, marginTop: '-23%', marginLeft: '33%', offset: 0.1 }),
                    style({ opacity: 1, marginTop: '-80%', marginLeft: '24%', offset: 0.3 }),
                    style({ opacity: 1, marginTop: '-135%', marginLeft: '44%', offset: 0.6 }),
                    style({ opacity: 1, marginTop: '-190%', marginLeft: '10%', offset: 1.0 })
                ]))
            ]),
        ]),
        trigger('iphone-screen', [
            state('1', style({ marginTop: '-20%', marginLeft: '30%', })),
            transition('void => 1', [
                animate('2s', keyframes([
                    style({ opacity: 1,  offset: 0.3 }),
                    style({ opacity: 1, marginTop: '-20%', marginLeft: '30%', offset: 1.0 })
                ]))
            ]),
            state('2', style({ marginTop: '-70%', marginLeft: '30%', })),
            transition('1 => 2', [
                animate('2s', keyframes([
                    style({ opacity: 1,  offset: 0.3 }),
                    style({ opacity: 1, marginTop: '-70%', marginLeft: '30%', offset: 1.0 })
                ]))
            ]),
            state('3', style({ marginTop: '-119%', marginLeft: '40%', })),
            transition('1 => 3', [
                animate('1.5s', keyframes([
                    style({ opacity: 1, marginTop: '-20%', marginLeft: '30%', offset: 0.1 }),
                    style({ opacity: 1, marginTop: '-70%', marginLeft: '30%', offset: 0.6 }),
                    style({ opacity: 1, marginTop: '-119%', marginLeft: '40%', offset: 1.0 }),
                ]))
            ]),
            state('4', style({ marginTop: '-166%', marginLeft: '11%', })),
            transition('1 => 4', [
                animate('4s', keyframes([
                    style({ opacity: 1, marginTop: '-20%', marginLeft: '30%', offset: 0.1 }),
                    style({ opacity: 1, marginTop: '-70%', marginLeft: '30%', offset: 0.3 }),
                    style({ opacity: 1, marginTop: '-119%', marginLeft: '40%', offset: 0.6 }),
                    style({ opacity: 1, marginTop: '-166%', marginLeft: '11%', offset: 1.0 })
                ]))
            ]),
        ])
    ]
})
export class CandidateDashboardComponent implements OnInit {

    _isShortListed = false;
    _isInterview = false;
    _isOffer = false;
    _isOnBoarding = false;
    _start = true;
    _candDetails = new CandidateDetails();
    _statusCount = 1;

    constructor(private _candDash: CandidateDashboardService, private _router: Router) {
        const _context = JSON.parse(sessionStorage.getItem('Candidate_Token'));
        this._candDash.getCandidateBasedOnToken(_context.Token)
            .catch(e => {
                alert(e.message);
                return null;
            })
            .subscribe(resp => this.onGet(resp));
    }

    ngOnInit() {

    }

    onGet(resp) {
        if (resp != null && resp !== '') {
            if (resp.CandidateStatus === CandidateStatus.Shortlisted) {
                this._statusCount = 1;
                this._isShortListed = true;
            } else if (resp.CandidateStatus === CandidateStatus.Interview) {
                this._statusCount = 2;
                this._isInterview = true;
            } else if (resp.CandidateStatus === CandidateStatus.Offer) {
                this._statusCount = 3;
                this._isOffer = true;
            } else if (resp.CandidateStatus === CandidateStatus.OnBoarding) {
                this._statusCount = 4;
                this._isOnBoarding = true;
            }
        }
    }

    redirectToOnBoard() {
        this._router.navigate(['candidateflow/candidatejoiningdate']);
    }
    redirectToOffer() {
        this._router.navigate(['candidateflow/candidateoffer']);
    }
    redirectToInterview() {
        this._router.navigate(['candidateflow/candidateinterview']);
    }
    redirectToShortList() {
        this._router.navigate(['candidateflow/candidateshortlist']);
    }
}

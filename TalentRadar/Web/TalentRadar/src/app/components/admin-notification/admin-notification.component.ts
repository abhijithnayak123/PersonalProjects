import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { Practice } from './../../data-elements/practice';
import { Position } from './../../data-elements/position';
import { InterviewStage } from './../../data-elements/interview-stage';
import { InterviewType } from './../../data-elements/interview-type';
import { CandidateRegistration } from './../../data-elements/candidate-registration';
import { CandidateRegistrationService } from './../../services/candidate-registration.service';
import { CandidateDetails } from './../../data-elements/candidate-details';

@Component({
  selector: 'app-admin-notification',
  templateUrl: './admin-notification.component.html',
  styleUrls: ['./admin-notification.component.css'],
  providers: [CandidateRegistrationService]
})

export class AdminNotificationComponent implements OnInit {

  _isFilter = false;
  _filterText = 'filter_list';
  _lst_Practices: Practice[] = [];
  _lst_Positions: Position[] = [];
  _lst_Interview_Stage: InterviewStage[] = [];
  _lst_Interview_Type: InterviewType[] = [];
  _lst_CandidateDetails: CandidateDetails[] = [];
  _candidateDetails: CandidateDetails = new CandidateDetails();
  _selPract = '';
  _selPos = '';
  _selIntervStg = '';
  _selIntervTyp = '';
  _activePage = 0;
  _pages;
  _search = '';
  _numberOfRecord = 5;

  constructor(private _candRegService: CandidateRegistrationService, private _router: Router) { }

  ngOnInit() {
    this.GetPractices();
    this.GetPositions();
    this.GetInterViewStages();
    this.GetInterViewTypes();
    this.GetCandidateDetails(this._candidateDetails, this._activePage);
  }

  GetPractices(): Promise<Practice[]> {
    this._candRegService.getPractices()
      .subscribe(resp => this._lst_Practices = resp);
    return Promise.resolve(this._lst_Practices);
  }

  GetPositions(): Promise<Position[]> {
    this._candRegService.getPositions()
      .subscribe(resp => this._lst_Positions = resp);
    return Promise.resolve(this._lst_Positions);
  }

  GetInterViewStages(): Promise<InterviewStage[]> {
    this._candRegService.getInterViewStages()
      .subscribe(resp => this._lst_Interview_Stage = resp);
    return Promise.resolve(this._lst_Interview_Stage);
  }

  GetInterViewTypes(): Promise<InterviewType[]> {
    this._candRegService.getInterViewTypes()
      .subscribe(resp => this._lst_Interview_Type = resp);
    return Promise.resolve(this._lst_Interview_Type);
  }

  GetCandidateDetails(candidateDetails, pageNumber) {
    this._candRegService.getCandidateList(candidateDetails, (pageNumber * this._numberOfRecord), this._numberOfRecord)
    .catch(e => {
      alert(e.message);
      return null;
    })
    .subscribe(resp => this.onSearchResult(resp));
  }

  onSearchResult(resp) {
    console.log(resp);
    this._lst_CandidateDetails = resp;
    if (resp.length !== 0) {
      this.Pagination(resp[0].NumberCandidate);
    } else {
      this.Pagination(0);
    }
  }

  onExportResult(resp) {
    console.log(resp);
  }

  FilterToggle() {
    if (this._isFilter) {
      this._filterText = 'filter_list';
      this._isFilter = false;
    } else {
      this._filterText = 'clear_all';
      this._isFilter = true;
    }
  }

  Pagination(totalRecords) {
    console.log(totalRecords);
    this._pages = new Array(Math.ceil(totalRecords / this._numberOfRecord));
  }

  GetRecords(pageNumber) {
    this.getValue();
    this._activePage = pageNumber - 1;
    this.GetCandidateDetails(this._candidateDetails, this._activePage);
  }

  GetList() {
    this.getValue();
    this.GetCandidateDetails(this._candidateDetails, 0);
  }

  ExportData() {
    this._candRegService.exportData(this._candidateDetails, this._activePage, this._numberOfRecord)
    .catch(e => {
      alert(e.message);
      return null;
    })
    .subscribe(resp => this.onExportResult(resp));
  }

  CandidateSearch(email) {
    sessionStorage.setItem('Admin_Candidate_Search', JSON.stringify({
      Email: email
    }));
    this._router.navigate(['adminflow/admincandidateregistration']);
  }

  getValue() {
    if (this._selPos !== '') {
      this._candidateDetails.PositionName = this._selPos;
    }
    if (this._selPract !== '') {
      this._candidateDetails.PracticeName = this._selPract;
    }
    if (this._selIntervStg !== '') {
      this._candidateDetails.InterviewStageName = this._selIntervStg;
    }
    if (this._selIntervTyp !== '') {
      this._candidateDetails.InterviewTypeName = this._selIntervTyp;
    }
    if (this._search !== '') {
      if (this._search.indexOf('@') !== -1) {
        this._candidateDetails.Email = this._search;
      } else {
        this._candidateDetails.Mobile = this._search;
      }
    }
  }
}

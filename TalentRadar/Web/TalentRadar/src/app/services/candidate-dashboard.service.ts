import { Injectable } from '@angular/core';
import { Headers, Http, Request, Response } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import { WEB_API, WEB_API_CANDIDATE_DASHBORD } from './service.config';
import { CandidateLogin } from './../data-elements/candidate-login';
import { CandidateStatus } from './../data-elements/candidate-status';

@Injectable()
export class CandidateDashboardService {

  private headers = new Headers({ 'Content-Type': 'application/json' });

  constructor(private _http: Http) { }

  getCandidateDetails(candidateLogin: CandidateLogin) {
    return this._http.post(WEB_API_CANDIDATE_DASHBORD.GETCANDIDATE_URL, JSON.stringify(candidateLogin), {headers: this.headers })
      .map(resp => {
        return resp.json();
      })
      .catch(e => {
        if (e.message !== undefined) {
          throw new Error(e.message);
        } else if (e._body !== undefined && e.status === 500) {
          throw new Error((JSON.parse(e._body)).ExceptionMessage);
        } else {
          throw new Error('Unable to connect to the Service ! Please try again later.');
        }
      });
  }

  getCandidateBasedOnToken(filterData) {
    return this._http.get(WEB_API.SEARCH_CANDIDATE_URL + filterData + '/getcandidate')
      .map((resp: Response) => resp.json());
  }

  getCandidateNotification(filterData) {
    return this._http.get(WEB_API.SEARCH_CANDIDATE_URL + filterData + '/getnotification')
      .map((resp: Response) => resp.json());
  }

  getRecuirterDetails(filterData) {
    return this._http.get(WEB_API.SEARCH_CANDIDATE_URL + filterData + '/getrecuirter')
      .map((resp: Response) => resp.json());
  }

  updateReason(candidateStatus: CandidateStatus) {
    return this._http.post(WEB_API_CANDIDATE_DASHBORD.UPDATECANDIDATESTATUS_URL, JSON.stringify(candidateStatus), {headers: this.headers })
      .map(resp => {
        return resp.json();
      })
      .catch(e => {
        if (e.message !== undefined) {
          throw new Error(e.message);
        } else if (e._body !== undefined && e.status === 500) {
          throw new Error((JSON.parse(e._body)).ExceptionMessage);
        } else {
          throw new Error('Unable to connect to the Service ! Please try again later.');
        }
      });
  }
}

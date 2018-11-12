import { Injectable } from '@angular/core';
import { Headers, Http, Request, Response, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import { Practice } from './../data-elements/practice';
import { Position } from './../data-elements/position';
import { InterviewStage } from './../data-elements/interview-stage';
import { CandidateRegistration } from './../data-elements/candidate-registration';
import { CandidateValidation } from './../data-elements/candidate-validation';
import { CandidateDetails } from './../data-elements/candidate-details';
import { UserInfo } from './../data-elements/user-info';
import { FeedBack } from './../data-elements/feed-back';
import { WEB_API } from './service.config';

@Injectable()
export class CandidateRegistrationService {

  private headers = new Headers({ 'Content-Type': 'application/json' ,'Access-Control-Allow-Origin': '*', 'Access-Control-Allow-Headers':'Origin, X-Requested-With, Content-Type, Accept','Access-Control-Allow-Methods':' GET, POST, PUT'});

  constructor(private _http: Http) { }

  getPractices() {
    return this._http.get(WEB_API.PRACTICE_URL)
      .map((resp: Response) => resp.json());
  }

  getPositions() {
    return this._http.get(WEB_API.POSITION_URL)
      .map((resp: Response) => resp.json());
  }

  getInterViewStages() {
    return this._http.get(WEB_API.INTERVIEW_STAGES_URL)
      .map((resp: Response) => resp.json());
  }

  getInterViewTypes() {
    return this._http.get(WEB_API.INTERVIEW_TYPES_URL)
      .map((resp: Response) => resp.json());
  }

  validateMobileAndEmail(candValidation: CandidateValidation) {
    return this._http.post(WEB_API.VALIDATION_URL, JSON.stringify(candValidation), { headers: this.headers })
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

  searchCandidate(filterData) {
    return this._http.get(WEB_API.SEARCH_CANDIDATE_URL + filterData + '/Find')
      .map((resp: Response) => resp.json());
  }

  candidateRegistration(candRegistration: CandidateRegistration) {
    return this._http.post(WEB_API.REGISTER_CANDIDATE_URL, JSON.stringify(candRegistration), { headers: this.headers })
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

  getCandidateList (candidateDetails: CandidateDetails , filter: number, numberOfRecord: number) {
    return this._http.post(WEB_API.GET_CANDIDATELIST_URL + filter + '/'
    + numberOfRecord + '/listCandidates' , JSON.stringify(candidateDetails), { headers: this.headers })
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

  exportData (candidateDetails: CandidateDetails , filter: number, numberOfRecord: number) {
    return this._http.post(WEB_API.EXPORT_CANDIDATELIST_URL + filter + '/'
    + numberOfRecord + '/exportCandidates', JSON.stringify(candidateDetails), { headers: this.headers })
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

  authenticate(userInfo: UserInfo) {
    return this._http.post(WEB_API.ADMIN_LOGIN_URL, JSON.stringify(userInfo), { headers: this.headers })
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

  fileUpload(formData: FormData) {
    // let _headers = new Headers();
    //     /** No need to include Content-Type in Angular 4 */
    const headers = new Headers();
    headers.append('Content-Type', 'multipart/form-data');
    headers.append('Accept', 'application/json');
    const options = new RequestOptions({ headers: headers });
    return this._http.post(WEB_API.FILEUPLOAD_URL, formData, options)
      .map(resp => {
        return resp.json();
      })
      .catch(e => {
        if (e.message !== undefined) {
          throw new Error(e.message);
        } else if (e._body !== undefined && e.status === 500) {
          throw new Error((JSON.parse(e._body)).ExceptionMessage);
        } else {
          throw new Error(e);
          // throw new Error('Unable to connect to the Service ! Please try again later.');
        }
      });
  }

  updateFeedbackComment(feedback: FeedBack) {
    return this._http.post(WEB_API.UPDATE_FEEDBACK_URL, JSON.stringify(feedback), {headers: this.headers })
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

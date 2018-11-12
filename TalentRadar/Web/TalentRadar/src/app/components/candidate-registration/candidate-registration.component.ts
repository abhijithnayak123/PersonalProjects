import { Component, OnInit, ViewChild } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { CandidateRegistration } from './../../data-elements/candidate-registration';
import { Practice } from './../../data-elements/practice';
import { Position } from './../../data-elements/position';
import { InterviewStage } from './../../data-elements/interview-stage';
import { InterviewType } from './../../data-elements/interview-type';
import { Recruiter } from './../../data-elements/recruiter';
import { CandidateRegistrationService } from './../../services/candidate-registration.service';
import { CandidateValidation } from './../../data-elements/candidate-validation';
import { MdlDialogService, MdlDialogReference, MdlDialogComponent } from '@angular-mdl/core';
import { AlertPopupComponent } from './../common/alert-popup/alert-popup.component';
import { ConfirmPopupComponent } from './../common/confirm-popup/confirm-popup.component';
import { DisplayPopup } from './../../data-elements/display-popup';
import { FeedBack } from './../../data-elements/feed-back';

@Component({
  selector: 'app-candidate-registration',
  templateUrl: './candidate-registration.component.html',
  styleUrls: ['./candidate-registration.component.css'],
  providers: [CandidateRegistrationService, MdlDialogService]
})


export class CandidateRegistrationComponent implements OnInit {

  @ViewChild('editUserDialog') private  editUserDialog: MdlDialogComponent;

  _candReg: CandidateRegistration = new CandidateRegistration();
  _lst_Practices: Practice[] = [];
  _lst_Positions: Position[] = [];
  _lst_Interview_Stage: InterviewStage[] = [];
  _lst_Interview_Type: InterviewType[] = [];

  _position: Position = new Position();
  _practice: Practice = new Practice();
  _recruiter: Recruiter = new Recruiter();
  _stage: InterviewStage = new InterviewStage();
  _interviewType: InterviewType = new InterviewType();
  _candValidate: CandidateValidation = new CandidateValidation();
  _feedback: FeedBack = new FeedBack();

  _selPract = 0;
  _selPos = 0;
  _selIntervStg = 0;
  _selIntervTyp = 0;
  _isShortListed = false;
  _isInterview = false;
  _isOffer = false;
  _isOnBoarding = false;
  _isLoading = false;
  _validationEmailnMobile = false;
  _mobileNoNotValid = false;
  _emailNotValid = false;

  _formData: FormData = new FormData();
  _candidateSearch = JSON.parse(sessionStorage.getItem('Admin_Candidate_Search'));
  constructor(private _candRegService: CandidateRegistrationService, private _router: Router,
    private _dialog: MdlDialogService) { }

  ngOnInit() {
    this._candReg.Recruiter = this._recruiter;

    this.GetPractices();
    this.GetPositions();
    this.GetInterViewStages();
    this.GetInterViewTypes();

    if (this._candidateSearch != null && this._candidateSearch.Email != null && this._candidateSearch.Email.trim() !== '') {
      this._candReg.Search = this._candidateSearch.Email;
      this.SearchCandidate();
      this._candidateSearch.Email = '';
    }
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

  onInterviewStg_Change(selValue) {
    this._isShortListed = false;
    this._isInterview = false;
    this._isOffer = false;
    this._isOnBoarding = false;

    if (selValue !== undefined) {
      switch (selValue) {
        case 1:
          {
            this._isShortListed = true;
            return;
          }
        case 2:
          {
            this._isInterview = true;
            return;
          }
        case 3:
          {
            this._isOffer = true;
            return;
          }
        case 4:
          {
            this._isOnBoarding = true;
            return;
          }
      }
    }
  }

  OnRegister() {
    if (this.Validation()) {
      this._isLoading = true;
      this._practice.Id = this._selPract.toString();
      this._candReg.Practice = this._practice;
      this._position.Id = this._selPos.toString();
      this._candReg.Position = this._position;
      this._stage.Id = this._selIntervStg.toString();
      this._candReg.Stage = this._stage;
      if (this._selIntervStg === 2) {
        this._interviewType.Id = this._selIntervTyp.toString();
        this._candReg.InterviewType = this._interviewType;
        const _enterdate = new Date(this._candReg.InterviewDate);
        const _day = _enterdate.getDate();
        const _month = _enterdate.getMonth() + 1;
        const _year = _enterdate.getFullYear();
        const _date = new Date(_month + ' ' + _day + ' ' + _year);
        const _time = _enterdate.getHours() + ':' + _enterdate.getMinutes();
        this._candReg.InterviewDate = _date;
        this._candReg.InterviewTime = _time;
      }
      this._candRegService.candidateRegistration(this._candReg)
        .catch(e => {
          alert(e.message);
          return null;
        })
        .subscribe(resp => this.onRegisterResult(resp));
    }
  }

  onRegisterResult(resp) {
    this._isLoading = false;
    if (resp != null && resp === 'true') {
      this.showConfirmPopUp('Message', 'Successfully sent email to ' + this._candReg.FirstName + ' ' + this._candReg.LastName, 'OK');
      this.Reset();
    } else if (resp != null) {
      this._dialog.alert(resp.toString());
    }
  }

  Validation() {
    if (this._candReg.FirstName === undefined || this._candReg.FirstName === '') {
      this.showAlertPopUp('Error', 'Please enter candidate First Name', 'OK');
      return false;
    } else if (this._candReg.LastName === undefined || this._candReg.LastName === '') {
      this.showAlertPopUp('Error', 'Please enter candidate last name', 'OK');
      return false;
    } else if (this._candReg.Mobile === undefined || this._candReg.Mobile === '') {
      this.showAlertPopUp('Error', 'Please enter candidate contact number', 'OK');
      return false;
    } else if (this._candReg.Email === undefined || this._candReg.Email === '') {
      this.showAlertPopUp('Error', 'Please enter candidate email id', 'OK');
      return false;
    } else if (this._selPract === undefined || this._selPract === 0) {
      this.showAlertPopUp('Error', 'Please select the practice', 'OK');
      return false;
    } else if (this._selPos === undefined || this._selPos === 0) {
      this.showAlertPopUp('Error', 'Please select the position', 'OK');
      return false;
    } else if (this._selIntervStg === undefined || this._selIntervStg === 0) {
      this.showAlertPopUp('Error', 'Please select the stage', 'OK');
      return false;
    } else if (this._candReg.Email === undefined || this._candReg.Recruiter.Email === '') {
      this.showAlertPopUp('Error', 'Please enter candidate recruiter email id', 'OK');
      return false;
    } else if (this._validationEmailnMobile) {
      this.showAlertPopUp('Error', 'The mobile number already has been used', 'OK');
      return false;
    } else if (this._emailNotValid) {
      this.showAlertPopUp('Error', 'The email address already has been used', 'OK');
      return false;
    }
    return true;
  }

  validateMobile() {
    this._validationEmailnMobile = true;
    this._isLoading = true;
    this._candValidate.Mobile = this._candReg.Mobile;
    this._candValidate.Token = this._candReg.Token;
    this._candValidate.Email = '';
    this._candRegService.validateMobileAndEmail(this._candValidate)
      .catch(e => {
        this.showAlertPopUp('Error', e.Message, 'OK');
        return null;
      })
      .subscribe(resp => this.onValidateMobileAndEmailResult(resp));

  }

  validateEmail() {
    this._validationEmailnMobile = true;
    this._isLoading = true;
    this._candValidate.Mobile = '';
    this._candValidate.Email = this._candReg.Email;
    this._candValidate.Token = this._candReg.Token;
    this._candRegService.validateMobileAndEmail(this._candValidate)
      .catch(e => {
        this.showAlertPopUp('Error', e.Message, 'OK');
        return null;
      })
      .subscribe(resp => this.onValidateMobileAndEmailResult(resp));
  }

  onValidateMobileAndEmailResult(resp) {
    this._isLoading = false;
    if (resp != null && resp !== '' && this._validationEmailnMobile === true) {
      if (this._candValidate.Email !== '') {
        this._emailNotValid = true;
      } else {
        this._mobileNoNotValid = true;
      }
      this._validationEmailnMobile = false;
      this.showAlertPopUp('Error', resp, 'OK');
    }
    return true;
  }

  SearchCandidate() {
    if (this._candReg.Search != null && this._candReg.Search.trim() !== '') {
      this._candRegService.searchCandidate(this._candReg.Search.trim())
        .catch(e => {
          this._dialog.alert(e.message, 'OK', 'Error');
          return null;
        })
        .subscribe(resp => this.onSearchCandidateResult(resp));
    } else {
      this.Reset();
    }
  }

  onSearchCandidateResult(resp) {
    if (resp !== null && resp !== '') {
      sessionStorage.removeItem('Admin_Candidate_Search');
      this.Reset();
      this._candReg.Id = resp.Id;
      this._candReg.Token = resp.Token;
      this._candReg.FirstName = resp.FirstName;
      this._candReg.LastName = resp.LastName;
      this._candReg.Mobile = resp.Mobile;
      this._candReg.Email = resp.Email;
      this._selPract = resp.Practice.Id;
      this._selPos = resp.Position.Id;
      this._selIntervStg = resp.Stage.Id;
      this._candReg.Recruiter.Email = resp.Recruiter.Email;
      this._candReg.FeedBackComment = resp.FeedBackComment;
      this._candReg.JobDescription = resp.JobDescription;
      this._selIntervTyp = resp.InterviewTyp && resp.InterviewTyp.Id && null;
      if (resp.InterviewDate) {
        const _setDate = new Date(resp.InterviewDate.substring(0, 10) + ' ' + resp.InterviewTime);
        this._candReg.InterviewDate = _setDate;
        this._candReg.InterviewTime = resp.InterviewTime;
      }
      this._candReg.Comment = resp.Comment;
      this._candReg.OnBoardDescription = resp.OnBoardDescription;

      switch (this._selIntervStg) {
        case 1:
          {
            this._isShortListed = true;
            return;
          }
        case 2:
          {
            this._isInterview = true;
            return;
          }
        case 3:
          {
            this._isOffer = true;
            return;
          }
        case 4:
          {
            this._isOnBoarding = true;
            return;
          }
      }
    } else {
      this._dialog.alert('No Records found..!!', 'OK', 'Message');
    }
  }

  Reset() {
    this._candReg = new CandidateRegistration();
    this._candReg.Recruiter = new Recruiter();
    this._selPract = 0;
    this._selPos = 0;
    this._selIntervStg = 0;
    this._selIntervTyp = 0;
    this._isShortListed = false;
    this._isInterview = false;
    this._isOffer = false;
    this._isOnBoarding = false;
  }

  showAlertPopUp(title, strmessage, buttonTxt) {
    this.displayPopUpMessage(title, strmessage, buttonTxt);
    const pDialog = this._dialog.showCustomDialog({
      component: AlertPopupComponent,
      isModal: true,
      styles: {'width': '300px'},
      clickOutsideToClose: true,
      enterTransitionDuration: 400,
      leaveTransitionDuration: 400
    });
    pDialog.subscribe( (dialogReference: MdlDialogReference) => {
    });
  }

  showConfirmPopUp(title, strmessage, buttonTxt) {
    this.displayPopUpMessage(title, strmessage, buttonTxt);
    const pDialog = this._dialog.showCustomDialog({
      component: ConfirmPopupComponent,
      isModal: true,
      styles: {'width': '350px'},
      clickOutsideToClose: true,
      enterTransitionDuration: 400,
      leaveTransitionDuration: 400,
    });
    pDialog.subscribe( (dialogReference: MdlDialogReference) => {
    });
  }

  displayPopUpMessage(title, strmessage, buttonTxt) {
    const _displaypopup = new DisplayPopup();
    _displaypopup.Title = title;
    _displaypopup.Message = strmessage;
    _displaypopup.ButtonText = buttonTxt;
    sessionStorage.setItem('Display-PopUp', JSON.stringify({
      DisplayPopup: _displaypopup
    }));
  }

  AddFeedBack() {
    this._feedback.CandidateToken = this._candReg.Token;
    this._feedback.FeedBackComment = this._candReg.FeedBackComment;
    this._candRegService.updateFeedbackComment(this._feedback)
    .catch(e => {
      alert(e.message);
      return null;
    })
    .subscribe(resp => this.onAddFB(resp));
  }

  onAddFB(resp) {
    this.editUserDialog.close();
  }

  fileChange(event) {
    const fileList: FileList = event.target.files;
    alert(event.target.files);
    if (fileList.length > 0) {
      const file: File = fileList[0];
      console.log(file);
      this._formData.append('upfile', file, file.name);
      console.log(this._formData['upfile']);
      this._candRegService.fileUpload(this._formData)
        .catch(e => {
          alert(e.message);
          return null;
        })
        .subscribe(resp => this.onRegisterResult(resp));
    }
  }

  onDialogShow(dialogRef: MdlDialogReference) {
  }


  onDialogHide() {
  }
}

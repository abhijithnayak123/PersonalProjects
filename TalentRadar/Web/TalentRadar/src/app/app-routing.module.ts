import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CandidateRegistrationComponent } from './components/candidate-registration/candidate-registration.component';
import { CandidateDashboardComponent } from './components/candidate-user-flow/candidate-dashboard/candidate-dashboard.component';
import { CandidateShortlistComponent } from './components/candidate-user-flow/candidate-shortlist/candidate-shortlist.component';
import { CandidateInterviewComponent } from './components/candidate-user-flow/candidate-interview/candidate-interview.component';
import { CandidateOfferComponent } from './components/candidate-user-flow/candidate-offer/candidate-offer.component';
import { CandidateJoiningDateComponent } from './components/candidate-user-flow/candidate-joining-date/candidate-joining-date.component';
import { CandidateFlowComponent } from './components/candidate-user-flow/candidate-flow/candidate-flow.component';
import { RecruiterDetailsComponent } from './components/recruiter-details/recruiter-details.component';
import { CompanyDetailsComponent } from './components/company-details/company-details.component';
import { CandidateNotificationComponent } from './components/candidate-notification/candidate-notification.component';
import { CandidateConformationComponent } from './components/candidate-user-flow/candidate-conformation/candidate-conformation.component';
import { CandidateLoginComponent } from './components/candidate-user-flow/candidate-login/candidate-login.component';
import { GenericPopupComponent } from './components/common/generic-popup/generic-popup.component';
import { AdminNotificationComponent } from './components/admin-notification/admin-notification.component';
import { AdminModuleComponentComponent } from './components/admin-module-component/admin-module-component.component';
import { AlertPopupComponent } from './components/common/alert-popup/alert-popup.component';
import { ConfirmPopupComponent } from './components/common/confirm-popup/confirm-popup.component';
import { AdminLoginComponent } from './components/admin-login/admin-login.component';

const appRoutes: Routes = [
  { path: '', redirectTo: 'candidatelogin', pathMatch: 'full' },
  { path: 'candidatelogin', component: CandidateLoginComponent },
  { path: 'candidateconformation', component: CandidateConformationComponent },
  {
    path: 'candidateflow', component: CandidateFlowComponent,
    children: [
      { path: '', component: CandidateDashboardComponent },
      { path: 'candidatedashboard', component: CandidateDashboardComponent },
      { path: 'candidateshortlist', component: CandidateShortlistComponent },
      { path: 'candidateinterview', component: CandidateInterviewComponent },
      { path: 'candidateoffer', component: CandidateOfferComponent },
      { path: 'candidatejoiningdate', component: CandidateJoiningDateComponent },
      { path: 'recruiterdetails', component: RecruiterDetailsComponent },
      { path: 'companydetails', component: CompanyDetailsComponent },
      { path: 'candidatenotification', component: CandidateNotificationComponent },
    ]
  },
  { path: 'adminlogin', component: AdminLoginComponent },
  {
    path: 'adminflow', component: AdminModuleComponentComponent,
    children: [
      { path: '', component: AdminNotificationComponent },
      { path: 'adminnotification', component: AdminNotificationComponent },
      { path: 'admincandidateregistration', component: CandidateRegistrationComponent },
    ]
  },
  { path: 'GenericPopup', component: GenericPopupComponent },
  { path: 'AlertPopup', component: AlertPopupComponent },
  { path: 'ConfirmPopup', component: ConfirmPopupComponent },
];

@NgModule({
  imports: [
    RouterModule.forRoot(appRoutes)
  ],
  exports: [
    RouterModule
  ]
})
export class AppRoutingModule { }

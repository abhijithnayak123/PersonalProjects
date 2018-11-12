import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { AppRoutingModule } from './app-routing.module';
import { ReactiveFormsModule } from '@angular/forms';
import { DefaultUrlSerializer, UrlSerializer, UrlTree } from '@angular/router';
import {
  MdlButtonModule, MdlTextFieldModule, MdlIconModule, MdlDialogModule,
  MdlDialogOutletModule, MdlDialogService
} from '@angular-mdl/core';
import { MdlSelectModule } from '@angular-mdl/select';
import { DatePickerModule } from 'angular-io-datepicker';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppComponent } from './app.component';
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
import { MDL } from './directives/MaterialDesignLiteUpgradeElement';
import { GenericPopupComponent } from './components/common/generic-popup/generic-popup.component';
import { AdminNotificationComponent } from './components/admin-notification/admin-notification.component';
import { AdminModuleComponentComponent } from './components/admin-module-component/admin-module-component.component';
import { AlertPopupComponent } from './components/common/alert-popup/alert-popup.component';
import { ConfirmPopupComponent } from './components/common/confirm-popup/confirm-popup.component';
import { AdminLoginComponent } from './components/admin-login/admin-login.component';
import { LowerCaseUrlSerializer } from './urlserializer';


@NgModule({
  declarations: [
    AppComponent,
    CandidateRegistrationComponent,
    CandidateDashboardComponent,
    CandidateShortlistComponent,
    CandidateInterviewComponent,
    CandidateOfferComponent,
    CandidateJoiningDateComponent,
    CandidateFlowComponent,
    RecruiterDetailsComponent,
    CompanyDetailsComponent,
    CandidateNotificationComponent,
    CandidateConformationComponent,
    CandidateLoginComponent,
    MDL,
    GenericPopupComponent,
    AdminNotificationComponent,
    AdminModuleComponentComponent,
    AlertPopupComponent,
    ConfirmPopupComponent,
    AdminLoginComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    HttpModule,
    AppRoutingModule,
    MdlButtonModule,
    MdlTextFieldModule,
    MdlIconModule,
    MdlSelectModule,
    DatePickerModule,
    BrowserAnimationsModule,
    MdlDialogModule,
    MdlDialogOutletModule
  ],
  providers: [MdlDialogService,
    {
      provide: UrlSerializer,
      useClass: LowerCaseUrlSerializer
    }
],
  bootstrap: [AppComponent]
})
export class AppModule { }

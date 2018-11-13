import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { HttpModule } from "@angular/http";
import { HttpClientModule } from "@angular/common/http";
import { ReactiveFormsModule, FormsModule } from "@angular/forms";
import { InputsModule } from "@progress/kendo-angular-inputs";
import { ButtonModule } from "@progress/kendo-angular-buttons";
import { DialogModule } from "@progress/kendo-angular-dialog";
import { UserRouting } from "./user-management.router";

import { LoginComponent } from "./components/login/login.component";
import { LocalStorageService } from "../shared/wrappers/local-storage.service";
import { LoginService } from "./services/login.service";
import { HttpService } from "../shared/wrappers/http.service";
import { DropDownsModule } from "@progress/kendo-angular-dropdowns";
import {SharedModule} from '../shared/shared.module';

@NgModule({
  imports: [
    CommonModule,
    UserRouting,
    ReactiveFormsModule,
    InputsModule,
    FormsModule,
    ButtonModule,
    DialogModule,
    HttpClientModule,
    HttpModule,
    DropDownsModule,  
    SharedModule      
  ],
  declarations: [LoginComponent],
  providers: [LocalStorageService, LoginService, HttpService]
})
export class UserManagementModule {}

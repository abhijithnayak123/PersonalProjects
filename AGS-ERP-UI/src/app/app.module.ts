import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { InputsModule } from "@progress/kendo-angular-inputs";
import { ButtonModule } from "@progress/kendo-angular-buttons";
import { DropDownListModule } from "@progress/kendo-angular-dropdowns";
import { FormsModule } from "@angular/forms";
import { GridModule } from '@progress/kendo-angular-grid';
import { HTTP_INTERCEPTORS } from '@angular/common/http'

import { AppRoutingModule } from "./app.router";
import { UserManagementModule } from "./user-management/user-management.module";
import { AppComponent } from "./app.component";
import { Page404Component } from "./shared/components/page-404/page-404.component";
import { ErrorService } from "./shared/services/error.service";
import { HeaderComponent } from "./common/components/header/header.component";
import { LeftPanelComponent } from "./common/components/left-panel/left-panel.component";
import { MainComponent } from "./main/main.component";
import { BreadcrumbComponent} from "./common/components/breadcrumb/breadcrumb.component";
import {SharedModule} from './shared/shared.module';
import { AgglomerationsComponent } from "./common/components/agglomerations/agglomerations.component";
import { HeaderService } from './common/services/header.service';
import { TreeComponent } from './common/components/tree/tree.component';
import { HeaderMenuComponent } from './common/components/header-menu/header-menu.component';
import { LeftMenuComponent } from './common/components/left-menu/left-menu.component';
import { ModuleService } from "./common/services/module.service";
import { BreadcrumbService } from "./shared/services/breadcrumb.service";
import { ConfirmationService } from './shared/services/confirmation.service';
import { SuccessService } from './shared/services/success.service';
import { AlertService } from "./shared/services/alert.service";
import { CommonService } from "./shared/services/common.service";
import { ToastService } from './shared/services/toast.service';
import { LandingComponent } from './landing/landing.component';
import { LayoutComponent } from './layout/layout.component';
import { TokenInterceptor } from "./shared/interceptors/token.interceptor";

@NgModule({
  declarations: [
    AppComponent,
    Page404Component,
    HeaderComponent,
    LeftPanelComponent,
    MainComponent,
    BreadcrumbComponent,
    AgglomerationsComponent,
    TreeComponent,
    HeaderMenuComponent,
    LeftMenuComponent,
    LandingComponent,
    LayoutComponent
  ],
  imports: [
    InputsModule,
    ButtonModule,
    BrowserModule,
    BrowserAnimationsModule,
    DropDownListModule,
    FormsModule,
    AppRoutingModule,
    UserManagementModule,
    GridModule,
    SharedModule
  ],
  bootstrap: [AppComponent],
  providers: [
    ErrorService, 
    HeaderService, 
    ModuleService, 
    BreadcrumbService, 
    ConfirmationService,
    SuccessService, 
    AlertService, 
    CommonService, 
    ToastService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
    }
  ]
})
export class AppModule {}

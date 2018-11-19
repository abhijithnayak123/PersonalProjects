import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';  
import { ReactiveFormsModule } from "@angular/forms";  
import { AppComponent } from './app.component';
import { ListempComponent } from './listemp/listemp.component';
import { AddempComponent } from './addemp/addemp.component';
import { AppRoutingModule } from './app-routing.module';
import { EmployeeService } from './service/employee.service';
import { AngularFontAwesomeModule } from 'angular-font-awesome';

@NgModule({
  declarations: [
    AppComponent,
    ListempComponent,
    AddempComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,  
    AppRoutingModule,
    ReactiveFormsModule,
    AngularFontAwesomeModule
  ],
  providers: [EmployeeService],
  bootstrap: [AppComponent]
})
export class AppModule { }

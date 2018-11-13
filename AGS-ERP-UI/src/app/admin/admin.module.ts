import { NgModule } from '@angular/core';
import { LayoutModule } from '@progress/kendo-angular-layout';
import { DropDownListModule, ComboBoxModule } from "@progress/kendo-angular-dropdowns";
import { GridModule, PDFModule, ExcelModule } from '@progress/kendo-angular-grid';
import { InputsModule } from '@progress/kendo-angular-inputs';
import { FormsModule } from '@angular/forms';
import { DialogModule } from "@progress/kendo-angular-dialog";
import { DateInputsModule } from '@progress/kendo-angular-dateinputs';
import { PDFExportModule } from '@progress/kendo-angular-pdf-export';

import { CommonModule } from '@angular/common';
import { AdminComponent } from './admin.component';
import { Secure360Component } from './secure-360/secure-360.component';
import { AdminRoutingModule } from './admin.router';
import { SharedModule } from '../shared/shared.module';
import { MasterComponent } from './master/master.component';
import { BomMaintenanceComponent } from './master/components/bom-maintenance/bom-maintenance.component';
import { ItemMaintenanceComponent } from './master/components/item-maintenance/item-maintenance.component';

// import { BrowserModule } from '@angular/platform-browser';
// import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { PopupModule } from '@progress/kendo-angular-popup';
import { VendorMaintenanceComponent } from './master/components/vendor-maintenance/vendor-maintenance.component';
import { SkuMaintenanceComponent } from './master/components/sku-maintenance/sku-maintenance.component';
import { SupplierDataComponent } from './master/components/supplier-data/supplier-data.component';
import { BomComponent } from './master/components/bom/bom.component';
import { VendorContactComponent } from './master/components/vendor-contact/vendor-contact.component';
import { VendorSiteComponent } from './master/components/vendor-site/vendor-site.component';

@NgModule({
  imports: [
    CommonModule,
    AdminRoutingModule,
    SharedModule,
    LayoutModule,
    DropDownListModule,
    ComboBoxModule,
    InputsModule,
    FormsModule,
    DateInputsModule,
    GridModule,
    DialogModule,
    // BrowserModule, 
    // BrowserAnimationsModule, 
    PopupModule
  ],
  declarations: [
    AdminComponent, 
    Secure360Component, 
    MasterComponent, 
    BomMaintenanceComponent, 
    ItemMaintenanceComponent, 
    VendorMaintenanceComponent, 
    SkuMaintenanceComponent, 
    SupplierDataComponent, BomComponent, VendorContactComponent, VendorSiteComponent
  ]
})
export class AdminModule { }

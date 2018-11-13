import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReceivingComponent } from './receiving/receiving.component';
import { RawMaterialRoutingModule } from './raw-material.router';
import { RawMaterialComponent } from "./raw-material.component";
import { LayoutModule } from '@progress/kendo-angular-layout';
import { DropDownListModule, ComboBoxModule } from "@progress/kendo-angular-dropdowns";
import { GridModule, PDFModule, ExcelModule } from '@progress/kendo-angular-grid';
import { InputsModule } from '@progress/kendo-angular-inputs';
import { FormsModule } from '@angular/forms';
import { DialogModule } from "@progress/kendo-angular-dialog";

import { PDFExportModule } from '@progress/kendo-angular-pdf-export';

import { PhysicalAdjustmentsComponent } from './inventory/physical-adjustments/physical-adjustments.component';
import { InventoryComponent } from './inventory/inventory.component';
import { MaintainRollsComponent } from "./inventory/physical-adjustments/components/maintain-rolls/maintain-rolls.component"
import { SharedModule } from '../shared/shared.module';
import { DateInputsModule } from '@progress/kendo-angular-dateinputs';
import { ContainerPoComponent } from './inventory/container-po/container-po.component';
import { ContainerBuildComponent } from './inventory/container-po/components/container-build/container-build.component';
import { PurchasingComponent } from './purchasing/purchasing.component';
import { ReportsComponent } from './reports/reports.component';
import { OrderScreenComponent } from './purchasing/components/order-screen/order-screen';
import { LookupComponent } from './receiving/components/lookup/lookup.component';
import { ContainerTrackingComponent } from './inventory/container-po/components/container-tracking/container-tracking.component';
import { PoTrackingComponent } from './purchasing/components/po-tracking/po-tracking.component';
import { SearchComponent } from './inventory/container-po/components/search/search.component';
import { MaintenanceComponent } from './purchasing/components/maintenance/maintenance.component';
import { FabricReceivingComponent } from './fabric-receiving/fabric-receiving.component';
import { FutureWatchlistComponent } from './fabric-planning/components/future-watchlist/future-watchlist.component';
import { FabricWatchlistComponent } from './fabric-planning/components/fabric-watchlist/fabric-watchlist.component';
import { FabricPlanningReportComponent } from './fabric-planning/components/fabric-planning-report/fabric-planning-report.component';
import { FabricPlanningComponent } from './fabric-planning/fabric-planning.component';
import { PlanningGridComponent } from './fabric-planning/components/planning-grid/planning-grid.component';
import { FabricRecevingService } from './fabric-receiving/fabric-receving.service';
import { ReceiptsComponent } from './receiving/components/receipts/receipts.component';
import { ReceiptDetailsComponent } from './receiving/components/receipt-details/receipt-details.component';

@NgModule({
  imports: [
    CommonModule,
    RawMaterialRoutingModule,
    LayoutModule,
    DropDownListModule,
    GridModule,
    InputsModule,
    FormsModule,
    SharedModule,
    DialogModule,
    DateInputsModule,
    PDFModule,
    ExcelModule,
    ComboBoxModule,
    PDFExportModule
  ],
  declarations: [
    ReceivingComponent,
    ReceiptsComponent,
    RawMaterialComponent,
    LookupComponent,
    FabricReceivingComponent,
    PhysicalAdjustmentsComponent,
    InventoryComponent,
    MaintainRollsComponent,
    FutureWatchlistComponent,
    FabricWatchlistComponent,
    FabricPlanningReportComponent,
    FabricPlanningComponent,
    OrderScreenComponent,
    ContainerTrackingComponent,
    ContainerPoComponent,
    ContainerBuildComponent,
    PoTrackingComponent,
    SearchComponent,
    MaintenanceComponent,
    PlanningGridComponent,
    PurchasingComponent,
    ReportsComponent,
    ReceiptDetailsComponent
  ],
  providers: [FabricRecevingService]
})
export class RawMaterialModule { }

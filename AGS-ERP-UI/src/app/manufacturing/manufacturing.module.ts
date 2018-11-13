import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FgWatchlistComponent } from './planning/components/fg-watchlist/fg-watchlist.component';
import { PlanningComponent } from './planning/planning.component';
import { FgFutureWatchlistComponent } from './planning/components/fg-future-watchlist/fg-future-watchlist.component';
import { PlanningReportComponent } from './planning/components/planning-report/planning-report.component';
import { CapacityComponent } from './planning/components/capacity/capacity.component';
import { ManufacturingRoutingModule } from './manufacturing.router';
import { ManufacturingComponent } from './manufacturing.component';
import { ManufacturingOrdersComponent } from './manufacturing-orders/manufacturing-orders.component';
import { CreateComponent } from './manufacturing-orders/components/create/create.component';
import { SearchComponent } from './manufacturing-orders/components/search/search.component';
import { CutPlanComponent } from './manufacturing-orders/components/cut-plan/cut-plan.component';
import { CareLabelsComponent } from './manufacturing-orders/components/care-labels/care-labels.component';
import { MaintainComponent } from './manufacturing-orders/components/maintain/maintain.component';
import { ReportsComponent } from './reports/reports.component';
import { MasterProductionScheduleComponent } from './reports/components/master-production-schedule/master-production-schedule.component';
import { PullRequestComponent } from './reports/components/pull-request/pull-request.component';
import { LayoutModule } from '@progress/kendo-angular-layout';
import { SharedModule } from '../shared/shared.module';
import { GridModule, PDFModule, ExcelModule } from '@progress/kendo-angular-grid';
import { InputsModule } from '@progress/kendo-angular-inputs';
import { FormsModule } from '@angular/forms';
import { DialogModule } from '@progress/kendo-angular-dialog';
import { DateInputsModule } from '@progress/kendo-angular-dateinputs';
import { ComboBoxModule } from '@progress/kendo-angular-dropdowns/dist/es/combobox.module';
import { PDFExportModule } from '@progress/kendo-angular-pdf-export';
import { DropDownListModule } from '@progress/kendo-angular-dropdowns/dist/es/dropdownlist.module';
import { AllocateComponent } from './fabric-control/components/allocate/allocate.component';
import { DeAllocateComponent } from './fabric-control/components/de-allocate/de-allocate.component';
import { PullComponent } from './fabric-control/components/pull/pull.component';
import { FabricControlService } from './fabric-control/fabric-control.service';
import { FabricControlComponent } from './fabric-control/fabric-control.component';
import { ReturnComponent } from './fabric-control/components/return/return.component';
import { ConsumeComponent } from './fabric-control/components/consume/consume.component';
import { TransposeComponent } from './planning/components/transpose/transpose.component';

@NgModule({
  imports: [
    CommonModule,
    ManufacturingRoutingModule,
    LayoutModule,
    SharedModule,
    DropDownListModule,
    GridModule,
    InputsModule,
    FormsModule,
    DialogModule,
    DateInputsModule,
    PDFModule,
    ExcelModule,
    ComboBoxModule,
    PDFExportModule
  ],
  declarations: [
    FgWatchlistComponent, 
    PlanningComponent, 
    FgFutureWatchlistComponent, 
    PlanningReportComponent, 
    CapacityComponent,
    ManufacturingComponent,
    ManufacturingOrdersComponent,
    CreateComponent,
    SearchComponent,
    CutPlanComponent,
    CareLabelsComponent,
    MaintainComponent,
    ReportsComponent,
    MasterProductionScheduleComponent,
    PullRequestComponent,
    AllocateComponent,
    DeAllocateComponent,
    PullComponent,
    ConsumeComponent,
    ReturnComponent,
    FabricControlComponent,
    TransposeComponent
  ],
  providers: [FabricControlService]
})
export class ManufacturingModule { }

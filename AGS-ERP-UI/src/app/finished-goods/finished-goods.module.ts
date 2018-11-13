import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FinishedGoodsComponent } from './finished-goods.component';
import { InventoryComponent } from './inventory/inventory.component';
import { MaintenanceComponent } from './inventory/components/maintenance/maintenance.component';
import { PhysicalComponent } from './inventory/components/physical/physical.component';
import { PrintLabelsComponent } from './inventory/components/print-labels/print-labels.component';
import { ReceivingComponent } from './receiving/receiving.component';
import { ReceiptsComponent } from './receiving/components/receipts/receipts.component';
import { LookupComponent } from './receiving/components/lookup/lookup.component';
import { OrderFulfilmentComponent } from './order-fulfilment/order-fulfilment.component';
import { ReleaseOrdersComponent } from './order-fulfilment/components/release-orders/release-orders.component';
import { FgAllocationComponent } from './order-fulfilment/components/fg-allocation/fg-allocation.component';
import { PickTicketsComponent } from './order-fulfilment/components/pick-tickets/pick-tickets.component';
import { ReportsComponent } from './reports/reports.component';
import { PrintLabelComponent } from './print-label/print-label.component';
import { FinishedGoodsRoutingModule } from './finished-goods.router';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  imports: [
    CommonModule,
    FinishedGoodsRoutingModule,
    SharedModule
  ],
  declarations: [
    FinishedGoodsComponent, 
    InventoryComponent, 
    MaintenanceComponent, 
    PhysicalComponent, 
    PrintLabelsComponent, 
    ReceivingComponent, 
    ReceiptsComponent, 
    LookupComponent, 
    OrderFulfilmentComponent, 
    ReleaseOrdersComponent, 
    FgAllocationComponent, 
    PickTicketsComponent, 
    ReportsComponent, PrintLabelComponent
  ]
})
export class FinishedGoodsModule { }

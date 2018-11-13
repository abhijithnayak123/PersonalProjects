import { Routes, RouterModule } from "@angular/router";
import { NgModule } from "@angular/core";
import { ReportsComponent } from "./reports/reports.component";
import { FinishedGoodsComponent } from "./finished-goods.component";
import { InventoryComponent } from "./inventory/inventory.component";
import { OrderFulfilmentComponent } from "./order-fulfilment/order-fulfilment.component";
import { ReceivingComponent } from "./receiving/receiving.component";
import { PrintLabelComponent } from "./print-label/print-label.component";

const routes: Routes = [
    {
        path: 'finished-goods',
        component: FinishedGoodsComponent,
        data:{
            breadcrumb: 'Finished Goods'
        },
        children: [
            {
                path: 'inventory', component: InventoryComponent,
                data: {
                    breadcrumb: 'Inventory'
                }
            },
            {
                path: 'order-fulfilment', component: OrderFulfilmentComponent,
                data: {
                    breadcrumb: 'Order Fulfilment'
                }
            },
            {
                path: 'print-labels', component: PrintLabelComponent,
                data: {
                    breadcrumb: 'Print Labels'
                }
            },
            {
                path: 'receiving', component: ReceivingComponent,
                data: {
                    breadcrumb: 'Receiving'
                }
            },
            {
                path: 'reports', component: ReportsComponent,
                data: {
                    breadcrumb: 'Reports'
                }
            }
        ]
    }
]


@NgModule({
    imports: [
      RouterModule.forChild(routes)
    ],
    exports: [RouterModule]
  })
  export class FinishedGoodsRoutingModule { }
import { Routes, RouterModule } from "@angular/router";
import { ManufacturingComponent } from "./manufacturing.component";
import { PlanningComponent } from "./planning/planning.component";
import { NgModule } from "@angular/core";
import { ManufacturingOrdersComponent } from "./manufacturing-orders/manufacturing-orders.component";
import { ReportsComponent } from "./reports/reports.component";
import { FabricControlComponent } from "./fabric-control/fabric-control.component";

const routes: Routes = [
    {
        path: 'manufacturing',
        component: ManufacturingComponent,
        data:{
            breadcrumb: 'Manufacturing'
        },
        children: [
            {
                path: 'planning', component: PlanningComponent,
                data: {
                    breadcrumb: 'Planning'
                }
            },
            {
                path: 'rm-assignment', component: FabricControlComponent,
                data: {
                    breadcrumb: 'RM Assignment'
                }
            },
            {
                path: 'manufacturing-orders', component: ManufacturingOrdersComponent,
                data: {
                    breadcrumb: 'Manufacturing Orders'
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
  export class ManufacturingRoutingModule { }
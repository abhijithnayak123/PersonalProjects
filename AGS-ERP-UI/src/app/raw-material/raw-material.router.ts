import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RawMaterialComponent } from './raw-material.component';
import { PhysicalAdjustmentsComponent } from './inventory/physical-adjustments/physical-adjustments.component';
import { AuthGuard } from '../shared/gaurd/auth-guard.service';
import { InventoryComponent } from './inventory/inventory.component';
import { ContainerPoComponent } from './inventory/container-po/container-po.component';
import { PurchasingComponent } from './purchasing/purchasing.component';
import { ReceivingComponent } from './receiving/receiving.component';
import { ReportsComponent } from './reports/reports.component';
import { FabricPlanningComponent } from './fabric-planning/fabric-planning.component';


const routes: Routes = [
  {
    path: 'raw-material',
    component: RawMaterialComponent,
    data: {
      breadcrumb: 'Raw Material',
    },
    children: [
      {
        path: 'inventory', component: PhysicalAdjustmentsComponent,
        data: {
          breadcrumb: 'Inventory'
        }
      },
      {
        path: 'planning', component: FabricPlanningComponent,
        data: {
          breadcrumb: 'Planning'
        }
      },
      {
        path: 'container', component: ContainerPoComponent,
        data: {
          breadcrumb: 'Container'
        }
      },
      {
        path: 'purchasing', component: PurchasingComponent,
        data: {
          breadcrumb: 'Purchasing'
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
        // children: [
        //   {
        //     path: 'receiving', component: FabricReceivingComponent,
        //     data: {
        //       breadcrumb: 'Fabric Receiving'
        //     }
        //   },
        //   {
        //     path: 'fabric-control', component: FabricControlComponent,
        //     data: {
        //       breadcrumb: 'Fabric Control'
        //     }
        //   },
        //   {
        //     path: 'fabric-planning', component: FabricPlanningComponent,
        //     data: {
        //       breadcrumb: 'Watchlist'
        //     }
        //   },
        //   {
        //     path: 'container-po', component: ContainerPoComponent,
        //     data: {
        //       breadcrumb: 'Container/PO'
        //     }
        //   }
        // ]
      //},

    ]
  }
];

@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class RawMaterialRoutingModule { }

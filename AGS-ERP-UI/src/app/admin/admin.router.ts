import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AdminComponent } from "./admin.component";
import { Secure360Component } from "./secure-360/secure-360.component";
import { MasterComponent } from "./master/master.component";

const routes: Routes = [
    {
        path: 'admin',
        component: AdminComponent,
        data:{
            breadcrumb: 'Admin'
        },
        children: [
            {
                path: 'secure-360',
                component: Secure360Component,
                data:{
                    breadcrumb: 'Secure 360'
                }
            },
            {
                path: 'master',
                component: MasterComponent,
                data:{
                    breadcrumb: 'Master'
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
  export class AdminRoutingModule { }
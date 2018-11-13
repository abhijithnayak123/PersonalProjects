import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";

import { Page404Component } from "./shared/components/page-404/page-404.component";
import { AuthGuard } from "./shared/gaurd/auth-guard.service";
import { AppComponent } from "./app.component";
import { MainComponent } from "./main/main.component";
import { LoginComponent } from "./user-management/components/login/login.component";
import { LandingComponent } from "./landing/landing.component";
import { LayoutComponent } from "./layout/layout.component";

export const routes: Routes = [
  {
    path: "user", loadChildren: "./user-management/user-management.module#UserManagementModule"    
  },
  {
    path: '', redirectTo: 'landing', pathMatch:"full"
  },
  {
    path:'', component: LayoutComponent, children: [
      {
        path: 'landing', component: LandingComponent
      },
      {
        path: '', component: MainComponent, canActivateChild: [AuthGuard], children: [
          {
            path: '', loadChildren:
            "./raw-material/raw-material.module#RawMaterialModule"
          },
          {
            path: '', loadChildren:
            "./manufacturing/manufacturing.module#ManufacturingModule"
          },
          {
            path: '', loadChildren:
            "./finished-goods/finished-goods.module#FinishedGoodsModule"
          },
          {
            path: '', loadChildren:
            "./admin/admin.module#AdminModule"
          }
        ]
      }
    ]
  },
  { path: "**", component: Page404Component }
];
@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: true })],
  exports: [RouterModule],
  providers: [AuthGuard]
})
export class AppRoutingModule { }

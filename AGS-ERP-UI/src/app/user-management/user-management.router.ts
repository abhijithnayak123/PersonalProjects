import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { AuthGuard } from "../shared/gaurd/auth-guard.service";
import { LoginComponent } from "../user-management/components/login/login.component";
const routes: Routes = [
  //{ path: "", component: LoginComponent },
  { path: "login", component: LoginComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: [AuthGuard]
})
export class UserRouting {}

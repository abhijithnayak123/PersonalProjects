import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';  
import { ListempComponent } from './listemp/listemp.component';  
import { AddempComponent } from './addemp/addemp.component';  

export const routes: Routes = [  
  { path: '', component: ListempComponent, pathMatch: 'full' },  
  { path: 'list-emp', component: ListempComponent },  
  { path: 'add-emp', component: AddempComponent }  
]; 

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forRoot(routes) 
  ],
  exports: [RouterModule], 
  declarations: []
})
export class AppRoutingModule { }

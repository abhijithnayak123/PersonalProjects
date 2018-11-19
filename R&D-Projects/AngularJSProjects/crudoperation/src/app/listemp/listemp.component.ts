import { Component, OnInit } from '@angular/core';
import { EmployeeService } from '../service/employee.service';  
import { Employee } from '../model/employee.model';  
import { Router } from "@angular/router";  

@Component({
  selector: 'app-listemp',
  templateUrl: './listemp.component.html',
  styleUrls: ['./listemp.component.css']
})
export class ListempComponent implements OnInit {
   employees: Employee[]; 

  constructor(private empService: EmployeeService,  private router: Router ) { }  

  ngOnInit() {
    // this.empService.getEmployees(); 
    this.empService.getEmployees()  
      .subscribe(data => {  
        this.employees = data;  
      });   
  }

  deleteEmp(employee: Employee): void {  
    this.empService.deleteEmployees(employee.EmpId)  
      .subscribe(data => {  
        if(data)
        {
          this.employees = this.employees.filter(u => u !== employee);  
        }
      })  
  }  
  editEmp(employee: Employee): void {  
    debugger;
    localStorage.removeItem('editEmpId');  
    localStorage.setItem('editEmpId', employee.EmpId.toString());  
    this.router.navigate(['add-emp']);  
  }  
}

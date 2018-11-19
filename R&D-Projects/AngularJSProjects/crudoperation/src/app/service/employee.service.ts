import { Injectable } from '@angular/core';  
import { HttpClient } from '@angular/common/http';  
import { Employee } from '../model/employee.model';  
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({  
  providedIn: 'root'  
})  
export class EmployeeService {  
  employees: Employee[];
  constructor(private http: HttpClient) { }  
  baseUrl: string = 'http://localhost:53546/api/Employee';  
  
getEmployees() {  
    return this.http.get<Employee[]>(this.baseUrl);  
  }  

//   getEmployees() {  
//     return this.http.get<Employee[]>(this.baseUrl)
//         .pipe(
//             map((data: Employee[]) => {
//                 this.employees = <Employee[]>data;
//                 this.employees.EmployeeId = res.json().Total;
//             }))
//     //     .subscribe((data: Employee[]) => {  
//     //     debugger;
//     //     this.employees = data;  
//     //   });  
//   }  
  deleteEmployees(id: number) {  
    return this.http.delete<Employee[]>(this.baseUrl + id);  
  }  
  createUser(employee: Employee) {  
    return this.http.post(this.baseUrl, employee);  
  }  
  getEmployeeById(id: number) {  
    return this.http.get<Employee>(this.baseUrl + '/' + id);  
  }  
  updateEmployee(employee: Employee) {  
    return this.http.put(this.baseUrl + '/' + employee.EmpId, employee);  
  }  
}  
import { Component, OnInit } from '@angular/core';  
import { FormBuilder, FormGroup, Validators } from "@angular/forms";  
import { EmployeeService } from '../service/employee.service';  
import { Router } from "@angular/router";  

@Component({
  selector: 'app-addemp',
  templateUrl: './addemp.component.html',
  styleUrls: ['./addemp.component.css']
})
export class AddempComponent implements OnInit {

   empformlabel: string = 'Add Employee';  
  empformbtn: string = 'Save';  
  constructor(private formBuilder: FormBuilder, private router: Router, private empService: EmployeeService) {  
  }  
  
  addForm: FormGroup;  
  btnvisibility: boolean = true;  
  ngOnInit() {  
    this.addForm = this.formBuilder.group({  
      id: [],  
      employee_name: ['', Validators.required],  
      // employee_salary: ['', [Validators.required, Validators.maxLength(9)]],  
      // employee_age: ['', [Validators.required, Validators.maxLength(3)]]  
    });  
  
    let empid = localStorage.getItem('editEmpId');  
    if (+empid > 0) {  
      this.empService.getEmployeeById(+empid).subscribe(data => {  
        this.addForm.patchValue(data);  
      })  
      this.btnvisibility = false;  
      this.empformlabel = 'Edit Employee';  
      this.empformbtn = 'Update';  
    }  
  }  
  onSubmit() {  
    console.log('Create fire');  
    this.empService.createUser(this.addForm.value)  
      .subscribe(data => {  
        this.router.navigate(['list-emp']);  
      },  
      error => {  
        alert(error);  
      });  
  }  
  onUpdate() {  
    console.log('Update fire');  
    this.empService.updateEmployee(this.addForm.value).subscribe(data => {  
      this.router.navigate(['list-emp']);  
    },  
      error => {  
        alert(error);  
      });  
  }  

}

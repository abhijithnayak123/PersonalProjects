import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';

@Component({
  selector: 'app-admin-module-component',
  templateUrl: './admin-module-component.component.html',
  styleUrls: ['./admin-module-component.component.css']
})
export class AdminModuleComponentComponent implements OnInit {

  constructor(private _router: Router) { }

  ngOnInit() {
  }

  Registration() {
    sessionStorage.removeItem('Admin_Candidate_Search');
    this._router.navigate(['adminflow/admincandidateregistration']);
  }

  AdminNotification() {
    this._router.navigate(['adminflow/adminnotification']);
  }

}

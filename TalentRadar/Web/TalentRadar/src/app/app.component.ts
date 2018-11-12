import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent  {

  constructor(private _router: Router) { }

  OnInit() {
    debugger;
    const _userName = JSON.parse(sessionStorage.getItem('userName'));
    const _context = JSON.parse(sessionStorage.getItem('Candidate_Token'));

    if (_context === null && window.location.pathname.indexOf('admin') === -1) {
      this._router.navigate(['candidatelogin']);
    } else if (_userName === null) {
      this._router.navigate(['adminlogin']);
    }
  }
}

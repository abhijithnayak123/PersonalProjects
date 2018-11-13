import { Component, OnInit } from '@angular/core';
import { LocalStorageService } from '../shared/wrappers/local-storage.service';
import { BreadcrumbService } from '../shared/services/breadcrumb.service';
import { Location } from '@angular/common';
@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit {

  constructor(
    private _localStore: LocalStorageService,
    private _breadcrumbService: BreadcrumbService,
    private _location: Location,
  ) {}

  MainDynamicClass: string = 'maximizedMain';
  ngOnInit() {
  }

  onMenuExpanded(isExpanded: boolean) {
    if(isExpanded) {  
      this.MainDynamicClass = 'maximizedMain'
    }
    else {
      this.MainDynamicClass = 'restoredMain'
    }
  }
 
}

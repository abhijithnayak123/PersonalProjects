import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute, ActivatedRouteSnapshot, NavigationEnd, RoutesRecognized, Params, PRIMARY_OUTLET, UrlSegment } from "@angular/router";
import "rxjs/add/operator/filter";
import { LocalStorageService } from "../../../shared/wrappers/local-storage.service";
import { BreadcrumbService } from "../../../shared/services/breadcrumb.service";
import { IBreadcrumb } from "../../models/breadcrumb.model";
import { Subscription } from 'rxjs/Subscription';
import { HeaderService } from "../../services/header.service";

@Component({
  selector: "breadcrumb",
  templateUrl: "./breadcrumb.component.html"
})


export class BreadcrumbComponent implements OnInit,OnDestroy {
    subscription : Subscription;
    public breadcrumbs: IBreadcrumb[];

  constructor(
    private localStorageService: LocalStorageService,
    private breadcrumbService : BreadcrumbService,
    private headerService: HeaderService
  ) {
    this.breadcrumbs = [];

  }
  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  ngOnInit() {
    let breadCrumd = JSON.parse(this.localStorageService.get('breadcrumbs'));
    if(breadCrumd){
      this.breadcrumbs = breadCrumd;
    }
    this.subscription = this.breadcrumbService.getBreadcrumbs().subscribe(data=>{
      if(data){
        this.breadcrumbs = data;
      }
    })
  }
 
  home(){
    this.headerService.notifyOther({ key: 'moduleId', value: 0 })
  }

}
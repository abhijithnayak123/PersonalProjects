import { Component, OnInit } from "@angular/core";
import { Title } from "@angular/platform-browser";
import { LocalStorageService } from "./shared/wrappers/local-storage.service";
import { Router, NavigationEnd, ActivatedRoute, PRIMARY_OUTLET } from "@angular/router";
import { UserPrevilagesModel } from "./user-management/models/user-previlages.model";
import { IBreadcrumb } from "./common/models/breadcrumb.model";
import { BreadcrumbService } from "./shared/services/breadcrumb.service";
import { SessionStorageService } from "./shared/wrappers/session-storage.service";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.css"]
})
export class AppComponent implements OnInit {
  title = "app";
  public breadcrumbs: IBreadcrumb[];

  constructor(
    private _localStorageService: LocalStorageService,
    private _breadcrumbService: BreadcrumbService,
    private _router: Router,
    private activatedRoute: ActivatedRoute,
    private _title: Title,
    private _sessionStorage: SessionStorageService
  ){    
    if (this.checkIfUserLoggedIn()) {
    }else{
      this._router.navigate(['/user/login']);
    }
  }

  ngOnInit(): void {

    this._sessionStorage.removeAll(['future_watchlist_load','future_watchlist_vendors','future_watchlist_vendor_selected',
  'future_watchlist_selected_item','fuWlGridData','future_watchlist_itemList',
'fabric_watchlist_load','fabric_watchlist_vendors','fabric_watchlist_selected_vendor','fabric_watchlist_selected_item','fbWlGridData','fabric_watchlist_itemList'])
    //subscribe to the NavigationEnd event
    this._router.events.filter(event => event instanceof NavigationEnd).subscribe(event => {
      this._title.setTitle('AMCS2');
      //set breadcrumbs
      let root: ActivatedRoute = this.activatedRoute.root;
      this.breadcrumbs = this.getBreadcrumbs(root);
      
      if(this.breadcrumbs.length>0){
        this.breadcrumbs[this.breadcrumbs.length-1].lastElement = true;
        this._title.setTitle(this.breadcrumbs[this.breadcrumbs.length-1].label);
      }
      this._localStorageService.add('breadcrumbs', JSON.stringify(this.breadcrumbs));
      if(this.breadcrumbs && this.breadcrumbs.length > 0){
        this._breadcrumbService.getLocalBreadcrumb(this.breadcrumbs);
      }
    });
  }

  
  private getBreadcrumbs(route: ActivatedRoute, ul: string="/", breadcrumbs: IBreadcrumb[]=[]): IBreadcrumb[] {
    const ROUTE_DATA_BREADCRUMB: string = "breadcrumb";

    //get the child routes
    let children: ActivatedRoute[] = route.children;

    //return if there are no more children
    if (children.length === 0) {
      return breadcrumbs;
    }

    //iterate over each children
    for (let child of children) {
      //verify primary route
      if (child.outlet !== PRIMARY_OUTLET) {
        continue;
      }

      //verify the custom data property "breadcrumb" is specified on the route
      if (!child.snapshot.data.hasOwnProperty(ROUTE_DATA_BREADCRUMB)) {
        return this.getBreadcrumbs(child, ul, breadcrumbs);
      }

      //get the route's URL segment
      let routeURL: string = child.snapshot.url.map(segment => segment.path).join("/");

      //append route URL to URL
      ul += `/${routeURL}`;
     
      //add breadcrumb
      let breadcrumb: IBreadcrumb = {
        label: child.snapshot.data[ROUTE_DATA_BREADCRUMB],
        params: child.snapshot.params,
        url: ul,
        lastElement : false
      };
      breadcrumbs.push(breadcrumb);
      //recursive
      return this.getBreadcrumbs(child, ul, breadcrumbs);
    }
    
    //we should never get here, but just in case
    return breadcrumbs;
  }

  checkIfUserLoggedIn(): boolean {
    const result = this._localStorageService.get('ags_erp_user_previlage');

    if (result) {
      let previlage: UserPrevilagesModel = new UserPrevilagesModel();
      previlage = JSON.parse(result);
      return true;
    } else {
      return false;
    }
  }
}

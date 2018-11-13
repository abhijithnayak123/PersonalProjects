import { BaseModel } from "../../shared/models/base.model";
import { Router, ActivatedRoute, ActivatedRouteSnapshot, NavigationEnd, RoutesRecognized, Params, PRIMARY_OUTLET, UrlSegment } from "@angular/router";


export class IBreadcrumb extends BaseModel{
    constructor()
    {
        super();
    }
    label: string;
    params?: Params;
    url: string;
    lastElement : boolean;

}
import { BaseModel } from "../../shared/models/base.model";

export class Language extends BaseModel {
    constructor(public code:string , public name:string){
        super();
    }
        
   
}
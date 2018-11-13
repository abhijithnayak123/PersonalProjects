import {BaseModel} from './base.model';

export class ErrorModel extends BaseModel{
    constructor(){
        super();
    }    
    ErrorCode:string;
    ErrorDescription:string;
    ErrorData:string;
}
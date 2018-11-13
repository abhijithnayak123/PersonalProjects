import {BaseModel} from '../../shared/models/base.model';

export class LoginModel extends BaseModel{
    username: string;
    password: string;
    rememberMe: boolean;
}
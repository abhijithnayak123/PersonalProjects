import {BaseModel} from './base.model';

export class UserModel extends BaseModel{
    id: number;
    name: string;
    username: string;
    email: string
}
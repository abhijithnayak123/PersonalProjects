import { BaseModel } from "../../../shared/models/base.model";

export class Site extends BaseModel {
    public Id: number;
    public Code: string;
    public Name: string;
}
import { BaseModel } from "../../../shared/models/base.model";

export class FiberContent extends BaseModel{
constructor(
    public Id:number,
    public Content:string
){
    super()
}
}
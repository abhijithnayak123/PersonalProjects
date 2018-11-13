import { BaseModel } from "../../../../shared/models/base.model";

export class FarbricDetailsModel extends BaseModel{
    constructor(
        public Id:number,
        public ItemCode: string,
        public ItemName: string,
        public ColorId:string,
        public Color:string,
        public ColorCode:string,
        public Width:number ,
        public FiberContent:string,
    ){
        super();
    }
}

import { BaseModel } from "../../../../shared/models/base.model";

export class ColorDetails extends BaseModel{
    constructor(
        public ColorId: string,
        public Color: string,
        public ColorCode: string,
        public ColorInfo : string
    ){
        super();
    }
}
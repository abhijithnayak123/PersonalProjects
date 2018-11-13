import { BaseModel } from "../../shared/models/base.model";

export class ModuleModel extends BaseModel{
    constructor(
        public Id: number,
        public Name: string,
        public Path: string,
        public ParentId: number
    ) {
        super();
    }
}
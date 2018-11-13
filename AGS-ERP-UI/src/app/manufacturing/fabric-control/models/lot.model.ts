import { BaseModel } from "../../../shared/models/base.model";

export class Lot extends BaseModel {
    constructor(
        public LotNumber: string,
        public Available: number,
        public BOMYards: number,
        public MarkerYards: number,
        public Allocated: number,
    ){
        super();
    }
}

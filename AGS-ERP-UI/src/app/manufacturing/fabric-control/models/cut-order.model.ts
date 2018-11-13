import { BaseModel } from "../../../shared/models/base.model";

export class CutOrder extends BaseModel {
    constructor(
        public Id: number,
        public CutHbrId: number,
        public CutOrder: string,
	    public SewPlant: string,
	    public CutPlant: string,
	    public Style: string,
	    public StyleDesc: string,
	    public Color: string,
	    public CutQuantity: number,
	    public FiscalWeek: string,
	    public CutDate: string,
    ){
        super();
    }
}


import { BaseModel } from '../../../../shared/models/base.model';

export class ContainerType extends BaseModel {
    constructor(
        public ContainerTypeId: number,
        public CantainerTypeCode: string,
        public ContainerTypeDesc: string,
        public RecoPDS: number,
        public MaxPDS: number
        
    ) {
        super();
    }
}

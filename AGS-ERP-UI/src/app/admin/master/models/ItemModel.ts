import { BaseModel } from '../../../shared/models/base.model';
import { Item } from './Item';
import { ItemStatus } from './ItemStatus';
import { ItemType } from './ItemType';
import { ProductCategory } from './ProductCategory';
import { UoM } from './UoM';
import { ItemColor } from './ItemColor';
import { GreigeItem } from './GreigeItem';

export class ItemModel extends BaseModel {
    constructor() {
        super();
    }
        public Id:number;
        public Item: Item;
        public Code: string;
        public Description : string;
        public StatusId: number;
        public Status: ItemStatus;
        public LastStatusId: number;
        public FabricItem: boolean;
        public FiberContent: string;
        public TypeId: number;
        public Type: ItemType;
        public ProductCategory: ProductCategory;
        public ProductCategoryId: number;
        public CostUOM: UoM;
        public StockUOM: UoM;
        public CostUOMId: number;
        public StockUOMId: number;
        public PackedAsRoll: boolean;
        public StdCost:number;
        public ProjCost: number;
        public Weight: number;
        public Color: ItemColor;
        public ColorId: number;
        public FlameResistant: boolean;
        public Finish: string;
        public CuttableWidth: number;
        public GreigeGood: string;
        public GreigeItem: GreigeItem;
        public GreigeItemId: number;
        public Pattern: string;
        public LastStatusChange: Date;
        public Comments: string;
        public CreatedON: Date;
        public CreatedBy: string;
        public LastModifiedON: Date;
        public LastModifiedBy: string; 
        
        public ItemList: Array<Item>;
        public StatusList: Array<ItemStatus>;
        public TypeList : Array<ItemType>;
        public ColorList: Array<ItemColor>;
        public UOMList: Array<UoM>;
        public PCList: Array<ProductCategory>;
        public GIList: Array<GreigeItem>;

        public isAddMode: boolean;
        public isEditMode: boolean;
}

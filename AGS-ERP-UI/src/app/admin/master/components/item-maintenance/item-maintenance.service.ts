import { Injectable } from '@angular/core';
import { HttpService } from '../../../../shared/wrappers/http.service';
import {Item } from '../../../master/models/Item';
import {ItemType } from '../../../master/models/ItemType';
import {ItemStatus } from '../../../master/models/ItemStatus';
import {ItemColor } from '../../../master/models/ItemColor';
import {Supplier } from '../../../master/models/Supplier';
import { ItemDetail } from '../../models/ItemDetail';
import { UoM } from '../../models/UoM';
import { ProductCategory } from '../../models/ProductCategory';
import { GreigeItem } from '../../models/GreigeItem';
import { COO } from '../../models/COO';
import { SupplierVendor } from '../../models/SupplilerVendor';
import { Response } from '@angular/http/src/static_response';
import { Bom } from '../../models/Bom';
import { ItemModel } from '../../models/ItemModel';

@Injectable()
export class ItemMaintenanceService {

constructor(private _httpService: HttpService) { }

getItemDetails(id: number) {
    return this._httpService.get<ItemModel>('item-management/items/'+id)
      .map(response => {
        return response;
      });
}

getItems() {
    return this._httpService.get<Item[]>('item-management/items')
      .map(response => {
        return response;
      });
}

getItemTypes() {
    return this._httpService.get<ItemType[]>('item-management/item-types')
      .map(response => {
        return response;
      });
}


getItemStatus() {
    return this._httpService.get<ItemStatus[]>('item-management/item-status')
      .map(response => {
        return response;
      });
}

getItemColors() {
    return this._httpService.get<ItemColor[]>('item-management/colors')
      .map(response => {
        return response;
      });
}

getUOM(){
  return this._httpService.get<UoM[]>('item-management/uoms')
  .map(response => {
    return response;
  });
}



getCOOs(){
  return this._httpService.get<COO[]>('item-management/coos')
  .map(response => {
    return response;
  });
}

getSupplierVendors() {
  return this._httpService.get<SupplierVendor[]>('item-management/supplier-vendors')
  .map(response => {
    return response;
  });
}

getProductCategory(){
  return this._httpService.get<ProductCategory[]>('item-management/product-category')
  .map(response => {
    return response;
  });
}

getGreigeItems(){
  return this._httpService.get<GreigeItem[]>('item-management/greige-items')
  .map(response => {
    return response;
  });
}

getSupplerDataByItem(selectedItemId: number) {
    return this._httpService.get<Supplier[]>('item-management/supplier-data/' + selectedItemId)
      .map(response => {
        return response;
      });
}


getBomDataByItem(selectedItemId: number){
  return this._httpService.get<Bom[]>('item-management/boms/' + selectedItemId)
  .map(response => {
    return response;
  });
}

saveItem(model: ItemModel){
  return this._httpService.post('item-management/items', model)
      .map(response => {
        return response;
  });
}

deleteItem(model: ItemModel){
  return this._httpService.post('item-management/delete-items', model)
      .map(response => {
        return response;
  });
}

deleteSuppliers(item: Array<Supplier>){
  return this._httpService.post('item-management/delete-supplier-data', item)
      .map(response => {
        return response;
  });
}

saveSuppliers(item: Array<Supplier>){
  return this._httpService.post('item-management/supplier-data', item)
      .map(response => {
        return response;
  });
}

}

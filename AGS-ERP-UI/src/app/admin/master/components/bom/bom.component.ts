import { Component, OnInit, Input } from '@angular/core';
import { ItemMaintenanceService } from '../item-maintenance/item-maintenance.service';
import { HttpErrorResponse } from "@angular/common/http";
import { ConfirmationService } from '../../../../shared/services/confirmation.service';
import { ToastService } from '../../../../shared/services/toast.service';
import { ErrorService } from '../../../../shared/services/error.service';
import {Bom}  from '../../models/Bom';
import { GridDataResult } from "@progress/kendo-angular-grid/dist/es/data/data.collection";
import { SortDescriptor, orderBy } from '@progress/kendo-data-query';

@Component({
  selector: 'app-bom',
  templateUrl: './bom.component.html',
  styleUrls: ['./bom.component.css'],
  providers: [ItemMaintenanceService]
})
export class BomComponent implements OnInit {

  BomsList: Array<Bom>=[];
  @Input() ItemId: number;
  filterGridData: GridDataResult = {data: [],total : 0}
  filterSort: SortDescriptor[] = [];

  constructor(
    private _errorService: ErrorService,
    private _itemMaintenance: ItemMaintenanceService,
    private _toastService: ToastService,
    private _confirmationService: ConfirmationService
  ) { }

  ngOnInit() {
    this.getItemBoms(this.ItemId);
  }

  getItemBoms(ItemId){
    this._itemMaintenance.getBomDataByItem(ItemId).subscribe(
      data =>{
        this.filterGridData.data = this.BomsList = data;
      },
      (err: HttpErrorResponse) => {
        this._errorService.error(err);
      });
  }

  filterSortChange(sort: SortDescriptor[]): void {
    this.filterSort = sort;
    this.sortFilterDataRolls();
  }

  sortFilterDataRolls() {
    this.filterGridData = {
      data: orderBy(this.filterGridData.data, this.filterSort),
      total: this.filterGridData.data.length
    };
  }
}

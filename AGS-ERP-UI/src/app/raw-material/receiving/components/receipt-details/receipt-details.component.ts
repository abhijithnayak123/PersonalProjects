import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { GridComponent, GridDataResult } from '@progress/kendo-angular-grid';
import { ReceivingService } from '../../receiving.service';
import { ErrorService } from '../../../../shared/services/error.service';
import { CommonService } from '../../../../shared/services/common.service';
import { PickupDetails } from '../../models/pickup-details.model';
import { AramarkItem } from '../../../fabric-planning/models/aramark-item';
import { LookUpGridDetails } from '../../models/lookup-details';
import { SortDescriptor } from '@progress/kendo-data-query/dist/es/sort-descriptor';
import { orderBy } from '@progress/kendo-data-query/dist/es/array.operators';
import { LookHeaderDetails } from '../../models/look-header-details';
import { PONumber } from '../../../purchasing/models/poNumber';
import { HttpErrorResponse } from '@angular/common/http';
import { ReceiptLookUpDetails } from '../../models/receipt-look-up-details';
@Component({
  selector: 'app-receipt-details',
  templateUrl: './receipt-details.component.html',
  styleUrls: ['./receipt-details.component.css'],
  providers: [ReceivingService]
})
export class ReceiptDetailsComponent implements OnInit, OnDestroy {
  @Input() ReceiptNumber: string;
  @Input() PONumber: string;
  receivedYards: number;
  yards: number;
  gridView: GridDataResult = { data: [], total: 0 };
  show: boolean;
  ReceiptHeader: LookHeaderDetails;
  ReceiptsGridData: LookUpGridDetails[];
  sort: SortDescriptor[] = [];
  constructor(
    private _receivingService: ReceivingService,
    private _errorService: ErrorService,
    private _commonService: CommonService
  ) { }

  ngOnInit() {
    this.show = false;
    this.gridView = { data: [], total: 0 };
    this.ReceiptsGridData = [];
    this.ReceiptHeader = new LookHeaderDetails(0, '', 0, '', 0, 0, '', '', '', 0, '',
      '', new Date(), null, null, '', '', '', 0, 0, 0, 0, '', '', new Date());
    this.getReceiptDetails();
    this.receivedYards = this.yards = 0;
  }

  getReceiptDetails() {
    // region Grid
    this.show = true;
    this._receivingService.GetReceiptDetailsInfo(this.ReceiptNumber, this.PONumber).subscribe(
      data => {
        if (data) {
          let details = new ReceiptLookUpDetails();
          details = data;
          this.ReceiptHeader = details.LoopUpHeader;
          this.ReceiptHeader.Comments = details.Details[0].Comments;
          this.ReceiptsGridData = details.Details;
          this.ReceiptsGridData.forEach(x => {
            x.ReceivedYards = (x.Quantity - x.Defective);
            this.receivedYards += x.ReceivedYards;
            this.yards += x.Quantity;
          });
          this.loadGrid();
        }
        this.show = false;
      },
      (err: HttpErrorResponse) => {
        this.show = false;
        this._errorService.error(err);
      }
    );
    // endregion
  }
  onExportPDF(grid: GridComponent) {
    grid.saveAsPDF();
  }

  onExportExcel(grid: GridComponent) {
    grid.saveAsExcel();
  }
  sortChange(sort: SortDescriptor[]): void {
    this.sort = sort;
    this.loadGrid();
  }
  loadGrid() {
    this.gridView = {
      data: orderBy(this.ReceiptsGridData, this.sort),
      total: this.ReceiptsGridData.length
    };

  }

  ngOnDestroy() {
    this._commonService.Notify({
      key: 'destroyLookUpDetail',
      value: { 'ReceiptNumber': '' }
    });
  }
}

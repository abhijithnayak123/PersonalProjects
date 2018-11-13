import { Injectable } from '@angular/core';
import { HttpService } from '../../shared/wrappers/http.service';
import { LookUpGridDetails } from './models/lookup-details';
import { ContainerModel } from './models/container.model';
import { PickupModel } from './models/pick-up.model';
import { PickupDetails } from './models/pickup-details.model';
import { POLineModel } from './models/po-line.model';
import { POModel } from './models/po.model';
import { PODetailModel } from './models/po-detail.model';
import { LookHeaderDetails } from './models/look-header-details';
import { ReceiptLookUpDetails } from './models/receipt-look-up-details';

@Injectable()
export class ReceivingService {

  constructor(
    private _httpService: HttpService
  ) { }


  GetReceiptsContainers() {
    return this._httpService.get<ContainerModel[]>('receiving/containers')
      .map(response => {
        return response;
      });
  }

  GetReceiptsPickups() {
    return this._httpService.get<PickupDetails[]>('receiving/pickups')
      .map(response => {
        return response;
      });
  }

  getReceiptsPODetails(containerId: number, containerStopId: number) {
    return this._httpService.get<PODetailModel[]>('receiving/pos/' + containerId + '/' + containerStopId)
      .map(response => {
        return response;
      });
  }


  DeleteReceiptsRollCases(model: { ReceiptHeader: PickupDetails, ReceiptDetails: POLineModel[] }) {
    return this._httpService.post('receiving/deletereceipts', model)
      .map(response => {
        return response;
      });
  }

  SaveReceiptsRollCases(model: { ReceiptHeader: PickupDetails, ReceiptDetails: POLineModel[] }) {
    return this._httpService.post('receiving/savereceipts', model)
      .map(response => {
        return response;
      });
  }

  PostReceiptsRollCases(model: { ReceiptHeader: PickupDetails, ReceiptDetails: POLineModel[] }) {
    return this._httpService.post('receiving/postreceipts', model)
      .map(response => {
        return response;
      });
  }

  // Lookup screen services
  GetLookUpContainers() {
    return this._httpService.get<LookHeaderDetails[]>('receiving/lookup/header-details')
      .map(response => {
        return response;
      });
  }

  GetGridDetails(containerId, pickUpNbr) {
    return this._httpService.get<LookUpGridDetails[]>('receiving/receipts/' + containerId + '/' + pickUpNbr)
      .map(response => {
        return response;
      });
  }

  GetLookupGridDetails(searchCriteria) {
    return this._httpService.post<LookUpGridDetails[]>('receiving/lookup', searchCriteria)
      .map(response => {
        return response;
      });
  }

  GetReceiptDetailsInfo(receiptNumber, poNumber) {
    return this._httpService.get<ReceiptLookUpDetails>('receiving/receipt-detail/' + receiptNumber + '/' + poNumber)
      .map(response => {
        return response;
      });
  }
}

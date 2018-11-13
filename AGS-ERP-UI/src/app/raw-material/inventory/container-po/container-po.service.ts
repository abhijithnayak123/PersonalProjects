import { Injectable } from '@angular/core';
import { HttpService } from '../../../shared/wrappers/http.service';
import { Vendor } from './models/vendor';
import { PickupLocation } from './models/pickup-location';
import { PurchaseOrder } from './models/purchase-order';
import { ContainerBuildDetail } from './models/container-build-detail';
import { Build } from './models/build';
import { FabricContainer } from './models/fabric-container';
import { Carrier } from './models/carrier';
import { ContainerType } from './models/containertype';
import { ContainertrackingHeaderdetails } from './models/containertracking-headerdetails';
import { ContainerDetails } from './models/container-details';
import { Container } from './models/container';
import { isNull } from 'util';
import * as moment from 'moment';
import { AttachmentType } from './models/attachment-type';
import { Note } from './models/note';
import { AramarkItem } from "./models/aramarkItem";
import { TrackingAttachment } from './models/tracking-attachment';
import { ContainerMetaData} from './models/container-metadata';
import { DistributionCenter } from "./models/distributionCenter";


@Injectable()
export class ContainerPoService {

  constructor(private _httpService: HttpService) { }

  containerBuildGetVendors() {
    return this._httpService.get<Vendor[]>('container-build/vendors')
      .map(response => {
        return response;
      });
  }

  containerBuildGetPickupLocations(selectedVendor: Vendor) {
    return this._httpService.get<PickupLocation[]>('container-build/pickup-locations/' + selectedVendor.VendorId)
      .map(response => {
        return response;
      });
  }

  containerBuildGetPOs(selectedPickupLocn: PickupLocation, selectedReqDate) {
    let a = selectedPickupLocn.PickupLocnCode.trim();
    return this._httpService.get<PurchaseOrder[]>('container-build/pos/' +
      selectedPickupLocn.VendorSiteId + '/' + a + '/' + selectedReqDate)
      .map(response => {
        return response;
      });
  }

  containerBuildGetItems(selectedPo: PurchaseOrder) {
    return this._httpService.get<AramarkItem[]>('container-build/items/' +
      selectedPo.POHdrId)
      .map(response => {
        return response;
      });
  }

  containerBuildGetPOLines(vendorSiteId, requestedShipDate, itemVendorId?, poHdrId?) {
    return this._httpService.get<ContainerBuildDetail[]>('container-build/po-lines/' +
      vendorSiteId + '/' + requestedShipDate + '/' + itemVendorId + '/' + poHdrId)
      .map(response => {
        return response;
      });
  }

  containerBuildGetBuildData(batchStatus: string) {
    return this._httpService.get<Build[]>('container-build/builds/' + batchStatus)
      .map(response => {
        return response;
      });
  }

  containerBuildGetFabricContainers(buildNbr: number) {
    return this._httpService.get<FabricContainer[]>('container-build/fabric-containers/' + buildNbr)
      .map(response => {
        return response;
      });
  }

  containerBuildGetCarrierData() {
    return this._httpService.get<Carrier[]>('container-build/carriers')
      .map(response => {
        return response;
      });
  }

  containerBuildGetContainerTypes() {
    return this._httpService.get<ContainerType[]>('container-build/conatiner-types')
      .map(response => {
        return response;
      });
  }

  containerBuildCreateContainers(pos: Array<ContainerBuildDetail>) {
    return this._httpService.post('container-build/approve', pos)
      .map(response => {
        return response;
      });
  }

  containerBuildHoldContainers(pos: Array<ContainerBuildDetail>) {
    return this._httpService.post('container-build/hold', pos)
      .map(response => {
        return response;
      });
  }

  containerBuildGetContainerTypeOnContTypeId(containerTypeId: number) {
    return this._httpService.get<ContainerType>('')
      .map(response => {
        return response;
      })
  }

  containerBuildGetCurrentSelectionData(selectedBuild: Build, fabricContainer: FabricContainer) {
    let fabContainerId = 0;
    if (fabricContainer) {
      fabContainerId = fabricContainer.ContainerId;
    }
    return this._httpService.get<ContainerBuildDetail[]>('container-build/currentselection/'
      + selectedBuild.BuildNbr + '/' + fabContainerId)
      .map(response => {
        return response;
      });
  }
  containerBuildDeletePoLine(pos: Array<ContainerBuildDetail>){
    return this._httpService.post('container-build/delete', pos)
      .map(response => {
        return response;
      });
  }

  containerBuildGetDCs(){
    let dc1 = new DistributionCenter();
    let dcs : Array<DistributionCenter> = [];
    dc1.Id = 1;
    dc1.Key = 'Monc';
    dc1.Name = 'Monclova Raw Material Warehouse'
    dcs.push(dc1);
    return dcs;
  }

  // containerBuildGetContainerTypeOnId(containerTypeId : number){
  //   return this._httpService.get<ContainerType>('container-build/containerById/'+ containerTypeId)
  //   .map(response => {
  //     return response;
  //   })
  //  }

  // containerBuildGetCarrierOnId(carrierId : number){
  //   return this._httpService.get<Carrier>('container-build/carrierById/'+ carrierId)
  //   .map(response => {
  //     return response;
  //   })
  // }

  // container Tracking API Area.

  containerTrackingGetVendors() {
    return this._httpService.get<Vendor[]>('containertracking/Vendors')
      .map(response => {
        return response;
      });
  }
  containerTrackingGetContainers(vendorId, pickUpDate) {
    return this._httpService.get<Container[]>('containertracking/Containers/' + vendorId + '/' + pickUpDate)
      .map(response => {
        return response;
      });
  }

  containerTrackingGetHeaderDetails(containerId) {
    return this._httpService.get<ContainertrackingHeaderdetails>('containertracking/Header/' + containerId)
      .map(response => {
        return response;
      });
  }

  containerTrackingGetContainerDetails(containerId, vendorId) {
    return this._httpService.get<ContainerDetails[]>('containertracking/ContainerDetails/' + containerId + '/' + vendorId)
      .map(response => {
        return response;
      });
  }

  ContainerTrackingGetAttachmentTypes() {
    return this._httpService.get<AttachmentType[]>('containertracking/AttachmentTypes')
      .map(response => {
        return response;
      });
  }

  ContainerTrackingGetNotes(containerId) {
    return this._httpService.get<Note[]>('containertracking/Notes/' + containerId)
      .map(response => {
        return response;
      });
  }

  PostContainerTrackingrData(Containertrackingdetails) {
    return this._httpService.post('containertracking/Details', Containertrackingdetails)
      .map(response => {
        return response;
      });
  }

  PostCTNotes(containerTrakingNotes) {
    return this._httpService.post('containertracking/AddNotes', containerTrakingNotes)
      .map(response => {
        return response;
      });
  }
  ContainerTrackingGetAttachmentsList(containerId) {
    return this._httpService.get<TrackingAttachment[]>('containertracking/AttachmentDetails/' + containerId)
      .map(response => {
        return response;
      });
  }
  ContainerTrackingUploadFile(fileToUpload) {
    return this._httpService.post('containertracking/upload-file', fileToUpload)
      .map(response => {
        return response;
      });
  }

  ContainerTrackingDownloadFile(fileName, containerNumber) {
    return this._httpService.get('containertracking/download-file/' + fileName + '/' + containerNumber)
      .map(response => {
        return response;
      });
  }


  ContainerTrackingDeleteFile(file) {
    return this._httpService.post('containertracking/delete-file', file)
      .map(response => {
        return response;
      });
  }

  //Getting complete container search data from service
  GetContainerSearchData(){
    return this._httpService.get<ContainerMetaData[]>('containertracking/search')
    .map(response => {
      return response;
    });    
  }

}

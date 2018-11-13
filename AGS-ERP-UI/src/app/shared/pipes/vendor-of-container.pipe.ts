import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'vendorOfContainer'
})
export class VendorOfContainerPipe implements PipeTransform {

  transform(value: string, args?: any): string {
    let vendors = [];
    let vendor : string;
    let vendorNames = [] = value.split(',')
    vendorNames.forEach(vName => {
      if(!vendors.includes(vName)){
          vendors.push(vName);
          }
      });
      if(vendors.length > 1){
        vendor = "Multiple";
      }
      if(vendors.length === 1){
        vendor = vendors[0];
      }
    return vendor;
  }

}

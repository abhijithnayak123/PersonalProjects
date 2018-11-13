import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'roundUp'
})
export class RoundUpPipe implements PipeTransform {

  transform(value: any, type?: string): any {
    if(type ==='up'){
      return Math.ceil(value)
    }
  }

}

import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'sortPipe'
})
export class SortPipe implements PipeTransform {

  transform(value: any, sortParameter?: any, unique?: boolean): any {
    if(value !== undefined){
      if(sortParameter !== undefined){
        value.sort((r1, r2) => {
          if (r1[sortParameter] > r2[sortParameter]) {
             return 1;
          }
          else if (r1[sortParameter] < r2[sortParameter]) {
            return -1;
          }
        return 0;
        });
       if(unique !== undefined && unique === true){
         value = this.removeDuplicates(value,sortParameter);
       }
      }
      else{
        value.sort();
      }
    return value;
    }
  }

  /*Added By Satya*/
  removeDuplicates(value, sortParameter) {
        var newArray = [];
        var lookupObject  = {};   
        for(var i in value) {
           lookupObject[value[i][sortParameter]] = value[i];
        }   
        for(i in lookupObject) {
            newArray.push(lookupObject[i]);
        }
         return newArray;
    }
   
}

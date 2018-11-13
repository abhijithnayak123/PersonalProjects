import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'unique',
  pure: false
})
export class UniquePipe implements PipeTransform {
  transform(value: any): any {       
        if(value !== undefined && value !=null){
              var newArray = [];
              var lookupObject  = {};  
              value.sort(); 
              for(var i in value) {
                lookupObject[value[i]] = value[i];
              }   
              for(i in lookupObject) {
                  newArray.push(lookupObject[i]);
              }
              return newArray;          
        }  
    }
  }

 




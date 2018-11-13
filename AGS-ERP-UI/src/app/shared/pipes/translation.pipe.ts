import { Pipe, PipeTransform } from '@angular/core';
import { TranslactionEn } from "../translations/translation_en";
import { TranslactionEs } from "../translations/translation_es";
import { LocalStorageService } from "../wrappers/local-storage.service";

@Pipe({
  name: 'translation'
})
export class TranslationPipe implements PipeTransform {

  constructor(
    localStorageService : LocalStorageService
  ){
    this.preferedLanguage = localStorageService.get("ags_erp_prefered_language");
  }

  preferedLanguage : string;

  transform(value: string) {
    if(this.preferedLanguage == "es"){
        let trans = TranslactionEs[value];
        if(trans){
          return trans;
        }
        else{
          return TranslactionEn[value];
        }
    }
    return TranslactionEn[value];
  }
}

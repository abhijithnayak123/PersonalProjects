
import { Injectable } from '@angular/core';
import { Language } from '../models/language.model';
import { LocalStorageService } from "../../shared/wrappers/local-storage.service";

@Injectable()
export class LanguageService{
constructor(private localStorageService : LocalStorageService){}

  getLanguages(): Array<Language> {
    let lang1=new Language("en","English");
    let lang2 = new Language("es","Spanish");
    return [lang1,lang2];
  }
  
  setPreferedLanguageToLocalStorage(value) {
    this.localStorageService.add("ags_erp_prefered_language", value.code);
  }
  getPreferedLanguageFromLocalStorage() :any{
    return this.localStorageService.get("ags_erp_prefered_language");
  }
}
import { Component, OnInit } from '@angular/core';

import {Language} from '../../models/language.model';
import {LanguageService} from '../../services/language.service';

@Component({
  selector: 'app-language',
  templateUrl: './language.component.html',
  styleUrls: ['./language.component.css']
})
export class LanguageComponent implements OnInit {

  constructor(private _languageService: LanguageService) {}

  dataLanguage: Array<Language>;
  defaultLanguage: Language;

  ngOnInit() {
    this.dataLanguage = this.languages();
    this.checkLocalStorageForDefaultLanguage();
  }

  languages(): Language[] {
    return this._languageService.getLanguages();
  }

  handleLanguageChange(value) {
    this._languageService.setPreferedLanguageToLocalStorage(value);
    location.reload();
  }

  // Checking weather local storage has any default language
  checkLocalStorageForDefaultLanguage() {
    const languageCode = this._languageService.getPreferedLanguageFromLocalStorage();
    if (languageCode === null) {
      this.defaultLanguage = new Language('en', 'English');
      // this.defaultLanguage.code = "en";
      // this.defaultLanguage.name = "English";
    } else {
      this.getPreferedLanguage(languageCode);
    }
  }
  // Read the Language from local Storage and assign as default value in dropdown
  getPreferedLanguage(languageCode: string) {
    const lanuguages = this._languageService.getLanguages();
    lanuguages.forEach(element => {
      if (element.code === languageCode) {
        this.defaultLanguage = new Language(element.code, element.name);
      }
    });
  }
}

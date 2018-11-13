import { Injectable } from '@angular/core';

@Injectable()
export class SessionStorageService {

  constructor() { }

  add(key : string, value : any){
    sessionStorage.setItem(key, JSON.stringify(value));
  }

  get(key: string) : any {
    return JSON.parse(sessionStorage.getItem(key));
  }

  remove(key : string){
    sessionStorage.removeItem(key);
  }

  removeAll(keys: string[]){
    keys.forEach(key => {
      sessionStorage.removeItem(key);
    });
  }
}

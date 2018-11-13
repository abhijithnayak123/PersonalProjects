import { Injectable } from '@angular/core';

@Injectable()
export class LocalStorageService {

  constructor() { }

  add(key : string, value : string){
    localStorage.setItem(key, value);
  }

  get(key: string) : any {
    return localStorage.getItem(key);
  }

  remove(key : string){
    localStorage.removeItem(key);
  }
}

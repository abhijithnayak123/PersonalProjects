import { Injectable } from '@angular/core';

@Injectable()
export class LoggerService {

  constructor() { }

  info(message: string){
    console.info("INFO: " + message);
  }

  warn(message: string){
    console.warn("WARN: " + message);
  }

  error(message: string){
    console.error("ERROR: " + message);
  }
}

import { Directive, ElementRef, HostListener } from '@angular/core';

@Directive({
  selector: '[currencyGroupByThree]'
})
export class CurrencyDirective {

  constructor(
      private el:ElementRef
  ) {}
  @HostListener('change',['$event'])
    onChange(event:KeyboardEvent){
      let price : string;
      if(typeof this.el.nativeElement.value !== 'string'){
        price = String(this.el.nativeElement.value);
      }
      else if(typeof this.el.nativeElement.value === 'string'){
        price = this.el.nativeElement.value.replace(/,/g,'');
      }
      this.el.nativeElement.value = '$'+ Number(price).toLocaleString();
    }

}

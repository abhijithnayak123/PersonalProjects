import { Directive, ElementRef, HostListener } from '@angular/core';

@Directive({
  selector: '[groupByThree]'
})
export class GroupByThreeDirective {

  constructor(
      private el:ElementRef
  ) {}
  @HostListener('change',['$event'])
    onChange(event:KeyboardEvent){
        let numberString = String(this.el.nativeElement.value);
        let number: number = Number(numberString.replace(/,/g,''));
        this.el.nativeElement.value = number.toLocaleString();      
    }
}

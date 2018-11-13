import {Directive, ElementRef,HostListener} from '@angular/core';
import { concat } from 'rxjs/observable/concat';

@Directive({
    selector:'[only-number]'
})
export class NumberDirective{
    private regex:RegExp = new RegExp(/-?(\d+|\d+\.\d+|\.\d+)([eE][-+]?\d+)?/);
    constructor(
        private el:ElementRef
    ){}
    @HostListener('keypress',['$event'])

    onKeyPress(event:KeyboardEvent){
        let numberString = String(this.el.nativeElement.value);
        numberString = numberString.replace(/,/g, '');
        let number: number = Number(numberString);
        if(!String(event.key).match(this.regex)){
            event.preventDefault();
        }        
    }
}
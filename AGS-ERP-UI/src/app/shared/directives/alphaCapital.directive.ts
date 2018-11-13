import { Directive, HostListener, ElementRef } from '@angular/core';

@Directive({
  selector: '[appAlphaCapital]'
})
export class AlphaCapitalDirective {
  private regex: RegExp = new RegExp('^[A-Z+]*$');

  constructor(
    private el: ElementRef
  ) { }
  @HostListener('keypress', ['$event'])
  onKeyPress(event: KeyboardEvent) {
    const string = String(this.el.nativeElement.value);
    if (!String(event.key).match(this.regex)) {
      event.preventDefault();
    }
  }
}

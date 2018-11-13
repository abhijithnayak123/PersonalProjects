import { Directive, HostListener, ElementRef } from '@angular/core';

@Directive({
  selector: '[appAlphaNumeric]'
})
export class AlphaNumericDirective {
  private regex: RegExp = new RegExp('^[a-zA-Z0-9+]*$');

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

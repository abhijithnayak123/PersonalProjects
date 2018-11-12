import { Component, OnInit } from '@angular/core';
import { transition, trigger, state, style, animate, group, keyframes, sequence } from '@angular/animations';
import { MdlDialogService, MdlDialogReference } from '@angular-mdl/core';
import { DisplayPopup } from './../../../data-elements/display-popup';

@Component({
  selector: 'app-confirm-popup',
  templateUrl: './confirm-popup.component.html',
  styleUrls: ['./confirm-popup.component.css'],
  animations: [
    trigger('dialog', [
      transition('void => *', [
        style({ transform: 'scale3d(.3, .3, .3)' }),
        animate(100)
      ]),
      transition('* => void', [
        animate(100, style({ transform: 'scale3d(.0, .0, .0)' }))
      ])
    ])
  ],
})
export class ConfirmPopupComponent implements OnInit {

  _displayPopup: DisplayPopup = new DisplayPopup();
  constructor(private dialog: MdlDialogReference) { }

  ngOnInit() {
    this.GetPopUPDetails();
  }
  public close() {
    this.dialog.hide();
  }

  GetPopUPDetails() {
    const obj = JSON.parse(sessionStorage.getItem('Display-PopUp'));
    this._displayPopup.Title = obj.DisplayPopup.Title;
    this._displayPopup.Message = obj.DisplayPopup.Message;
    this._displayPopup.ButtonText = obj.DisplayPopup.ButtonText;
  }
}

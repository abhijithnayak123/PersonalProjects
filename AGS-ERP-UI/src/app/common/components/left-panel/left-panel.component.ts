import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { Subscription } from 'rxjs/Subscription';

import { HeaderService } from '../../services/header.service';
import { LocalStorageService } from '../../../shared/wrappers/local-storage.service';


@Component({
  selector: 'app-left-panel',
  templateUrl: './left-panel.component.html',
  styleUrls: ['./left-panel.component.css']
})
export class LeftPanelComponent implements OnInit {
  
  @Output() onMenuExpanded = new EventEmitter<Boolean>();
  ngOnInit() {
    this.shortMenuClass = 'shortMenu';    
  }
  shortMenuClass: string;
  onMenuExpaned(isExpanded: boolean){
    this.onMenuExpanded.emit(isExpanded);
    if(isExpanded){
      this.shortMenuClass = 'shortMenu';
    }
    else{
      this.shortMenuClass = '';
    }
  }  
}

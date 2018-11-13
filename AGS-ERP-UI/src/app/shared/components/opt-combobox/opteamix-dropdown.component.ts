import { Component, Directive, Input, Output, EventEmitter, ElementRef, ViewChild, Renderer } from '@angular/core';
import { BinLocationsModel } from '../../../raw-material/inventory/physical-adjustments/models/bin-locations.model';


@Directive({
    selector: '[track-scroll]',
    host: { '(scroll)': 'track($event)' },
})


export class TrackScrollDirective {

    @Output() endOfScrollEvent = new EventEmitter();
    constructor() { }

    track($event) {
        if (Math.ceil($event.target.offsetHeight) + Math.ceil($event.target.scrollTop) === Math.ceil($event.target.scrollHeight)) {
            if ($event.target.id === "customDropDown") { // This is condition check is not mandatory untill you have so many components in one page. This case pass object state/id in the callback envent
                this.endOfScrollEvent.emit(true);
            }
        }
    }

}

export class DropDownModel {
    public id: string;
    public label: string;
}


//   export class BinLocationsModel {
//     public label:string;
//     public label:string; value1 value2 value3

//   }

@Component({
    selector: 'OPT-Dropdown',
    templateUrl: './opteamix-dropdown.component.html',
    styleUrls: ['./opteamix-dropdown.component.css'],
})


export class OPTDropDownComponent {


    dropDownData: Array<BinLocationsModel>;
    pageSize: number = 20;
    pageNumber: number;
    selectedValue: string;
    autoComplete: boolean = false;
    inputValue: string;
    showHide: string;
    @Output() valueChangeSelectedEvent = new EventEmitter();
    @Input() data: Array<BinLocationsModel>;
    @Output() dataChange = new EventEmitter<Array<BinLocationsModel>>();
    @ViewChild('dropDownInputField') dropDownInputElement: ElementRef;

    constructor(private renderer: Renderer) {
        this.data = new Array<BinLocationsModel>();
        this.dropDownData = new Array<BinLocationsModel>();
        this.pageNumber = 0;
        this.showHide = "menu k-dropdown cusc hide"
        this.addData();
    }

    endOfScrollEventReceived(status) {
        if (this.autoComplete === false) {
            this.addData();
        }
    }

    addData() {
        let size = this.pageSize + this.pageNumber;
        if (size > this.data.length) {
            return;
        }
        for (var i = this.pageNumber; i < size; i++) {
            this.dropDownData[i] = this.data[i];
        }
        this.pageNumber = this.pageNumber + this.pageSize;
    }

    onInputChange(event: KeyboardEvent) {
        try {
            this.showHide = "menu k-dropdown cusc show"
            if (event instanceof KeyboardEvent && (<HTMLInputElement>event.target).value !== undefined)  {
                this.inputValue = (<HTMLInputElement>event.target).value;
            }
            if (this.inputValue.length > 0) {
                this.autoComplete = true;
                this.filterDropDownData();
            }
            else {
                this.resetDropDownData();
            }
        }
        catch (ex) { // Mouse event for catching Dropdown arrow click.
            this.dropDownInputElement.nativeElement.focus();
            this.addData();
        }

    }

    clearInput() {
        this.inputValue = "";
        this.resetDropDownData();
        this.valueChangeSelectedEvent.emit(undefined);
    }

    hideDropDown(event: KeyboardEvent) {
        this.showHide = "menu k-dropdown cusc hide";
    }


    onSelectDropDownValue(item) {
        this.showHide = "menu k-dropdown cusc hide";
        this.inputValue = item.AreaCode;
        this.filterDropDownData();
        this.valueChangeSelectedEvent.emit(item);
    }

    filterDropDownData() {
        this.dropDownData = this.data.filter((item) => {
            if (item.AreaCode.startsWith(this.inputValue))
                return item;
            else
                return;
        })
    }


    resetDropDownData() {
        this.pageNumber = 0;
        this.autoComplete = false;
        this.dropDownData.splice(0, this.dropDownData.length);
        this.addData();
    }

}

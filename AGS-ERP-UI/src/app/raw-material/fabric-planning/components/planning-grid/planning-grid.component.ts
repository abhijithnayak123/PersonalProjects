import { Component, OnInit, Input } from '@angular/core';
import { PlanningReport } from '../../models/planning-report';
import { RowClassArgs } from '@progress/kendo-angular-grid';

@Component({
  selector: 'app-planning-grid',
  templateUrl: './planning-grid.component.html',
  styleUrls: ['./planning-grid.component.css']
})
export class PlanningGridComponent implements OnInit {
  @Input() gridData: PlanningReport;
  reportGridData: any[];
  headers: Array<string> = ['Week End Date'];
  FcstReqCSS: Array<string>;
  HistReqCSS: Array<string>;
  constructor() { }

  ngOnInit() {
    this.headers = this.headers.concat(this.gridData.WEDateLabel);
    this.populateData();
  }

  colorCode(a, label) {
    const weekDate = a['Week End Date'];
    const columnValue = a[label];
    const index = this.headers.indexOf(label);
    if (weekDate === 'Forecast Requirement WOS') {
      return this.FcstReqCSS[index - 1] + 'p';
    } else if (weekDate === 'Historical Requirement WOS') {
      return this.HistReqCSS[index - 1] + 'p';
    }
  }

  rowColor(context: RowClassArgs) {

    switch (context.dataItem['Week End Date']) {
      case 'Forecast Requirement':
      case 'Forecast Requirement End Inventory':
      case 'Forecast Requirement WOS':
        return {
          foreCast: true
        };
      case 'Historical Average Requirement':
      case 'Historical Requirement End Inventory':
      case 'Historical Requirement WOS':
        return {
          history: true
        };
      default: return {
        default: true
      };
    }
  }

  populateData() {
    const grid = this.gridData;
    this.FcstReqCSS = this.gridData.FcstReqWOSCSS;
    this.HistReqCSS = this.gridData.HistReqWOSCSS;
    const gridData1 = [];
    let jsonData = {};
    jsonData['Week End Date'] = 'Scheduled Receipts';

    for (let i = 0; i < grid.WEDateLabel.length; i++) {
      jsonData[String(grid.WEDateLabel[i])] = grid.SchedRecpts[i].toLocaleString();
    }
    gridData1.push(jsonData);
    jsonData = {};
    jsonData['Week End Date'] = 'Unallocated Cut Requirement';

    for (let i = 0; i < grid.WEDateLabel.length; i++) {
      jsonData[String(grid.WEDateLabel[i])] = grid.UnallocCutReq[i].toLocaleString();
    }
    gridData1.push(jsonData);
    jsonData = {};
    jsonData['Week End Date'] = 'Forecast Requirement';

    for (let i = 0; i < grid.WEDateLabel.length; i++) {
      jsonData[String(grid.WEDateLabel[i])] = grid.FcstReq[i].toLocaleString();
    }
    gridData1.push(jsonData);
    jsonData = {};
    jsonData['Week End Date'] = 'Forecast Requirement End Inventory';

    for (let i = 0; i < grid.WEDateLabel.length; i++) {
      jsonData[String(grid.WEDateLabel[i])] = grid.FcstReqEndInvt[i].toLocaleString();
    }
    gridData1.push(jsonData);
    jsonData = {};
    jsonData['Week End Date'] = 'Forecast Requirement WOS';

    for (let i = 0; i < grid.WEDateLabel.length; i++) {
      jsonData[String(grid.WEDateLabel[i])] = grid.FcstReqWOS[i].toLocaleString();
    }
    gridData1.push(jsonData);
    jsonData = {};
    jsonData['Week End Date'] = 'Historical Average Requirement';

    for (let i = 0; i < grid.WEDateLabel.length; i++) {
      jsonData[grid.WEDateLabel[i]] = grid.HistAvgReq[i].toLocaleString();
    }
    gridData1.push(jsonData);
    jsonData = {};
    jsonData['Week End Date'] = 'Historical Requirement End Inventory';

    for (let i = 0; i < grid.WEDateLabel.length; i++) {
      jsonData[grid.WEDateLabel[i]] = grid.HistReqEndInvt[i].toLocaleString();
    }
    gridData1.push(jsonData);
    jsonData = {};
    jsonData['Week End Date'] = 'Historical Requirement WOS';

    for (let i = 0; i < grid.WEDateLabel.length; i++) {
      jsonData[grid.WEDateLabel[i]] = grid.HistReqWOS[i].toLocaleString();
    }
    gridData1.push(jsonData);
    this.reportGridData = gridData1;
  }

  getColumnValue(data, column) {
    return data[column];
  }

}

import { Component, OnInit } from '@angular/core';
import { AlertType } from '../../models/alerttype.model';
import { ToastService } from '../../services/toast.service';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-toast',
  templateUrl: './toast.component.html',
  styleUrls: ['./toast.component.css']
})
export class ToastComponent implements OnInit {
  type: AlertType;
  message: string;
  opened = false;
  timeout: number = environment.toastTimeout;
  constructor(
    private _toastService: ToastService,
  ) { }

  ngOnInit() {
    this._toastService.getAlert().subscribe(
      res => {
        if (res.hasOwnProperty('key') && res.key === 'toastMessage') {
          this.type = res.value.type;
          this.message = res.value.message;
          this.opened = true;
          setTimeout(() => {
            this.close();
          }, this.timeout);
        }
      });
  }

  cssClass() {
    switch (this.type) {
      case AlertType.Success:
        return 'alert alert-success';
      case AlertType.Error:
        return 'alert alert-danger';
      case AlertType.Info:
        return 'alert alert-info';
      case AlertType.Warning:
        return 'alert alert-warning';
    }
  }

  close() {
    this.opened = false;
  }
}

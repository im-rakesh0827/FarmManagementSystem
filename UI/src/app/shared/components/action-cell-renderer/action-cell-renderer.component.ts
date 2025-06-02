import { Component } from '@angular/core';
import { ICellRendererAngularComp } from 'ag-grid-angular';

@Component({
  selector: 'app-action-cell-renderer',
  standalone: true,
  templateUrl: './action-cell-renderer.component.html',
  styleUrls: ['./action-cell-renderer.component.scss']
})
export class ActionCellRendererComponent implements ICellRendererAngularComp {
  params: any;

  agInit(params: any): void {
    this.params = params;
  }

  refresh(): boolean {
    return false;
  }

  onView() {
    this.params?.onView?.(this.params.data);
  }

  onEdit() {
    this.params?.onEdit?.(this.params.data);
  }

  onDelete() {
    this.params?.onDelete?.(this.params.data);
  }
}

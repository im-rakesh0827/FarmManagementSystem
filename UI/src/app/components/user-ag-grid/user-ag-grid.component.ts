import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AgGridModule } from 'ag-grid-angular';
import { ColDef } from 'ag-grid-community';
import { ModuleRegistry } from 'ag-grid-community';
import { AllCommunityModule } from 'ag-grid-community'; // ✅ Use singular "Module"
import { ActionCellRendererComponent } from '@shared/components/action-cell-renderer/action-cell-renderer.component';
import { LoaderService } from '@shared/services/loader.service';

ModuleRegistry.registerModules([AllCommunityModule]);

import { UserService } from '@shared/services/user.service';
import { User } from '@shared/models/user.model';

@Component({
  selector: 'app-user-ag-grid',
  standalone: true,
  imports: [
    CommonModule,
    AgGridModule
  ],
  templateUrl: './user-ag-grid.component.html',
  styleUrls: ['./user-ag-grid.component.scss']
})
export class UserAgGridComponent implements OnInit {
  rowData: User[] = [];

  frameworkComponents = {
    actionCellRenderer: ActionCellRendererComponent
  };

  defaultColDef: ColDef = {
    flex: 1,
    minWidth: 100,
    resizable: true
  };

  constructor(private userService: UserService, 
    private loaderService: LoaderService) {}

  ngOnInit(): void {
    this.loaderService.show();
    this.userService.getAllUsers().subscribe({
      next: (data) => {
        this.loaderService.hide();
        this.rowData = data;
        this.setColumnDefs(); // ✅ Only call after data is ready
      },
      error: () => {
        this.loaderService.hide();
        console.error('Failed to fetch users');
      }
    });
  }

  columnDefs: ColDef[] = [];

  private setColumnDefs(): void {
    this.columnDefs = [
      { headerName: '#', valueGetter: 'node.rowIndex + 1', width: 80 },
      { headerName: 'Full Name', field: 'fullName', sortable: true, filter: true },
      { headerName: 'Email', field: 'email', sortable: true, filter: true },
      { headerName: 'Role', field: 'role', sortable: true, filter: true },
      {
        headerName: 'Actions',
        cellRenderer: ActionCellRendererComponent, // ✅ Use component class directly
        cellRendererParams: {
          onView: (user: User) => this.onView(user),
          onEdit: (user: User) => this.onEdit(user),
          onDelete: (user: User) => this.onDelete(user)
        },
        width: 150
      }
    ];
  }

  onView(user: User): void {
    console.log('View user:', user);
  }

  onEdit(user: User): void {
    console.log('Edit user:', user);
  }

  onDelete(user: User): void {
    console.log('Delete user:', user);
  }
  
}

import { Routes } from '@angular/router';
import { RegisterComponent } from './auth/register/register.component';
import { LoginComponent } from './auth/login/login.component';
import { UserListComponent } from './components/user-list/user-list.component';
import { UserAgGridComponent } from './components/user-ag-grid/user-ag-grid.component';
export const routes: Routes = [
  // Default route redirects to login
  { path: '', redirectTo: 'login', pathMatch: 'full' },

  // Auth routes
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },

  // User list/grid
     { path: 'users', component: UserListComponent },
    { path: 'users-grid', component: UserAgGridComponent },

  // Wildcard route (any undefined path redirects to login)
  { path: '**', redirectTo: 'login' }
];

import { Routes } from '@angular/router';
import { RegisterComponent } from './auth/register/register.component';
import { LoginComponent } from './auth/login/login.component';
import { UserListComponent } from './components/user-list/user-list.component';
import { UserAgGridComponent } from './components/user-ag-grid/user-ag-grid.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { authGuard } from './auth/auth.guard'; 

export const routes: Routes = [
  // Default route redirects to dashboard
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },

  // Auth routes
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },

  // Dashboard
  { path: 'dashboard', component: DashboardComponent, canActivate: [authGuard] },
  // User management
  { path: 'users', component: UserListComponent },
  { path: 'users-grid', component: UserAgGridComponent },

  // Wildcard route
  { path: '**', redirectTo: 'dashboard' }
];

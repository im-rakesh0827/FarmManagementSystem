import { Routes } from '@angular/router';
import { RegisterComponent } from './auth/register/register.component';
import { LoginComponent } from './auth/login/login.component';
import { UserListComponent } from './components/user-list/user-list.component';
import { UserAgGridComponent } from './components/user-ag-grid/user-ag-grid.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ForgotPasswordComponent } from './auth/forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './auth/reset-password/reset-password.component';
import { VerifyOtpComponent } from './auth/verify-otp/verify-otp.component';
import { authGuard } from './auth/auth.guard'; 

export const routes: Routes = [
  // Default route redirects to dashboard
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },

  // Auth routes
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'forgot-password', component: ForgotPasswordComponent },
{ path: 'verify-otp', component: VerifyOtpComponent },
{ path: 'reset-password', component: ResetPasswordComponent },


  // Dashboard
  { path: 'dashboard', component: DashboardComponent, canActivate: [authGuard] },
  // User management
  { path: 'users', component: UserListComponent },
  { path: 'users-grid', component: UserAgGridComponent },

  // Wildcard route
  { path: '**', redirectTo: 'dashboard' },
  
];

import { Routes } from '@angular/router';
import { RegisterComponent } from './auth/register/register.component';
import { LoginComponent } from './auth/login/login.component';
import { UserListComponent } from './components/user-list/user-list.component';
import { UserAgGridComponent } from './components/user-ag-grid/user-ag-grid.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ForgotPasswordComponent } from './auth/forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './auth/reset-password/reset-password.component';
import { VerifyOtpComponent } from './auth/verify-otp/verify-otp.component';
import { authGuard } from './core/guards/auth.guard'; 
import { unauthGuard } from './core/guards/unauth.guard';
import { resetPasswordGuard } from './core/guards/reset-password.guard';
import { verifyOtpGuard } from './core/guards/verify-otp.guard';
import { forgotPasswordGuard } from './core/guards/forgot-password.guard';
import { ProfileComponent } from './components/profile/profile.component';


export const routes: Routes = [
  // Default route redirects to dashboard
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },

  // Auth routes
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },

  { path: 'forgot-password', component: ForgotPasswordComponent, canActivate: [forgotPasswordGuard, unauthGuard] },
{ path: 'verify-otp', component: VerifyOtpComponent, canActivate: [verifyOtpGuard, unauthGuard] },
{ path: 'reset-password', component: ResetPasswordComponent, canActivate: [resetPasswordGuard, unauthGuard] },
  { path: 'setting/profile', component: ProfileComponent, canActivate: [authGuard] },


  // Dashboard
  { path: 'dashboard', component: DashboardComponent, canActivate: [authGuard] },
  // User management
  { path: 'users', component: UserListComponent, canActivate: [authGuard] },
  { path: 'users-grid', component: UserAgGridComponent, canActivate: [authGuard] },

  // Wildcard route
  { path: '**', redirectTo: 'dashboard' },
  
];

import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService, LoginRequest, AuthResponse } from '@shared/services/auth.service';
import { Router } from '@angular/router';
import { LoaderService } from '@shared/services/loader.service';
@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  model: LoginRequest = {
    email: '',
    password: ''
  };

  error = '';
  success = '';

  constructor(
    private authService: AuthService,
    private router: Router,
    private loaderService: LoaderService) { }

  /**
   * Redirect to dashboard if already logged in
   */
  ngOnInit(): void {
    if (this.authService.isAuthenticated()) {
      this.router.navigate(['/dashboard']);
    }
    else {
        this.authService.setCameFromLogin(true);
    }
  }

  goToRegister() {
  this.router.navigate(['/register']);
  }
  
  forgotPassword() {
    this.authService.resetOtpFlow(); // clear previous flow
  this.authService.setCameFromLogin(true);
    this.router.navigate(['/forgot-password']);
  }

  /**
   * Handle login
   */
  login(): void {
    this.error = '';
    this.success = '';
    console.log('📦 Sending login request:', this.model);
    this.loaderService.show(); // Show loader before API call
    this.authService.login(this.model).subscribe({
      next: (response: AuthResponse) => {
        this.loaderService.hide();
        this.success = '✅ Login successful.';
        console.log('🎉 Auth success:', response);

        // Store auth data in localStorage
        localStorage.setItem('token', response.token);
        localStorage.setItem('role', response.role);
        localStorage.setItem('email', response.email);
        localStorage.setItem('fullName', response.fullName);

        // Navigate to dashboard/home
        this.router.navigate(['/dashboard']);
      },
      error: err => {
        this.loaderService.hide();
        this.error = err.error?.message || '❌ Login failed.';
        console.error('Login error:', err);
      }
    });
  }
}

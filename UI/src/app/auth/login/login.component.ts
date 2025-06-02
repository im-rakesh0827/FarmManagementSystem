import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService, LoginRequest, AuthResponse } from '@shared/services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  model: LoginRequest = {
    email: '',
    password: ''
  };

  error = '';
  success = '';

  constructor(private authService: AuthService, private router: Router) {}

  login() {
    this.error = '';
    this.success = '';
    console.log('📦 Sending login request:', this.model);

    this.authService.login(this.model).subscribe({
      next: (response: AuthResponse) => {
        this.success = '✅ Login successful.';
        console.log('🎉 Auth success:', response);

        // Save token to local storage
        localStorage.setItem('token', response.token);
        localStorage.setItem('role', response.role);
        localStorage.setItem('email', response.email);
        localStorage.setItem('fullName', response.fullName);

        // Navigate to dashboard/home
        this.router.navigate(['/dashboard']);
      },
      error: err => {
        this.error = err.error?.message || '❌ Login failed.';
        console.error('Login error:', err);
      }
    });
  }
}

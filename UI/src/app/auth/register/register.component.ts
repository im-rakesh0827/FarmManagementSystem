import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService, RegisterRequest } from '@shared/services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  model: RegisterRequest = {
    fullName: '',
    email: '',
    password: '',
    role: ''
  };

  roles = ['User', 'Manager', 'Admin', 'Operator'];
  error = '';
  success = '';

  constructor(private authService: AuthService) {}

  register() {
    this.success = '';
    this.error = '';
    console.log('📦 Sending registration request:', this.model);

    this.authService.register(this.model).subscribe({
      next: () => {
        this.success = '✅ Registration successful. You can now log in.';
        this.model = { fullName: '', email: '', password: '', role: '' }; // Reset form
      },
      error: err => {
        this.error = err;
        console.error('❌ Registration error:', err);
      }
    });
  }
}

import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AuthService } from '@shared/services/auth.service';

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent {
  email: string = '';
  newPassword: string = '';
  confirmPassword: string = '';
  error: string = '';
  success: string = '';

  constructor(private route: ActivatedRoute, private router: Router, private authService: AuthService) {
    this.route.queryParams.subscribe(params => {
      this.email = params['email'];
    });
  }

  resetPassword() {
    if (this.newPassword !== this.confirmPassword) {
      this.error = 'Passwords do not match.';
      return;
    }

    this.authService.resetPasswordViaOtp(this.email, this.newPassword).subscribe({
      next: () => {
        this.success = 'Password reset successful. You can now login.';
        this.router.navigate(['/login']); // optional
      },
      error: (err) => {
        this.error = err.error?.message || 'Failed to reset password.';
      }
    });
  }

}

import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AuthService } from '@shared/services/auth.service';
import { LoaderService } from '@shared/services/loader.service';

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

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService,
  private loaderService: LoaderService) {
  const navigation = this.router.getCurrentNavigation();
  const state = navigation?.extras?.state as { email?: string };

  this.email = state?.email || sessionStorage.getItem('resetPasswordEmail') || '';

  if (!this.email) {
    this.router.navigate(['/login']);
  }
}


  resetPassword() {
    if (this.newPassword !== this.confirmPassword) {
      this.error = 'Passwords do not match.';
      return;
    }
    this.loaderService.show();

    this.authService.resetPasswordViaOtp(this.email, this.newPassword).subscribe({
      next: () => {
        this.loaderService.hide();
        this.success = 'Password reset successful. You can now login.';
        sessionStorage.removeItem('resetPasswordEmail');
        this.authService.resetOtpFlow();
        this.authService.setCameFromLogin(true);
        this.router.navigate(['/login']);
      },
      error: (err) => {
        this.loaderService.hide();
        this.error = err.error?.message || 'Failed to reset password.';
      }
    });
  }

}

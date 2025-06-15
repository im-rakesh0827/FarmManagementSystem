import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '@shared/services/auth.service';
import { Router } from '@angular/router';
import { AuthFlowService } from '@shared/services/auth-flow.service';
import { LoaderService } from '@shared/services/loader.service';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss']
})
export class ForgotPasswordComponent {
  email = '';
  otp = '';
  success = '';
  error = '';
  showOtpInput = false;
  isEmailSent = false;
  countdown = 30;
  timer: any;

  constructor(
    private authService: AuthService,
    private authFlowService: AuthFlowService,
    private router: Router,
    private loaderService: LoaderService) { }

  sendOtp() {
    this.loaderService.show();
  this.authService.requestOtp(this.email).subscribe({
    next: () => {
      sessionStorage.setItem('resetPasswordEmail', this.email);
      this.loaderService.hide();
      this.success = 'OTP sent to your email.';
      this.error = '';
      this.isEmailSent = true;
      this.showOtpInput = true;
      this.authFlowService.markOtpSent();
      this.authService.setCameFromLogin(false); // consumed

      // this.router.navigate(['/verify-otp'], { queryParams: { email: this.email } });
      this.router.navigate(['/verify-otp'], {state: { email: this.email }
});
    },
    error: () => {
      this.loaderService.hide();
      this.error = 'Failed to send OTP.';
      this.success = '';
    }
  });
}
}
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '@shared/services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-verify-otp',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './verify-otp.component.html',
  styleUrls: ['./verify-otp.component.scss']
})
export class VerifyOtpComponent {
  email = '';
  otp = '';
  success = '';
  error = '';
  showOtpInput = false;
  isEmailSent = false;
  countdown = 10;
  timer: any;

  constructor(private authService: AuthService, private router: Router) {}

  sendOtp() {
    this.authService.requestOtp(this.email).subscribe({
      next: () => {
        this.success = 'OTP sent to your email.';
        this.error = '';
        this.isEmailSent = true;
        this.showOtpInput = true;
        this.startTimer();
      },
      error: () => {
        this.error = 'Failed to send OTP.';
        this.success = '';
      }
    });
  }

  startTimer() {
    this.countdown = 10;
    this.timer = setInterval(() => {
      this.countdown--;
      if (this.countdown === 0) {
        clearInterval(this.timer);
      }
    }, 1000);
  }

  resendOtp() {
    this.sendOtp();
  }

  getMaskedEmail(): string {
    const parts = this.email.split('@');
    if (parts.length !== 2) return this.email;
    const name = parts[0];
    const domain = parts[1];
    const maskedName = name[0] + '*'.repeat(Math.max(1, name.length - 1));
    const maskedDomain = domain[0] + '*'.repeat(Math.max(1, domain.length - 5)) + domain.slice(-4);
    return `${maskedName}@${maskedDomain}`;
  }

  verifyOtp() {
    this.authService.verifyOtp(this.email, this.otp).subscribe({
      next: () => {
        this.router.navigate(['/reset-password'], { queryParams: { email: this.email } });
      },
      error: () => {
        this.error = 'Invalid OTP.';
        this.success = '';
      }
    });
  }
}
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '@shared/services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthFlowService } from '@shared/services/auth-flow.service';
import { LoaderService } from '@shared/services/loader.service';

@Component({
  selector: 'app-verify-otp',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './verify-otp.component.html',
  styleUrls: ['./verify-otp.component.scss']
})
export class VerifyOtpComponent implements OnInit {
  email = '';
  otp = '';
  success = '';
  error = '';
  countdown = 10;
  timer: any;

  constructor(
    private authService: AuthService,
    private authFlowService: AuthFlowService,
    private route: ActivatedRoute,
    private router: Router,
    private loaderService: LoaderService
  ) {}


  ngOnInit(): void {
  this.route.queryParams.subscribe(() => {
    // Try to get email from navigation state or sessionStorage
    const navigation = this.router.getCurrentNavigation();
    const state = navigation?.extras?.state as { email?: string };
    this.email = state?.email || sessionStorage.getItem('resetPasswordEmail') || '';
    if (this.email) {
      this.startTimer();
    } else {
      this.error = 'Email is missing. Cannot verify OTP.';
      // Optionally redirect user to login
      this.router.navigate(['/login']);
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
    if (!this.email) {
      this.error = 'Email not found. Cannot resend OTP.';
      return;
    }
    this.loaderService.show();
    this.authService.requestOtp(this.email).subscribe({
      next: () => {
        this.loaderService.hide();
        this.success = 'OTP resent successfully.';
        this.error = '';
        this.startTimer();
      },
      error: () => {
        this.loaderService.hide();
        this.error = 'Failed to resend OTP.';
        this.success = '';
      }
    });
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
    if (!this.email || !this.otp) {
      this.error = 'Email or OTP is missing.';
      return;
    }
    this.loaderService.show();

    this.authService.verifyOtp(this.email, this.otp).subscribe({
      next: () => {
        this.loaderService.hide();
        this.success = '';
        this.error = '';
        this.authFlowService.markOtpVerified();
        this.router.navigate(['/reset-password'], { state: { email: this.email } });
      },
      error: () => {
        this.loaderService.hide();
        this.error = 'Invalid OTP or OTP expired.';
        this.success = '';
      }
    });
  }
}

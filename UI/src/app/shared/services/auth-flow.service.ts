import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthFlowService {
  private otpSent = false;
  private otpVerified = false;

  markOtpSent() {
    this.otpSent = true;
  }

  markOtpVerified() {
    this.otpVerified = true;
  }

  isOtpSent(): boolean {
    return this.otpSent;
  }

  isOtpVerified(): boolean {
    return this.otpVerified;
  }

  resetFlow() {
    this.otpSent = false;
    this.otpVerified = false;
  }
}

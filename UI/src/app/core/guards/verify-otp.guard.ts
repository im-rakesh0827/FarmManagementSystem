
import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { AuthFlowService } from '@shared/services/auth-flow.service';

export const verifyOtpGuard: CanActivateFn = () => {
  const authFlowService = inject(AuthFlowService);
  const router = inject(Router);

  // Allow access only if OTP has been sent (not yet verified)
  if (authFlowService.isOtpSent() && !authFlowService.isOtpVerified()) {
    return true;
  } else {
    router.navigate(['/login']); // Redirect back to login if flow is broken
    return false;
  }
};

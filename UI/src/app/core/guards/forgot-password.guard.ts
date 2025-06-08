// core/guards/forgot-password.guard.ts
import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { AuthService } from '@shared/services/auth.service';

export const forgotPasswordGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (!authService.getOtpSentStatus() || authService.getCameFromLogin()) {
    authService.setCameFromLogin(false); // consume flag
    return true;
  }

  router.navigate(['/login']);
  return false;
};

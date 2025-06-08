// src/app/core/guards/reset-password.guard.ts
import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { AuthFlowService } from '@shared/services/auth-flow.service';

export const resetPasswordGuard: CanActivateFn = () => {
  const authFlowService = inject(AuthFlowService);
  const router = inject(Router);

  if (authFlowService.isOtpVerified()) {
    return true;
  } else {
    router.navigate(['/login']);
    return false;
  }
};

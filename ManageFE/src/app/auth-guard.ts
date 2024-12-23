import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from './M-Services';

export const authGuard = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.isLoggedIn()) {
    return true; // Navigazione consentita
  } else {
    router.navigate(['Login']); // Reindirizza al login
    return false; // Navigazione bloccata
  }
};
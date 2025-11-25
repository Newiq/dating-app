import { CanActivateFn } from '@angular/router';
import { AccountService } from '../service/account-service';
import { inject } from '@angular/core';
import { ToastService } from '../service/toast-service';

export const authGuard: CanActivateFn = () => {
  const accountService = inject(AccountService);
  const toast = inject(ToastService);
  if (accountService.currentUser()) {
    return true;
  } else {
    toast.error('You shall not pass! Please log in to access this page.');
    return false;
  }
};

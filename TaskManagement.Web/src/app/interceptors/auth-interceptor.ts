import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { AccountService } from '../services/account-service';
import { Router } from '@angular/router';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  
  const accountService = inject(AccountService);
  const router = inject(Router);

  const token = accountService.getToken();

  if (token) {
    const clonedReq = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
    return next(clonedReq).pipe(
      catchError((error: HttpErrorResponse) => {
        if(error.status === 401 && error.error.detail === "Invalid token") {
          accountService.logout();
          router.navigate(['/login']);
        }
        return throwError(() => error)
      })
    );
  }

  return next(req);
};

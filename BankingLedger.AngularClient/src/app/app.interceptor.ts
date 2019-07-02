import { HttpInterceptor, HttpRequest, HttpHandler } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { Injectable } from '@angular/core';
import { AccountService } from './account.service';
import { Router } from '@angular/router';

export class ApiRequestInterceptor implements HttpInterceptor {
    intercept(req: HttpRequest<any>, next: HttpHandler) {
        const token = localStorage.getItem('token');
        const newReq = req.clone({
            headers: req.headers
                .append('Authorization', 'Bearer ' + token)
                .append('Content-Type', 'application/json')
        });

        return next.handle(newReq);
    }
}

@Injectable({
    providedIn: 'root'
})
export class ErrorHandlingInterceptor implements HttpInterceptor {

    constructor(private accountService: AccountService, private router: Router) {}

    intercept(req: HttpRequest<any>, next: HttpHandler) {
        return next.handle(req).pipe(
            catchError(err => {
                if (err.status === 401) {
                    this.accountService.removeToken();
                    this.router.navigate(['/auth']);
                }

                return throwError(err);
            })
        );
    }

}
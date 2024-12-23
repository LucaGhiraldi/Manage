import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { AuthService } from "../API/Auth/auth.service";
import { Observable } from "rxjs";


@Injectable()
export class JwtInterceptor implements HttpInterceptor {

    constructor(private authService: AuthService) {}

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const isLoggedIn = this.authService.isLoggedIn();

        if (isLoggedIn) {
            request = request.clone({
                setHeaders: {
                    Authorization: `Bearer ${localStorage.getItem('accessToken')}`
                }
            });
        }

        return next.handle(request);
    }
}
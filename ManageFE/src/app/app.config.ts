import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { HTTP_INTERCEPTORS, provideHttpClient } from '@angular/common/http';
import { JwtInterceptor } from './M-Services/Interceptors/jwt-interceptor';
import { ErrorInterceptor } from './M-Services';

export const appConfig: ApplicationConfig = {
  providers: [
    provideAnimationsAsync(),
    // provideAnimations(),
    provideHttpClient(),
    provideRouter(routes), 
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true }, // Class-Based
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true } // Class-Based Error
  ]
};

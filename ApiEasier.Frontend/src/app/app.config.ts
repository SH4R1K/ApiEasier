import { NG_EVENT_PLUGINS } from '@taiga-ui/event-plugins';
import { provideAnimations } from '@angular/platform-browser/animations';
import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { APP_ROUTES } from './app.routes';
import { HTTP_INTERCEPTORS, provideHttpClient } from '@angular/common/http';
import { HttpErrorInterceptor } from './interceptors/http-error.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(),
    { provide: HTTP_INTERCEPTORS, useClass: HttpErrorInterceptor, multi: true },
    provideAnimations(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(APP_ROUTES),
    NG_EVENT_PLUGINS,
  ],
};

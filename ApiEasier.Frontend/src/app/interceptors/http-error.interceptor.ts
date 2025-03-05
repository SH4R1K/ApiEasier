import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ErrorHandlerService } from '../services/error-handler.service';
import { TuiAlertService } from '@taiga-ui/core';
import { Injectable } from '@angular/core';

/**
 * Интерсептор HttpErrorInterceptor перехватывает HTTP-запросы и обрабатывает ошибки,
 * возникающие в процессе выполнения запросов. Он использует сервис ErrorHandlerService
 * для обработки ошибок и TuiAlertService для отображения уведомлений об ошибках.
 *
 * @remarks
 * Этот интерсептор позволяет централизованно обрабатывать ошибки HTTP-запросов,
 * что упрощает управление ошибками в приложении. Он автоматически показывает
 * уведомления об ошибках пользователю и может быть настроен для выполнения
 * дополнительных действий при возникновении ошибок.
 *
 */
@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {

  constructor(private errorHandler: ErrorHandlerService, private alerts: TuiAlertService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        this.errorHandler.handleError(error);
        return throwError(error);
      })
    );
  }
}

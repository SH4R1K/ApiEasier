import { Injectable } from '@angular/core';
import {
  HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ErrorHandlerService } from '../services/error-handler.service';

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
  /**
   * Конструктор интерсептора.
   * @param {ErrorHandlerService} errorHandler - Сервис для обработки ошибок.
   */
  constructor(private errorHandler: ErrorHandlerService,) {}

  /**
   * Метод перехвата HTTP-запросов.
   * @param {HttpRequest<any>} req - HTTP-запрос.
   * @param {HttpHandler} next - HTTP-обработчик.
   * @returns {Observable<HttpEvent<any>>} - Наблюдаемый объект с HTTP-событиями.
   * @memberof HttpErrorInterceptor
   */
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        this.errorHandler.handleError(error);
        return throwError(error);
      })
    );
  }
}

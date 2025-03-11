import { HttpErrorResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { TuiAlertService } from "@taiga-ui/core";

/**
 * Сервис для обработки ошибок HTTP-запросов.
 *
 * @remarks
 * Этот сервис предоставляет метод для обработки ошибок, возникающих при выполнении HTTP-запросов.
 * Он анализирует статус ошибки и выполняет соответствующие действия, такие как отображение сообщений об ошибках
 * и перенаправление пользователя на страницы ошибок.
 *
 * @type {ErrorHandlerService}
 * @memberof Component
 */
@Injectable({
  providedIn: 'root',
})
export class ErrorHandlerService {
  private errorMessages: { [key: number]: string } = {
    404: 'Страница не найдена. Возможно, она была удалена или перемещена. 🕵️‍♂️',
    405: 'Метод не разрешен. Попробуйте другой метод запроса. 🛑',
    406: 'Неприемлемый запрос. Сервер не может отправить ответ в запрашиваемом формате. 🚫',
    408: 'Время ожидания запроса истекло. Попробуйте еще раз. ⏳',
    409: 'Конфликт. Попробуйте обновить данные и повторите запрос. 💥',
    410: 'Ресурс удален и больше недоступен. 🗑️',
    413: 'Слишком большая полезная нагрузка. Уменьшите размер запроса. 📦',
    418: 'Я чайник. ☕️',
    422: 'Необработанный контент. Запрос не может быть обработан. 📜',
    429: 'Слишком много запросов. Попробуйте позже. 🕒',
    502: 'Плохой шлюз. Сервер недоступен. Попробуйте позже. 🌐',
    503: 'Сервис временно недоступен. Мы на ремонте! 🔧',
    504: 'Время ожидания ответа от сервера истекло. Попробуйте еще раз. ⏰',
  };

  constructor(private router: Router, private readonly alert: TuiAlertService) {}

  /**
   * Обрабатывает ошибки HTTP-запросов.
   *
   * @param {HttpErrorResponse} error - Объект ошибки HTTP.
   * @returns {void}
   * @memberof ErrorHandlerService
   */
  handleError(error: HttpErrorResponse): void {
    console.error('Error occurred:', error);
    const errorCode = error.status as number;

    if (errorCode === 400) {
      this.handleBadRequestError();
      return;
    }

    const errorMessage =
      this.errorMessages[errorCode] ||
      `Неизвестная ошибка. Код: ${errorCode}. 🤷‍♂️`;

    if (errorCode === 404) {
      this.router.navigate(['/page-not-found']);
      return;
    }

    this.router.navigate(['/error'], {
      queryParams: { code: errorCode, message: errorMessage },
    });
  }

  private handleBadRequestError(): void {
    const badRequestErorMessage =
      'Неверный запрос. Проверьте данные и попробуйте снова. 🤦‍♂️';
    this.alert.open(badRequestErorMessage).subscribe();
    return;
  }
}

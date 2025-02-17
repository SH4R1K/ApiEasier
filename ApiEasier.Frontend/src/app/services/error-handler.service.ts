import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { TuiAlertService } from '@taiga-ui/core';

@Injectable({
  providedIn: 'root',
})
export class ErrorHandlerService {
  constructor(private router: Router, private alertService: TuiAlertService) {}

  handleError(error: HttpErrorResponse): void {
    let errorMessage = 'Произошла ошибка при обработке запроса';
    let errorCode = error.status || 'Unknown';

    switch (error.status) {
      case 400:
        errorMessage = 'Неверный запрос. Проверьте данные и попробуйте снова. 🤦‍♂️';
        this.alertService.open(`Error ${errorCode}: ${errorMessage}`).subscribe();
        break;
      case 404:
        errorMessage = 'Страница не найдена. Возможно, она была удалена или перемещена. 🕵️‍♂️';
        this.router.navigate(['/page-not-found']);
        return;
      case 408:
        errorMessage = 'Время ожидания запроса истекло. Попробуйте еще раз. ⏳';
        break;
      case 500:
        errorMessage = 'Внутренняя ошибка сервера. Мы уже работаем над этим! 🛠️';
        break;
      case 502:
        errorMessage = 'Плохой шлюз. Сервер недоступен. Попробуйте позже. 🌐';
        break;
      case 503:
        errorMessage = 'Сервис временно недоступен. Мы на ремонте! 🔧';
        break;
      case 504:
        errorMessage = 'Время ожидания ответа от сервера истекло. Попробуйте еще раз. ⏰';
        break;
      default:
        errorMessage = `Неизвестная ошибка: ${errorMessage}. Код: ${errorCode}. 🤷‍♂️`;
        break;
    }

    // Переход на страницу ошибки для всех остальных случаев
    this.router.navigate(['/error'], {
      queryParams: { code: errorCode, message: errorMessage },
    });
  }
}

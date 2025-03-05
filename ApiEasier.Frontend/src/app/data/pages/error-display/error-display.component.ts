import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';
import { TuiButton } from '@taiga-ui/core';

/**
 * Компонент ErrorDisplayComponent отвечает за отображение страницы с ошибкой.
 * Он получает код ошибки и сообщение об ошибке из параметров маршрута и отображает их пользователю.
 *
 * @remarks
 * Этот компонент используется для отображения информации об ошибках, которые произошли в приложении.
 * Он предоставляет пользователю возможность вернуться на главную страницу.
 *
 * @example
 * HTML:
 * ```html
 * <app-error-display></app-error-display>
 */
@Component({
  selector: 'app-error-display',
  imports: [TuiButton],
  templateUrl: './error-display.component.html',
  styleUrls: ['./error-display.component.css'],
})
export class ErrorDisplayComponent implements OnInit {
  errorCode!: string;

  /**
   * Сообщение об ошибке.
   * @type {string}
   * @memberof ErrorDisplayComponent
   */
  errorMessage!: string;
  timeLeft: number = 20;
  private timer: any;
  private interval: any;

  /**
   * Конструктор компонента.
   * @param {ActivatedRoute} route - Активированный маршрут для получения параметров маршрута.
   */
  constructor(
    private route: ActivatedRoute,
    private location: Location
  ) {}

  /**
   * Метод жизненного цикла, который вызывается при инициализации компонента.
   * Получает параметры маршрута и устанавливает код ошибки и сообщение об ошибке.
   *
   * @memberof ErrorDisplayComponent
   */
  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      this.errorCode = params['code'] || 'Unknown Error';
      this.errorMessage = params['message'] || 'Произошла непредвиденная ошибка.';
    });

    // Установите таймер на 20 секунд
    this.timer = setTimeout(() => {
      this.location.back();
    }, 20000);

    // Обновляйте таймер каждую секунду
    this.interval = setInterval(() => {
      if (this.timeLeft > 0) {
        this.timeLeft--;
      }
    }, 1000);
  }

  ngOnDestroy(): void {
    // Очистите таймер и интервал при уничтожении компонента
    if (this.timer) {
      clearTimeout(this.timer);
    }
    if (this.interval) {
      clearInterval(this.interval);
    }
  }

  /**
   * Метод для возврата на главную страницу.
   *
   * @memberof ErrorDisplayComponent
   */
  goBack(): void {
    this.location.back();
  }
}

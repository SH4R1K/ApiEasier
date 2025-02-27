import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
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
  /**
   * Код ошибки.
   * @type {string}
   * @memberof ErrorDisplayComponent
   */
  errorCode!: string;

  /**
   * Сообщение об ошибке.
   * @type {string}
   * @memberof ErrorDisplayComponent
   */
  errorMessage!: string;

  /**
   * Конструктор компонента.
   * @param {ActivatedRoute} route - Активированный маршрут для получения параметров маршрута.
   * @param {Router} router - Роутер для навигации между представлениями.
   */
  constructor(private route: ActivatedRoute, private router: Router) {}

  /**
   * Метод жизненного цикла, который вызывается при инициализации компонента.
   * Получает параметры маршрута и устанавливает код ошибки и сообщение об ошибке.
   *
   * @memberof ErrorDisplayComponent
   */
  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      this.errorCode = params['code'] || 'Unknown Error';
      this.errorMessage =
        params['message'] || 'Произошла непредвиденная ошибка.';
    });
  }

  /**
   * Метод для возврата на главную страницу.
   *
   * @memberof ErrorDisplayComponent
   */
  goBack(): void {
    this.router.navigate(['/']);
  }
}

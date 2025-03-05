import { Component } from '@angular/core';
import { Location } from '@angular/common';
import { Router } from '@angular/router';

/**
 * Компонент BackButtonComponent предназначен для возврата пользователя на предыдущую страницу.
 * Использует Angular Router и Location для управления навигацией.
 *
 * @remarks
 * Этот компонент предоставляет функциональность кнопки "Назад", которая позволяет пользователю вернуться
 * на предыдущую страницу или на главную страницу, если история навигации пуста.
 *
 * @example
 * html
 * <app-back-button></app-back-button>
 *
 */
@Component({
  selector: 'app-back-button',
  imports: [],
  templateUrl: './back-button.component.html',
  styleUrls: ['./back-button.component.css'], // Исправлено с styleUrl на styleUrls
})
export class BackButtonComponent {
  /**
   * Конструктор компонента BackButtonComponent.
   *
   * @param location - Сервис для работы с историей навигации.
   * @param router - Сервис для управления маршрутизацией.
   *
   * @remarks
   * Внедряет сервисы Location и Router для управления навигацией.
   *
   * @memberof BackButtonComponent
   */
  constructor(private location: Location, private router: Router) {}

  /**
   * Метод для возврата на предыдущую страницу.
   *
   * @remarks
   * Проверяет текущий URL и предыдущий URL. Если предыдущий URL начинается с базового домена,
   * возвращает пользователя на предыдущую страницу. В противном случае перенаправляет на главную страницу.
   *
   * @memberof BackButtonComponent
   */
  goBack(): void {
    try {
      const previousUrl = this.location.path(true);
      const baseDomain = '/';
      if (previousUrl.startsWith(baseDomain)) {
        this.location.back();
      } else {
        this.router.navigateByUrl('/');
      }
    } catch (error) {
      this.router.navigateByUrl('/');
    }
  }
}

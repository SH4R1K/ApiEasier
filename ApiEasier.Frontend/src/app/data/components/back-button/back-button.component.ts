import { Component } from '@angular/core';
import { Location } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-back-button',
  imports: [],
  templateUrl: './back-button.component.html',
  styleUrl: './back-button.component.css'
})
export class BackButtonComponent {
  constructor(private location: Location, private router: Router) {}

  goBack(): void {
    // Получаем текущий URL
    const currentUrl = this.router.url;

    // Проверяем историю браузера для получения предыдущего URL
    try {
      const previousUrl = this.location.path(true); // true включает query params и fragment

      // Базовый домен вашего сайта
      const baseDomain = '/'; // Например, '/' или '/app'

      // Проверяем, принадлежит ли предыдущий URL вашему сайту
      if (previousUrl.startsWith(baseDomain)) {
        this.location.back(); // Переходим назад
      } else {
        // Если предыдущий URL не принадлежит вашему сайту, перенаправляем на главную страницу
        this.router.navigateByUrl('/');
      }
    } catch (error) {
      // Если что-то пошло не так (например, отсутствует история), перенаправляем на главную страницу
      this.router.navigateByUrl('/');
    }
  }

}
import { Component } from '@angular/core';

/**
 * Компонент LoadingComponent предназначен для отображения анимации загрузки с изображением.
 * Используется для индикации процесса загрузки данных или выполнения операций.
 *
 * @remarks
 * Этот компонент отображает статическое изображение, которое может быть заменено на анимацию или другой индикатор загрузки.
 * Подходит для использования в различных частях приложения, где требуется показать пользователю, что выполняется загрузка.
 *
 * @example
 * html
 * <app-loading></app-loading>
 */
@Component({
  selector: 'app-loading',
  imports: [],
  templateUrl: './loading.component.html',
  styleUrls: ['./loading.component.css'], // Исправлено с styleUrl на styleUrls
})
export class LoadingComponent {
  /**
   * Путь к изображению, отображаемому во время загрузки.
   *
   * @type {string}
   * @default "assets/dora.png"
   * @memberof LoadingComponent
   */
  path: string = "assets/dora.png";
}

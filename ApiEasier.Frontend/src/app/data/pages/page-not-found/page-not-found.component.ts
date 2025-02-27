import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { TuiButton } from '@taiga-ui/core';

/**
 * Компонент PageNotFoundComponent отвечает за отображение страницы "Страница не найдена" (404).
 * Он предоставляет пользователю информацию о том, что запрашиваемая страница не существует.
 *
 * @remarks
 * Этот компонент используется для отображения сообщения об ошибке 404, когда пользователь пытается получить доступ к несуществующей странице.
 * Он может быть настроен для отображения дополнительной информации или предложений по навигации.
 *
 * @example
 * HTML:
 * ```html
 * <app-page-not-found></app-page-not-found>
 */
@Component({
  selector: 'app-page-not-found',
  imports: [
    CommonModule,
    TuiButton,
    RouterModule,
  ],
  templateUrl: './page-not-found.component.html',
  styleUrls: ['./page-not-found.component.css'],
})
export class PageNotFoundComponent {
}

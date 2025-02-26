import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
import { BackButtonComponent } from '../back-button/back-button.component';
import { CommonModule } from '@angular/common';
import { Location } from '@angular/common';
import { tuiDialog } from '@taiga-ui/core';
import { ImportDialogComponent } from '../import-dialog/import-dialog.component';
import { Router } from '@angular/router';

/**
 * Компонент HeaderComponent предназначен для отображения заголовка страницы с логотипом и кнопкой.
 * Позволяет пользователю выполнять действия, такие как возврат на предыдущую страницу или импорт данных.
 *
 * @remarks
 * Этот компонент интегрируется с Taiga UI для создания интерактивного интерфейса.
 * Использует диалоговые окна для выполнения операций импорта.
 *
 * @example
 * html
 * <app-header
 *   [buttonText]="'Нажми меня'"
 *   [isApiPage]="true"
 *   (buttonClick)="handleButtonClick()">
 * </app-header>
 */
@Component({
  selector: 'app-header',
  imports: [BackButtonComponent, CommonModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css',
})
export class HeaderComponent implements OnInit {
  /**
   * URL логотипа для отображения в заголовке.
   *
   * @type {string}
   * @default "public/logo__ru.jpg"
   * @memberof HeaderComponent
   */
  @Input() logoUrl: string = 'public/logo__ru.jpg';

  /**
   * Текст кнопки в заголовке.
   *
   * @type {string}
   * @default ''
   * @memberof HeaderComponent
   */
  @Input() buttonText: string = '';

  /**
   * Событие, которое вызывается при нажатии на кнопку.
   *
   * @type {EventEmitter<void>}
   * @memberof HeaderComponent
   */
  @Output() buttonClick: EventEmitter<void> = new EventEmitter<void>();

  /**
   * Флаг, указывающий, является ли текущая страница страницей API.
   *
   * @type {boolean}
   * @default false
   * @memberof HeaderComponent
   */
  @Input() isApiPage: boolean = false;

  constructor(private location: Location, private router: Router) {}

  /**
   * Диалог для импорта данных.
   *
   * @type {tuiDialog}
   * @memberof HeaderComponent
   */
  private readonly dialog = tuiDialog(ImportDialogComponent, {
    closeable: true,
    dismissible: true,
    label: 'Импортировать',
  });
  imageLoaded: boolean = false;

  ngOnInit(): void {
    const img = new Image();
    img.src = this.logoUrl;
    img.onload = () => {
      this.imageLoaded = true;
    };
  }

  onLogoClick(event: Event): void {
    this.router.navigateByUrl('/');
  }
  

  Click(): void {
    this.buttonClick.emit();
  }

  /**
   * Открывает диалоговое окно для импорта данных.
   *
   * @remarks
   * Использует сервис tuiDialog для отображения диалогового окна.
   *
   * @memberof HeaderComponent
   */
  Import(): void {
    this.dialog().subscribe();
  }
}

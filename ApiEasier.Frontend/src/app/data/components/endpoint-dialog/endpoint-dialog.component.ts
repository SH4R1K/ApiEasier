
import { Component, inject, TemplateRef } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TuiAutoFocus } from '@taiga-ui/cdk';
import type { TuiDialogContext } from '@taiga-ui/core';
import { TuiAlertService, TuiButton, TuiDialogService, TuiTextfield } from '@taiga-ui/core';
import { TuiDataListWrapper, TuiSlider } from '@taiga-ui/kit';
import {
  TuiInputModule,
  TuiSelectModule,
  TuiTextfieldControllerModule,
} from '@taiga-ui/legacy';
import { injectContext } from '@taiga-ui/polymorpheus';
import { Endpoint } from '../../../interfaces/Endpoint';

/**
 * Компонент EndpointDialogComponent предназначен для отображения диалогового окна редактирования эндпоинта.
 * Позволяет пользователю вводить и изменять данные эндпоинта, такие как маршрут и тип запроса.
 *
 * @remarks
 * Этот компонент интегрируется с Taiga UI для создания интерактивного интерфейса.
 * Использует сервисы для управления диалоговыми окнами и вводом данных.
 *
 * @example
 * html
 * <app-endpoint-dialog></app-endpoint-dialog>
 */
@Component({
  selector: 'app-endpoint-dialog',
  imports: [
    FormsModule,
    TuiAutoFocus,
    TuiButton,
    TuiDataListWrapper,
    TuiInputModule,
    TuiSelectModule,
    TuiSlider,
    TuiTextfield,
    TuiTextfieldControllerModule,
  ],
  templateUrl: './endpoint-dialog.component.html',
  styleUrl: './endpoint-dialog.component.css',
})
export class EndpointDialogComponent {
  /**
   * Сервис для управления диалоговыми окнами.
   *
   * @type {TuiDialogService}
   * @memberof EndpointDialogComponent
   */
  private readonly dialogs = inject(TuiDialogService);
  private readonly alerts = inject(TuiAlertService);
  
  /**
   * Список доступных типов запросов для эндпоинта.
   *
   * @type {string[]}
   * @memberof EndpointDialogComponent
   */

  readonly types: string[] = ['get', 'post', 'put', 'delete', 'getbyindex'];

  /**
   * Контекст диалогового окна, содержащий данные эндпоинта.
   *
   * @type {TuiDialogContext<Endpoint, Endpoint>}
   * @memberof EndpointDialogComponent
   */
  public readonly context = injectContext<TuiDialogContext<Endpoint, Endpoint>>();

  /**
   * Флаг, указывающий, есть ли значение в поле маршрута.
   *
   * @type {boolean}
   * @returns {boolean} Возвращает true, если маршрут не пустой.
   * @memberof EndpointDialogComponent
   */
  protected get hasValue(): boolean {
    return this.data.route.trim() !== '';
  }

  /**
   * Данные эндпоинта, которые редактируются в диалоговом окне.
   *
   * @type {Endpoint}
   * @memberof EndpointDialogComponent
   */
  protected get data(): Endpoint {
    return this.context.data;
  }

  /**
   * Обработчик отправки формы.
   *
   * @remarks
   * Предотвращает стандартное поведение отправки формы и завершает диалог, если маршрут не пустой.
   *
   * @memberof EndpointDialogComponent
   */
  protected submit(): void {



    if (this.hasValue) {
      this.context.completeWith(this.data);
    }
  }

  /**
   * Открывает диалоговое окно с заданным содержимым.
   *
   * @param content - Шаблон содержимого диалогового окна.
   * @remarks
   * Использует сервис TuiDialogService для открытия диалога.
   *
   * @memberof EndpointDialogComponent
   */
  protected showDialog(content: TemplateRef<TuiDialogContext>): void {
    this.dialogs.open(content, { dismissible: true }).subscribe();
  }

  private showWarning(message: string): void {
    this.alerts
      .open(message, {
        label: 'Предупреждение',
        appearance: 'warning',
        autoClose: 5000,
      })
      .subscribe();
  }

  private showError(message: string): void {
    this.alerts
      .open(message, {
        label: 'Ошибка',
        appearance: 'negative',
        autoClose: 5000,
      })
      .subscribe();
  }

  /**
   * Обработчик ввода данных в поле маршрута.
   *
   * @param event - Событие ввода данных.
   * @remarks
   * Очищает значение от недопустимых символов и обновляет данные эндпоинта.
   *
   * @memberof EndpointDialogComponent
   */
  protected onInput(event: Event): void {
    const input = event.target as HTMLInputElement;
    const cleanedValue = input.value.replace(/[^a-zA-Z0-9]/g, '');

    const maxLength = 200; 
    const finalValue = this.checkLengthAndWarn(cleanedValue, maxLength);

    input.value = finalValue;
    this.data.route = finalValue; 
  }

  private checkLengthAndWarn(value: string, maxLength: number, warningThreshold: number = 15): string {
    if (value.length > maxLength) {
      this.showError(`Вы превышаете допустимую длину в ${maxLength} символов, добавление новых символов невозможно.`);
      return value.slice(0, maxLength); 
    } else if (value.length > maxLength - warningThreshold) {
      this.showWarning(`Вы приближаетесь к границе по длине символов. Осталось ${maxLength - value.length} символов.`);
    }
    return value;
  }
}
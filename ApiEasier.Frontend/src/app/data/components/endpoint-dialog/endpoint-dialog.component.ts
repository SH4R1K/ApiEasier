import type { TemplateRef } from '@angular/core';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TuiAutoFocus } from '@taiga-ui/cdk';
import type { TuiDialogContext } from '@taiga-ui/core';
import { TuiButton, TuiDialogService, TuiTextfield } from '@taiga-ui/core';
import { TuiDataListWrapper, TuiSlider } from '@taiga-ui/kit';
import {
  TuiInputModule,
  TuiSelectModule,
  TuiTextfieldControllerModule,
} from '@taiga-ui/legacy';
import { injectContext } from '@taiga-ui/polymorpheus';
import { Endpoint } from "../../../interfaces/Endpoint";

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
  styleUrls: ['./endpoint-dialog.component.css'],
})
export class EndpointDialogComponent {
  /**
   * Сервис для управления диалоговыми окнами.
   *
   * @type {TuiDialogService}
   * @memberof EndpointDialogComponent
   */
  private readonly dialogs = inject(TuiDialogService);

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
    if (event) {
      event.preventDefault();
    }
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
    const value = input.value;
    const cleanedValue = value.replace(/[^a-zA-Z0-9]/g, '');
    input.value = cleanedValue;
    this.data.route = cleanedValue;
  }
}

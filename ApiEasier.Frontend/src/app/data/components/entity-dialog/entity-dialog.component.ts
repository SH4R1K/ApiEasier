import type { TemplateRef } from '@angular/core';
import {
  ChangeDetectionStrategy,
  Component,
  inject,
  HostListener,
  ViewChild,
  ElementRef,
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TuiAutoFocus } from '@taiga-ui/cdk';
import type { TuiDialogContext } from '@taiga-ui/core';
import {
  TuiButton,
  TuiDialogService,
  TuiTextfield,
  TuiAlertService,
} from '@taiga-ui/core';
import { TuiDataListWrapper, TuiSlider } from '@taiga-ui/kit';
import { TuiTextareaModule } from '@taiga-ui/legacy';
import {
  TuiInputModule,
  TuiSelectModule,
  TuiTextfieldControllerModule,
} from '@taiga-ui/legacy';
import { injectContext } from '@taiga-ui/polymorpheus';
import { Entity } from "../../../interfaces/Entity";

/**
 * Компонент EntityDialogComponent предназначен для отображения диалогового окна редактирования сущности.
 * Позволяет пользователю вводить и изменять данные сущности, такие как имя и структура.
 *
 * @remarks
 * Этот компонент интегрируется с Taiga UI для создания интерактивного интерфейса.
 * Использует сервисы для управления диалоговыми окнами и вводом данных.
 *
 * @example
 * html
 * <app-entity-edit-dialog></app-entity-edit-dialog>
 */
@Component({
  selector: 'app-entity-edit-dialog',
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
    TuiTextareaModule,
  ],
  templateUrl: './entity-dialog.component.html',
  styleUrls: ['./entity-dialog.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class EntityDialogComponent {
  /**
   * Ссылка на элемент ввода имени.
   *
   * @type {ElementRef}
   * @memberof EntityDialogComponent
   */
  @ViewChild('nameInput', { read: ElementRef }) nameInputRef!: ElementRef;

  /**
   * Ссылка на элемент ввода структуры.
   *
   * @type {ElementRef}
   * @memberof EntityDialogComponent
   */
  @ViewChild('descriptionInput', { read: ElementRef })
  structureInputRef!: ElementRef;

  /**
   * Сервис для управления диалоговыми окнами.
   *
   * @type {TuiDialogService}
   * @memberof EntityDialogComponent
   */
  private readonly dialogs = inject(TuiDialogService);

  /**
   * Сервис для отображения уведомлений.
   *
   * @type {TuiAlertService}
   * @memberof EntityDialogComponent
   */
  private readonly alerts = inject(TuiAlertService);

  /**
   * Флаг, указывающий, можно ли отправить данные.
   *
   * @type {boolean}
   * @default true
   * @memberof EntityDialogComponent
   */
  private isCanSubmit: boolean = true;

  /**
   * Контекст диалогового окна, содержащий данные сущности.
   *
   * @type {TuiDialogContext<Entity, Entity>}
   * @memberof EntityDialogComponent
   */
  public readonly context = injectContext<TuiDialogContext<Entity, Entity>>();

  /**
   * Флаг, указывающий, есть ли значение в поле имени.
   *
   * @type {boolean}
   * @returns {boolean} Возвращает true, если имя не пустое.
   * @memberof EntityDialogComponent
   */
  protected get hasValue(): boolean {
    return this.data.name.trim() !== '';
  }

  /**
   * Данные сущности, которые редактируются в диалоговом окне.
   *
   * @type {Entity}
   * @memberof EntityDialogComponent
   */
  protected get data(): Entity {
    return this.context.data;
  }

  /**
   * Возвращает структуру сущности в формате JSON.
   *
   * @type {string}
   * @returns {string} Структура сущности в формате JSON.
   * @memberof EntityDialogComponent
   */
  protected get structure(): string {
    try {
      if (this.data.structure == null) return '';
      return JSON.stringify(this.data.structure, null, 2);
    } catch (error) {
      console.error('Ошибка при преобразовании структуры в JSON:', error);
      return '';
    }
  }

  /**
   * Устанавливает структуру сущности из строки JSON.
   *
   * @param value - Строка JSON для установки структуры.
   * @memberof EntityDialogComponent
   */
  protected set structure(value: string) {
    try {
      this.data.structure = JSON.parse(value);
      this.isCanSubmit = true;
    } catch {
      if (value.length === 0) {
        this.data.structure = null;
        this.isCanSubmit = true;
        return;
      }
      this.isCanSubmit = false;
    }
  }

  /**
   * Обработчик событий клавиатуры.
   *
   * @param event - Событие клавиатуры.
   * @remarks
   * Обрабатывает нажатия клавиш "Enter" и "Escape".
   *
   * @memberof EntityDialogComponent
   */
  @HostListener('document:keydown', ['$event'])
  handleKeyboardEvent(event: KeyboardEvent): void {
    if (event.key === 'Enter') {
      this.handleSubmit();
    } else if (event.key === 'Escape') {
      // Обработка нажатия клавиши "Escape"
    }
  }

  /**
   * Обработчик отправки формы.
   *
   * @remarks
   * Проверяет корректность данных и завершает диалог, если данные валидны.
   *
   * @memberof EntityDialogComponent
   */
  protected handleSubmit(): void {
    if (!this.isCanSubmit) {
      this.showError('JSON не правильной структуры');
      return;
    }
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
   * @memberof EntityDialogComponent
   */
  protected showDialog(content: TemplateRef<TuiDialogContext>): void {
    this.dialogs.open(content, { dismissible: true }).subscribe();
  }

  /**
   * Отображает сообщение об ошибке.
   *
   * @param message - Сообщение об ошибке.
   * @private
   * @memberof EntityDialogComponent
   */
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
   * Обработчик ввода данных в поле имени.
   *
   * @param event - Событие ввода данных.
   * @remarks
   * Очищает значение от недопустимых символов и обновляет данные сущности.
   *
   * @memberof EntityDialogComponent
   */
  protected onInput(event: Event): void {
    const input = event.target as HTMLInputElement;
    const value = input.value;
    const cleanedValue = value.replace(/[^a-zA-Z0-9]/g, '');
    input.value = cleanedValue;
    this.data.name = cleanedValue;
  }

  /**
   * Перемещает фокус на указанное поле ввода.
   *
   * @param targetInput - Ссылка на элемент ввода.
   * @remarks
   * Используется для управления фокусом между полями ввода.
   *
   * @memberof EntityDialogComponent
   */
  protected moveFocus(targetInput: ElementRef): void {
    targetInput.nativeElement.querySelector('input, textarea').focus();
  }
}

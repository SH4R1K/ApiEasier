import { ElementRef, TemplateRef } from '@angular/core';
import { Component, inject, ViewChild } from '@angular/core';
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
import { apiServiceShortStructure } from "../../../interfaces/apiServiceShortStructure";

/**
 * Компонент ApiDialogComponent предназначен для редактирования данных API через диалоговое окно.
 * Использует компоненты Taiga UI для создания интерактивного интерфейса.
 *
 * @remarks
 * Этот компонент предоставляет пользователю возможность редактировать данные API, такие как имя и описание,
 * и отправлять изменения через диалоговое окно.
 * Интегрируется с сервисами Taiga UI для управления диалогами и вводом данных.
 *
 * @example
 * html
 * <app-api-edit-dialog></app-api-edit-dialog>
 * 
 */
@Component({
  selector: 'app-api-edit-dialog',
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
  templateUrl: './api-dialog.component.html',
  styleUrls: ['./api-dialog.component.css'],
})
export class ApiDialogComponent {
  /**
   * Ссылка на элемент ввода имени.
   *
   * @remarks
   * Используется для управления фокусом и значением поля ввода имени.
   *
   * @type {ElementRef}
   * @memberof ApiDialogComponent
   */
  @ViewChild('nameInput', { read: ElementRef }) nameInputRef!: ElementRef;

  /**
   * Ссылка на элемент ввода описания.
   *
   * @remarks
   * Используется для управления фокусом и значением поля ввода описания.
   *
   * @type {ElementRef}
   * @memberof ApiDialogComponent
   */
  @ViewChild('descriptionInput', { read: ElementRef })
  descriptionInputRef!: ElementRef;

  /**
   * Сервис для управления диалоговыми окнами.
   *
   * @remarks
   * Внедряется с использованием Angular DI.
   *
   * @type {TuiDialogService}
   * @memberof ApiDialogComponent
   */
  private readonly dialogs = inject(TuiDialogService);
  private readonly alerts = inject(TuiAlertService);
  /**
   * Контекст диалогового окна, содержащий данные API.
   *
   * @remarks
   * Внедряется с использованием функции injectContext из Taiga UI.
   *
   * @type {TuiDialogContext<apiServiceShortStructure, apiServiceShortStructure>}
   * @memberof ApiDialogComponent
   */
  public readonly context =
    injectContext<
      TuiDialogContext<apiServiceShortStructure, apiServiceShortStructure>
    >();

  /**
   * Флаг, указывающий, есть ли значение в поле ввода имени.
   *
   * @type {boolean}
   * @returns {boolean} Возвращает true, если имя не пустое.
   * @memberof ApiDialogComponent
   */
  protected get hasValue(): boolean {
    return this.data.name.trim() !== '';
  }

  /**
   * Данные API, которые редактируются в диалоговом окне.
   *
   * @type {apiServiceShortStructure}
   * @memberof ApiDialogComponent
   */
  protected get data(): apiServiceShortStructure {
    return this.context.data;
  }

  /**
   * Обработчик отправки формы.
   *
   * @param event - Событие отправки формы.
   * @remarks
   * Предотвращает стандартное поведение отправки формы и завершает диалог, если имя не пустое.
   *
   * @memberof ApiDialogComponent
   */
  protected submit(event?: Event): void {
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
   * @memberof ApiDialogComponent
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
   * Обработчик ввода данных в поле имени.
   *
   * @param event - Событие ввода данных.
   * @remarks
   * Очищает значение от недопустимых символов и обновляет данные API.
   *
   * @memberof ApiDialogComponent
   */
  protected onInput(event: Event): void {
    const input = event.target as HTMLInputElement;
    const cleanedValue = input.value.replace(/[^a-zA-Z0-9]/g, '');
  
    const maxLength = 200; 
    const finalValue = this.checkLengthAndWarn(cleanedValue, maxLength);
  
    input.value = finalValue;
    this.data.name = finalValue;
  }
  
  protected onDescriptionInput(event: Event): void {
    const input = event.target as HTMLInputElement;
    const maxLength = 1000; 
    const warningThreshold = 25; 
    const finalValue = this.checkLengthAndWarn(input.value, maxLength, warningThreshold);
  
    input.value = finalValue;
    this.data.description = finalValue;
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

  /**
   * Перемещает фокус на указанное поле ввода.
   *
   * @param targetInput - Ссылка на элемент ввода.
   * @remarks
   * Используется для управления фокусом между полями ввода.
   *
   * @memberof ApiDialogComponent
   */
  protected moveFocus(targetInput: ElementRef): void {
    targetInput.nativeElement.querySelector('input').focus();
  }
}

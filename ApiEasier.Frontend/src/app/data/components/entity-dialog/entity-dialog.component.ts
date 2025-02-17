import type { TemplateRef } from '@angular/core';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
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
import { Entity } from '../../../services/service-structure-api';

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
  styleUrl: './entity-dialog.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class EntityDialogComponent {
  private readonly dialogs = inject(TuiDialogService);
  private readonly alerts = inject(TuiAlertService);
  private isCanSumbit: boolean = true;
  public readonly context = injectContext<TuiDialogContext<Entity, Entity>>();

  // Геттер для hasValue
  protected get hasValue(): boolean {
    return (
      this.data.name.trim() !== '' // Проверяем, что имя не пустое
    );
  }

  protected get data(): Entity {
    return this.context.data;
  }

  // Геттер для structure (возвращает строку JSON)
  protected get structure(): string {
    try {
      if (this.data.structure == null) return '';
      return JSON.stringify(this.data.structure, null, 2); // Преобразуем объект в строку JSON
    } catch (error) {
      console.error('Ошибка при преобразовании структуры в JSON:', error);
      return ''; // Возвращаем пустую строку в случае ошибки
    }
  }

  protected set structure(value: string) {
    try {
      this.data.structure = JSON.parse(value);
      this.isCanSumbit = true;
    } catch {
      if (value.length == 0) {
        this.data.structure = null;
        this.isCanSumbit = true;
        return;
      }
      this.isCanSumbit = false;
    }
  }

  protected submit(): void {
    if (!this.isCanSumbit) {
      this.showError('JSON не правильной структуры');
      return;
    }
    if (this.hasValue) {
      this.context.completeWith(this.data);
    }
  }

  protected showDialog(content: TemplateRef<TuiDialogContext>): void {
    this.dialogs.open(content, { dismissible: true }).subscribe();
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

  protected onInput(event: Event): void {
    const input = event.target as HTMLInputElement;
    const value = input.value;

    // Очищаем значение поля ввода от недопустимых символов
    const cleanedValue = value.replace(/[^a-zA-Z0-9]/g, '');
    input.value = cleanedValue;

    // Обновляем значение в data
    this.data.name = cleanedValue;
  }
}

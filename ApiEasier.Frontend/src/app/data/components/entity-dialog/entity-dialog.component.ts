import type { TemplateRef } from '@angular/core';
import {
  ChangeDetectionStrategy,
  Component,
  inject,
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
  @ViewChild('nameInput', { read: ElementRef }) nameInputRef!: ElementRef;
  @ViewChild('descriptionInput', { read: ElementRef })
  structureInputRef!: ElementRef;

  private readonly dialogs = inject(TuiDialogService);
  private readonly alerts = inject(TuiAlertService);
  private isCanSubmit: boolean = true;
  public readonly context = injectContext<TuiDialogContext<Entity, Entity>>();

  protected get hasValue(): boolean {
    return this.data.name.trim() !== '';
  }

  protected get data(): Entity {
    return this.context.data;
  }

  protected get structure(): string {
    try {
      if (this.data.structure == null) return '';
      return JSON.stringify(this.data.structure, null, 2);
    } catch (error) {
      console.error('Ошибка при преобразовании структуры в JSON:', error);
      return '';
    }
  }

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

  handleKeyboardEvent(event: KeyboardEvent): void {
    if (event.key === 'Enter') {
      this.handleSubmit();
    } else if (event.key === 'Escape') {
    }
  }

  handleSubmit(): void {
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

  protected onInput(event: Event): void {
    const input = event.target as HTMLInputElement;
    const cleanedValue = input.value.replace(/[^a-zA-Z0-9]/g, '');

    const maxLength = 200; 
    const finalValue = this.checkLengthAndWarn(cleanedValue, maxLength);

    input.value = finalValue;
    this.data.name = finalValue;
  }

  private checkLengthAndWarn(value: string, maxLength: number, warningThreshold: number = 15): string {
    if (value.length > maxLength) {
      this.showError(`Вы превышаете допустимую длину в ${maxLength} символов, добавление новых символов невозможно `);
      return value.slice(0, maxLength); 
    } else if (value.length > maxLength - warningThreshold) {
      this.showWarning(`Вы приближаетесь к границе по длине символов. Осталось ${maxLength - value.length} символов.`);
    }
    return value;
  }

    
  protected moveFocus(targetInput: ElementRef): void {
    targetInput.nativeElement.querySelector('input, textarea').focus();
  }
}

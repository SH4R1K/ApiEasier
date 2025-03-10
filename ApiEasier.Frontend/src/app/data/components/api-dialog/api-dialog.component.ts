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
  @ViewChild('nameInput', { read: ElementRef }) nameInputRef!: ElementRef;
  @ViewChild('descriptionInput', { read: ElementRef })
  descriptionInputRef!: ElementRef;
 private readonly alerts = inject(TuiAlertService);
  private readonly dialogs = inject(TuiDialogService);

  public readonly context =
    injectContext<
      TuiDialogContext<apiServiceShortStructure, apiServiceShortStructure>
    >();

  protected get hasValue(): boolean {
    return this.data.name.trim() !== '';
  }

  protected get data(): apiServiceShortStructure {
    return this.context.data;
  }

  protected submit(event?: Event): void {
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

  protected moveFocus(targetInput: ElementRef): void {
    targetInput.nativeElement.querySelector('input').focus();
  }
}

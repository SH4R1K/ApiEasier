import { CommonModule } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  Output,
  OnInit,
} from '@angular/core';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TuiSwitch, tuiSwitchOptionsProvider } from '@taiga-ui/kit';

/**
 * Компонент SwitchComponent представляет собой переключатель, который позволяет пользователю включать или выключать определенную функцию.
 * Он интегрируется с Taiga UI для создания стилизованного и интерактивного интерфейса.
 *
 * @remarks
 * Этот компонент использует реактивные формы для управления состоянием переключателя.
 * Поддерживает валидацию и может быть настроен для отображения иконки и изменения внешнего вида.
 *
 * @example
 * HTML:
 * ```html
 * <app-switch [value]="isFeatureEnabled" (toggle)="onToggle(\$event)"></app-switch>
 * ```
 *
 * TypeScript:
 * ```typescript
 * isFeatureEnabled: boolean = false;
 *
 * onToggle(value: boolean): void {
 *   this.isFeatureEnabled = value;
 *   console.log('Функция переключена:', value);
 * }
 * ```
 */
@Component({
  selector: 'app-switch',
  imports: [CommonModule, FormsModule, ReactiveFormsModule, TuiSwitch],
  templateUrl: './switch.component.html',
  styleUrls: ['./switch.component.css'],
  providers: [
    tuiSwitchOptionsProvider({ showIcons: false, appearance: () => 'primary' }),
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SwitchComponent implements OnInit {
  /**
   * Значение переключателя.
   * @type {boolean}
   * @default false
   * @memberof SwitchComponent
   */
  @Input() value: boolean = false;

  /**
   * Событие, которое срабатывает при переключении состояния.
   * @type {EventEmitter<boolean>}
   * @memberof SwitchComponent
   */
  @Output() toggle: EventEmitter<boolean> = new EventEmitter<boolean>();

  /**
   * Контроль для валидации состояния переключателя (истина).
   * @type {FormControl<boolean>}
   * @memberof SwitchComponent
   */
  protected readonly invalidTrue = new FormControl(true, () => ({
    invalid: true,
  }));

  /**
   * Контрол для валидации состояния переключателя (ложь).
   * @type {FormControl<boolean>}
   * @memberof SwitchComponent
   */
  protected readonly invalidFalse = new FormControl(false, () => ({
    invalid: true,
  }));

  /**
   * Инициализация компонента.
   * @memberof SwitchComponent
   */
  public ngOnInit(): void {
    this.invalidTrue.markAsTouched();
    this.invalidFalse.markAsTouched();
  }

  /**
   * Обработчик события переключения.
   * @memberof SwitchComponent
   * @example
   * onToggle(); // Вызывает событие toggle с текущим значением переключателя.
   */
  onToggle(): void {
    this.toggle.emit(this.value);
  }
}

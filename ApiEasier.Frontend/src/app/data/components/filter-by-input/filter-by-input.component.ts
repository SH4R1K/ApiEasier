import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { TuiDataListWrapper } from '@taiga-ui/kit';
import { TuiInputModule } from '@taiga-ui/legacy';

/**
 * Компонент FilterByInputComponent предназначен для фильтрации элементов на основе ввода пользователя.
 * Позволяет пользователю вводить текст для поиска и фильтрации списка элементов.
 *
 * @remarks
 * Этот компонент использует реактивные формы Angular для управления вводом и фильтрацией данных.
 * Интегрируется с Taiga UI для создания интерактивного интерфейса.
 *
 * @example
 * html
 * <app-filter-by-input
 *   [label]="'Поиск'"
 *   [controlName]="'search'"
 *   [items]="name"
 *   (searchQuery)="onSearchQuery($event)">
 * </app-filter-by-input>

 */
@Component({
  selector: 'app-filter-by-input',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    TuiDataListWrapper,
    TuiInputModule,
  ],
  templateUrl: './filter-by-input.component.html',
  styleUrls: ['./filter-by-input.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FilterByInputComponent {
  /**
   * Метка для поля ввода.
   *
   * @type {string}
   * @default 'Search'
   * @memberof FilterByInputComponent
   */
  @Input() label: string = 'Search';

  /**
   * Имя контрола формы.
   *
   * @type {string}
   * @default 'search'
   * @memberof FilterByInputComponent
   */
  @Input() controlName: string = 'search';

  /**
   * Список элементов для фильтрации.
   *
   * @type {string[]}
   * @default []
   * @memberof FilterByInputComponent
   */
  @Input() items: string[] = [];

  /**
   * Событие, которое вызывается при изменении поискового запроса.
   *
   * @type {EventEmitter<string>}
   * @memberof FilterByInputComponent
   */
  @Output() searchQuery = new EventEmitter<string>();

  /**
   * Реактивная форма для управления вводом.
   *
   * @type {FormGroup}
   * @memberof FilterByInputComponent
   */
  form = new FormGroup({
    [this.controlName]: new FormControl('', [Validators.pattern('^[a-zA-Z0-9]*$')]),
  });

  /**
   * Возвращает отфильтрованные элементы на основе введенного значения.
   *
   * @type {string[]}
   * @returns {string[]} Отфильтрованные элементы.
   * @memberof FilterByInputComponent
   */
  get filteredItems(): string[] {
    const control = this.form.get(this.controlName);
    if (control) {
      const value = control.value ?? '';
      this.searchQuery.emit(value);
      return this.items.filter(item => item.includes(value));
    }
    return this.items;
  }

  /**
   * Конструктор компонента FilterByInputComponent.
   *
   * @remarks
   * Подписывается на изменения значения контрола формы и вызывает событие searchQuery.
   *
   * @memberof FilterByInputComponent
   */
  constructor() {
    this.form.controls[this.controlName].valueChanges.subscribe(value => {
      this.searchQuery.emit(value ?? '');
    });
  }

  /**
   * Обработчик нажатия клавиш.
   *
   * @param event - Событие нажатия клавиши.
   * @remarks
   * Предотвращает ввод недопустимых символов.
   *
   * @memberof FilterByInputComponent
   */
  onKeyPress(event: KeyboardEvent): void {
    const inputChar = event.key;
    const allowedChars = /^[a-zA-Z0-9]$/;
    if (!allowedChars.test(inputChar) && !event.ctrlKey && !event.metaKey) {
      event.preventDefault();
    }
  }
}

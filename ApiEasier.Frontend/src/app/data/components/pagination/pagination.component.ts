import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  Output,
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TuiPagination } from '@taiga-ui/kit';
import {
  TuiInputSliderModule,
  TuiTextfieldControllerModule,
} from '@taiga-ui/legacy';

/**
 * Компонент PaginationComponent предназначен для управления пагинацией в Angular-приложениях.
 * Он предоставляет удобный интерфейс для навигации по страницам с данными.
 *
 * @remarks
 * Этот компонент интегрируется с Taiga UI для создания интерактивного интерфейса.
 * Использует реактивные формы для управления состоянием пагинации.
 *
 * @example
 * HTML:
 * ```html
 * <app-pagination
 *   [totalItems]="100"
 *   [itemsPerPage]="10"
 *   [currentPage]="1"
 *   (pageChange)="onPageChange(\$event)">
 * </app-pagination>
 * ```
 *
 * TypeScript:
 * ```typescript
 * onPageChange(page: number): void {
 *   console.log('Текущая страница:', page);
 * }
 * ```
 */
@Component({
  selector: 'app-pagination',
  imports: [
    FormsModule,
    TuiInputSliderModule,
    TuiPagination,
    TuiTextfieldControllerModule,
  ],
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PaginationComponent {
  /**
   * Общее количество элементов.
   * @type {number}
   * @default 0
   * @memberof PaginationComponent
   */
  @Input() totalItems: number = 0;

  /**
   * Количество элементов на одной странице.
   * @type {number}
   * @default 16
   * @memberof PaginationComponent
   */
  @Input() itemsPerPage: number = 16;

  /**
   * Текущая страница.
   * @type {number}
   * @default 1
   * @memberof PaginationComponent
   */
  @Input() currentPage: number = 1;

  /**
   * Событие, которое срабатывает при изменении страницы.
   * @type {EventEmitter<number>}
   * @memberof PaginationComponent
   */
  @Output() pageChange = new EventEmitter<number>();

  /**
   * Возвращает общее количество страниц.
   * @readonly
   * @type {number}
   * @memberof PaginationComponent
   */
  get totalPages(): number {
    return Math.ceil(this.totalItems / this.itemsPerPage);
  }

  /**
   * Переходит на указанную страницу.
   * @param {number} page - Номер страницы, на которую нужно перейти.
   * @memberof PaginationComponent
   * @example
   * goToPage(2); // Переходит на вторую страницу.
   */
  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages && page !== this.currentPage) {
      this.currentPage = page;
      this.pageChange.emit(page);
    }
  }
}

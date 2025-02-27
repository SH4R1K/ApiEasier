import {
  ChangeDetectorRef,
  Component,
  CUSTOM_ELEMENTS_SCHEMA,
  OnDestroy,
  OnInit,
} from '@angular/core';
import { Subscription } from 'rxjs';
import { ApiServiceRepositoryService } from '../../../repositories/api-service-repository.service';
import { Router } from '@angular/router';
import { TuiAlertService, tuiDialog } from '@taiga-ui/core';
import { apiServiceShortStructure } from '../../../interfaces/apiServiceShortStructure';
import { CommonModule } from '@angular/common';
import { CardApiComponent } from '../../components/card-api/card-api.component';
import { HeaderComponent } from '../../components/header/header.component';
import { RouterModule } from '@angular/router';
import { LoadingComponent } from '../../components/loading/loading.component';
import { PaginationComponent } from '../../components/pagination/pagination.component';
import {
  TuiInputSliderModule,
  TuiTextfieldControllerModule,
} from '@taiga-ui/legacy';
import { ApiDialogComponent } from '../../components/api-dialog/api-dialog.component';
import { FilterByInputComponent } from '../../components/filter-by-input/filter-by-input.component';
import JSZip from 'jszip';

/**
 * Компонент CardApiListComponent отвечает за отображение списка API в виде карточек
 * и управление их состоянием. Поддерживает фильтрацию, сортировку, пагинацию и экспорт выбранных API.
 *
 * @remarks
 * Этот компонент использует реактивные формы для управления состоянием карточек.
 * Поддерживает валидацию и может быть настроен для отображения иконок и изменения внешнего вида.
 *
 * @example
 * HTML:
 * ```html
 * <app-card-api-list></app-card-api-list>
 * ```
 */
@Component({
  selector: 'app-card-api-list',
  imports: [
    CardApiComponent,
    CommonModule,
    HeaderComponent,
    RouterModule,
    LoadingComponent,
    TuiInputSliderModule,
    TuiTextfieldControllerModule,
    PaginationComponent,
    FilterByInputComponent,
  ],
  templateUrl: './card-api-list.component.html',
  styleUrls: [
    './card-api-list.component.css',
    '../../styles/card-list.css',
    '../../styles/icon.css',
    '../../styles/button.css',
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class CardApiListComponent implements OnInit, OnDestroy {
  /**
   * Массив карточек API.
   * @type {apiServiceShortStructure[]}
   * @memberof CardApiListComponent
   */
  cards: apiServiceShortStructure[] = [];

  /**
   * Отфильтрованный массив карточек API.
   * @type {apiServiceShortStructure[]}
   * @memberof CardApiListComponent
   */
  filteredCards: apiServiceShortStructure[] = [];

  /**
   * Массив имен API.
   * @type {string[]}
   * @memberof CardApiListComponent
   */
  apiNames: string[] = [];

  /**
   * Подписка для управления процессом получения списка API.
   * @type {Subscription | null}
   * @memberof CardApiListComponent
   */
  private sub: Subscription | null = null;

  /**
   * Флаг, указывающий, загружаются ли данные в данный момент.
   * @type {boolean}
   * @default true
   * @memberof CardApiListComponent
   */
  loading = true;

  /**
   * Количество элементов на странице.
   * @type {number}
   * @default 16
   * @memberof CardApiListComponent
   */
  itemsPerPage = 16;

  /**
   * Текущая страница пагинации.
   * @type {number}
   * @default 1
   * @memberof CardApiListComponent
   */
  currentPage = 1;

  /**
   * Флаг, указывающий, активен ли поисковый запрос.
   * @type {boolean}
   * @default false
   * @memberof CardApiListComponent
   */
  searchQueryActive = false;

  /**
   * Флаг, указывающий, отсортированы ли карточки по возрастанию.
   * @type {boolean}
   * @default true
   * @memberof CardApiListComponent
   */
  isSortedAscending = true;

  /**
   * Сообщение об ошибке.
   * @type {string}
   * @memberof CardApiListComponent
   */
  errorMessage = '';

  /**
   * Код ошибки.
   * @type {string}
   * @memberof CardApiListComponent
   */
  errorCode = '';

  /**
   * Множество выбранных API.
   * @type {Set<string>}
   * @memberof CardApiListComponent
   */
  selectedApis = new Set<string>();

  /**
   * Флаг, указывающий, включен ли режим выбора.
   * @type {boolean}
   * @default false
   * @memberof CardApiListComponent
   */
  isSelectionMode = false;

  /**
   * Флаг, указывающий, выбраны ли все элементы.
   * @type {boolean}
   * @default false
   * @memberof CardApiListComponent
   */
  isAllSelected = false;

  /**
   * Объект для создания нового API.
   * @type {apiServiceShortStructure}
   * @memberof CardApiListComponent
   */
  api: apiServiceShortStructure = {
    name: '',
    isActive: false,
    description: '',
  };

  /**
   * Диалог для создания нового API.
   * @type {tuiDialog}
   * @memberof CardApiListComponent
   */
  private readonly dialog = tuiDialog(ApiDialogComponent, {
    dismissible: true,
    label: 'Создать',
  });

  /**
   * Конструктор компонента.
   * @param {ApiServiceRepositoryService} apiServiceRepository - Репозиторий сервиса для управления API.
   * @param {ChangeDetectorRef} changeDetector - Ссылка на детектор изменений для ручного обнаружения изменений.
   * @param {Router} router - Роутер для навигации между представлениями.
   * @param {TuiAlertService} alerts - Сервис уведомлений для отображения уведомлений.
   */
  constructor(
    private apiServiceRepository: ApiServiceRepositoryService,
    private changeDetector: ChangeDetectorRef,
    private router: Router,
    private readonly alerts: TuiAlertService
  ) {}

  /**
   * Обработчик изменения размера окна.
   * @private
   * @memberof CardApiListComponent
   */
  private handleWindowResize() {
    this.changeDetector.detectChanges();
  }

  /**
   * Метод жизненного цикла, который вызывается при инициализации компонента.
   * Загружает список API и добавляет обработчик изменения размера окна.
   *
   * @memberof CardApiListComponent
   */
  ngOnInit(): void {
    this.loadApiList();
    window.addEventListener('resize', () => this.handleWindowResize());
  }

  /**
   * Метод жизненного цикла, который вызывается при уничтожении компонента.
   * Удаляет обработчик изменения размера окна и отписывается от всех активных подписок.
   *
   * @memberof CardApiListComponent
   */
  ngOnDestroy(): void {
    window.removeEventListener('resize', () => this.handleWindowResize());
    this.sub?.unsubscribe();
  }

  /**
   * Загружает список API.
   *
   * @private
   * @memberof CardApiListComponent
   */
  private loadApiList(): void {
    this.sub = this.apiServiceRepository
      .getApiList()
      .subscribe(this.handleApiResponse());
  }

  /**
   * Обрабатывает ответ списка API.
   *
   * @private
   * @returns {Object} - Объект с обработчиками next и error.
   * @memberof CardApiListComponent
   */
  private handleApiResponse() {
    return {
      next: (apiList: apiServiceShortStructure[]) => {
        this.updateApiList(apiList);
      },
      error: (error: any) => {
        this.handleError(error);
      },
    };
  }

  /**
   * Обновляет список API.
   *
   * @private
   * @param {apiServiceShortStructure[]} apiList - Список API.
   * @memberof CardApiListComponent
   */
  private updateApiList(apiList: apiServiceShortStructure[]): void {
    this.cards = apiList;
    this.filteredCards = [...apiList];
    this.sortCards();
    this.apiNames = apiList.map((api) => api.name);
    this.updatePagination();
    this.changeDetector.detectChanges();
    this.loading = false;
  }

  /**
   * Обрабатывает ошибку.
   *
   * @private
   * @param {any} error - Объект ошибки.
   * @memberof CardApiListComponent
   */
  private handleError(error: any): void {
    this.errorMessage = error.message;
    this.errorCode = error.status;
    this.navigateToErrorPage(this.errorCode, this.errorMessage);
  }

  /**
   * Перенаправляет на страницу ошибки.
   *
   * @private
   * @param {string} errorCode - Код ошибки.
   * @param {string} errorMessage - Сообщение об ошибке.
   * @memberof CardApiListComponent
   */
  private navigateToErrorPage(errorCode: string, errorMessage: string): void {
    this.router.navigate(['/error'], {
      queryParams: { code: errorCode, message: errorMessage },
    });
  }

  /**
   * Открывает диалог создания нового API.
   *
   * @param {Event} [event] - Событие.
   * @memberof CardApiListComponent
   */
  openCreateDialog(event?: Event): void {
    if (event) {
      event.preventDefault();
    }
    this.dialog({ ...this.api }).subscribe(this.processDialogData());
  }

  /**
   * Обрабатывает данные из диалога.
   *
   * @private
   * @returns {Object} - Объект с обработчиками next и complete.
   * @memberof CardApiListComponent
   */
  private processDialogData() {
    return {
      next: (data: apiServiceShortStructure) => {
        if (this.isApiNameExists(data.name)) {
          this.showApiNameExistsError();
        } else {
          this.createApiService(data);
        }
      },
      complete: () => this.onDialogClose(),
    };
  }

  /**
   * Проверяет, существует ли API с указанным именем.
   *
   * @private
   * @param {string} name - Имя API.
   * @returns {boolean} - Существует ли API с указанным именем.
   * @memberof CardApiListComponent
   */
  private isApiNameExists(name: string): boolean {
    return this.cards.some((card) => card.name === name);
  }

  /**
   * Обработчик закрытия диалога.
   *
   * @private
   * @memberof CardApiListComponent
   */
  private onDialogClose(): void {
    console.info('Dialog closed');
  }

  /**
   * Показывает ошибку, если API с указанным именем уже существует.
   *
   * @private
   * @memberof CardApiListComponent
   */
  private showApiNameExistsError(): void {
    this.alerts
      .open('Ошибка: API с таким именем уже существует', {
        appearance: 'negative',
      })
      .subscribe();
  }

  /**
   * Создает новый API сервис.
   *
   * @private
   * @param {apiServiceShortStructure} data - Данные нового API.
   * @memberof CardApiListComponent
   */
  private createApiService(data: apiServiceShortStructure): void {
    this.apiServiceRepository.createApiService(data).subscribe({
      next: (response) => this.onApiServiceCreated(response, data),
      error: (error) => this.handleError(error),
    });
  }

  /**
   * Обработчик успешного создания API сервиса.
   *
   * @private
   * @param {any} response - Ответ сервера.
   * @param {apiServiceShortStructure} data - Данные нового API.
   * @memberof CardApiListComponent
   */
  private onApiServiceCreated(
    response: any,
    data: apiServiceShortStructure
  ): void {
    this.cards.push(data);
    this.sortCards();
    this.changeDetector.markForCheck();
    this.alerts
      .open('API успешно создано', {
        appearance: 'success',
      })
      .subscribe();
  }

  /**
   * Обработчик удаления API.
   *
   * @param {string} apiName - Имя удаленного API.
   * @memberof CardApiListComponent
   */
  onApiDeleted(apiName: string): void {
    this.selectedApis.delete(apiName);
    this.removeApiByName(apiName);
    this.updatePagination();
    this.changeDetector.markForCheck();
  }

  /**
   * Удаляет API по имени.
   *
   * @private
   * @param {string} apiName - Имя API.
   * @memberof CardApiListComponent
   */
  private removeApiByName(apiName: string): void {
    this.cards = this.cards.filter((card) => card.name !== apiName);
    this.filteredCards = this.filteredCards.filter(
      (card) => card.name !== apiName
    );
    this.apiNames = this.apiNames.filter((name) => name !== apiName);
  }

  /**
   * Обрабатывает поисковый запрос.
   *
   * @param {string} query - Поисковый запрос.
   * @memberof CardApiListComponent
   */
  onSearchQuery(query: string): void {
    this.searchQueryActive = !!query;
    this.filteredCards = this.cards.filter((card) =>
      card.name.toLowerCase().includes(query.toLowerCase())
    );
    this.currentPage = 1;
    this.sortCards();
    this.updatePagination();
    this.changeDetector.markForCheck();
  }

  /**
   * Возвращает общее количество страниц.
   *
   * @type {number}
   * @memberof CardApiListComponent
   */
  get totalPages(): number {
    return Math.ceil(this.filteredCards.length / this.itemsPerPage);
  }

  /**
   * Возвращает отфильтрованные карточки для текущей страницы.
   *
   * @type {apiServiceShortStructure[]}
   * @memberof CardApiListComponent
   */
  get paginatedCards(): apiServiceShortStructure[] {
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    return this.filteredCards.slice(startIndex, startIndex + this.itemsPerPage);
  }

  /**
   * Обработчик изменения страницы.
   *
   * @param {number} page - Номер страницы.
   * @memberof CardApiListComponent
   */
  onPageChange(page: number): void {
    this.currentPage = page;
    this.checkAllSelectedState();
    this.changeDetector.markForCheck();
  }

  /**
   * Обновляет пагинацию.
   *
   * @private
   * @memberof CardApiListComponent
   */
  private updatePagination(): void {
    const newCurrentPage = Math.max(
      1,
      Math.min(this.currentPage, this.totalPages)
    );
    if (newCurrentPage !== this.currentPage) {
      this.currentPage = newCurrentPage;
      this.changeDetector.markForCheck();
    }
  }

  /**
   * Сортирует карточки.
   *
   * @memberof CardApiListComponent
   */
  sortCards(): void {
    this.filteredCards.sort((a, b) =>
      this.isSortedAscending
        ? a.name.localeCompare(b.name)
        : b.name.localeCompare(a.name)
    );
  }

  /**
   * Переключает сортировку.
   *
   * @memberof CardApiListComponent
   */
  toggleSort(): void {
    this.isSortedAscending = !this.isSortedAscending;
    this.sortCards();
    this.updatePagination();
    this.changeDetector.markForCheck();
  }

  /**
   * Переключает режим выбора.
   *
   * @memberof CardApiListComponent
   */
  toggleSelectionMode(): void {
    this.isSelectionMode = !this.isSelectionMode;
    if (!this.isSelectionMode) {
      this.selectedApis.clear();
      this.isAllSelected = false;
    }
  }

  /**
   * Переключает выбор API.
   *
   * @param {string} apiName - Имя API.
   * @memberof CardApiListComponent
   */
  toggleApiSelection(apiName: string): void {
    if (!this.isSelectionMode) return;

    if (this.selectedApis.has(apiName)) {
      this.selectedApis.delete(apiName);
    } else {
      this.selectedApis.add(apiName);
    }
    this.checkAllSelectedState();
    this.changeDetector.markForCheck();
  }

  /**
   * Переключает выбор всех API.
   *
   * @memberof CardApiListComponent
   */
  toggleSelectAll(): void {
    if (this.isAllSelected) {
      this.filteredCards.forEach((item) => this.selectedApis.delete(item.name));
    } else {
      this.filteredCards.forEach((item) => this.selectedApis.add(item.name));
    }
    this.isAllSelected = !this.isAllSelected;
    this.changeDetector.markForCheck();
  }

  /**
   * Проверяет состояние "Выбрано все".
   *
   * @private
   * @memberof CardApiListComponent
   */
  private checkAllSelectedState(): void {
    this.isAllSelected =
      this.filteredCards.length > 0 &&
      this.filteredCards.every((item) => this.selectedApis.has(item.name));
  }

  /**
   * Экспортирует выбранные API в ZIP-архив.
   *
   * @memberof CardApiListComponent
   */
  exportSelectedApis(): void {
    this.loading = true;
    if (this.selectedApis.size === 0) return;

    const selectedNames = Array.from(this.selectedApis);
    const exportPromises = selectedNames.map((name) =>
      this.apiServiceRepository.getApiStructureList(name).toPromise()
    );

    Promise.all(exportPromises)
      .then((results) => {
        const timestamp = new Date().toISOString().replace(/[:.]/g, '-');
        const zip = new JSZip();

        results.forEach((data, index) => {
          if (!data) return;

          const { name, ...dataWithoutName } = data;
          const fileName = `${name || `api-${index}`}.json`;
          zip.file(fileName, JSON.stringify(dataWithoutName, null, 2));
        });

        zip.generateAsync({ type: 'blob' }).then((content: Blob) => {
          const url = window.URL.createObjectURL(content);
          const a = document.createElement('a');
          a.href = url;
          a.download = `apis-export-${timestamp}.zip`;
          a.click();
          window.URL.revokeObjectURL(url);
        });
      })
      .catch((error: any) => {
        console.error('Export error:', error);
        this.alerts
          .open('Ошибка экспорта', { appearance: 'negative' })
          .subscribe();
      })
      .finally(() => {
        this.loading = false;
        this.selectedApis.clear();
        this.isSelectionMode = false;
      });
  }
}

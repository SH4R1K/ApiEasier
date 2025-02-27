import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  OnDestroy,
  OnInit,
} from '@angular/core';
import { Observable, Subscription, switchMap } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { Entity } from "../../../interfaces/Entity";
import { ApiServiceStructure } from "../../../interfaces/ApiServiceStructure";
import { CommonModule } from '@angular/common';
import { TuiCardLarge } from '@taiga-ui/layout';
import { tuiDialog, TuiAlertService } from '@taiga-ui/core';
import { CardEntityComponent } from '../../components/card-entity/card-entity.component';
import { HeaderComponent } from '../../components/header/header.component';
import { SwitchComponent } from '../../components/switch/switch.component';
import { EntityDialogComponent } from '../../components/entity-dialog/entity-dialog.component';
import { ApiService } from '../../../services/api-service.service';
import { EntityRepositoryService } from '../../../repositories/entity-repository.service';
import { LoadingComponent } from '../../components/loading/loading.component';
import { FilterByInputComponent } from '../../components/filter-by-input/filter-by-input.component';
import { PaginationComponent } from '../../components/pagination/pagination.component';

/**
 * Компонент EntityCardListComponent отвечает за отображение списка сущностей (entities)
 * для выбранного API. Поддерживает создание, удаление и обновление состояния сущностей.
 *
 * @remarks
 * Этот компонент использует реактивные формы для управления состоянием сущностей.
 *
 * @example
 * HTML:
 * ```html
 * <app-entity-card-list></app-entity-card-list>
 */
@Component({
  selector: 'app-entity-card-list',
  imports: [
    TuiCardLarge,
    CommonModule,
    CardEntityComponent,
    HeaderComponent,
    SwitchComponent,
    LoadingComponent,
    FilterByInputComponent,
    PaginationComponent,
  ],
  templateUrl: './entity-card-list.component.html',
  styleUrls: [
    './entity-card-list.component.css',
    '../../styles/card-list.css',
    '../../styles/icon.css',
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class EntityCardListComponent implements OnInit, OnDestroy {
  /**
   * Список сущностей.
   * @type {Entity[]}
   * @memberof EntityCardListComponent
   */
  entities: Entity[] = [];

  /**
   * Отфильтрованный список сущностей.
   * @type {Entity[]}
   * @memberof EntityCardListComponent
   */
  filteredEntities: Entity[] = [];

  /**
   * Список имен сущностей.
   * @type {string[]}
   * @memberof EntityCardListComponent
   */
  entityNames: string[] = [];

  /**
   * Подписка для управления процессом получения данных.
   * @type {Subscription | null}
   * @memberof EntityCardListComponent
   */
  private sub: Subscription | null = null;

  /**
   * Имя API, для которого отображаются сущности.
   * @type {string}
   * @memberof EntityCardListComponent
   */
  apiName!: string;

  /**
   * Флаг, указывающий, загружаются ли данные в данный момент.
   * @type {boolean}
   * @default true
   * @memberof EntityCardListComponent
   */
  loading: boolean = true;

  /**
   * Информация о структуре API.
   * @type {ApiServiceStructure}
   * @memberof EntityCardListComponent
   */
  apiInfo: ApiServiceStructure = {} as ApiServiceStructure;

  /**
   * Флаг, указывающий, отсортированы ли сущности по возрастанию.
   * @type {boolean}
   * @default true
   * @memberof EntityCardListComponent
   */
  isSortedAscending: boolean = true;

  /**
   * Текущая страница пагинации.
   * @type {number}
   * @default 1
   * @memberof EntityCardListComponent
   */
  currentPage: number = 1;

  /**
   * Количество элементов на странице.
   * @type {number}
   * @default 16
   * @memberof EntityCardListComponent
   */
  itemsPerPage: number = 16;

  /**
   * Флаг, указывающий, активен ли поисковый запрос.
   * @type {boolean}
   * @default false
   * @memberof EntityCardListComponent
   */
  searchQueryActive = false;

  /**
   * Диалог для создания новой сущности.
   * @type {tuiDialog}
   * @memberof EntityCardListComponent
   */
  private readonly dialog = tuiDialog(EntityDialogComponent, {
    dismissible: true,
    label: 'Создать',
  });

  /**
   * Объект для создания новой сущности.
   * @type {Entity}
   * @memberof EntityCardListComponent
   */
  entity: Entity = {
    name: '',
    isActive: false,
    structure: null,
    endpoints: [],
  };

  /**
   * Конструктор компонента.
   * @param {ChangeDetectorRef} cd - Ссылка на детектор изменений для ручного обнаружения изменений.
   * @param {ActivatedRoute} route - Активированный маршрут для получения параметров маршрута.
   * @param {Router} router - Роутер для навигации между представлениями.
   * @param {ApiService} apiService - Сервис для работы с API.
   * @param {EntityRepositoryService} entityRepositoryService - Сервис для работы с сущностями.
   * @param {TuiAlertService} alerts - Сервис уведомлений для отображения уведомлений.
   */
  constructor(
    private cd: ChangeDetectorRef,
    private route: ActivatedRoute,
    private router: Router,
    private apiService: ApiService,
    private entityRepositoryService: EntityRepositoryService,
    private alerts: TuiAlertService
  ) {}

  /**
   * Метод жизненного цикла, который вызывается при уничтожении компонента.
   * Отписывается от всех активных подписок, чтобы предотвратить утечки памяти.
   *
   * @memberof EntityCardListComponent
   */
  ngOnDestroy(): void {
    this.sub?.unsubscribe();
  }

  /**
   * Метод жизненного цикла, который вызывается при инициализации компонента.
   * Загружает данные о сущностях.
   *
   * @memberof EntityCardListComponent
   */
  ngOnInit(): void {
    this.loadData();
  }

  /**
   * Обрабатывает изменение состояния активности API.
   *
   * @param {boolean} newState - Новое состояние активности.
   * @memberof EntityCardListComponent
   */
  onToggleChange(newState: boolean): void {
    this.updateApiServiceStatus(newState);
  }

  /**
   * Открывает диалог создания новой сущности.
   *
   * @memberof EntityCardListComponent
   */
  openCreateDialog(): void {
    this.dialog({ ...this.entity }).subscribe({
      next: (data) => this.handleCreateDialogData(data),
      complete: () => console.info('Dialog closed'),
    });
  }

  /**
   * Обрабатывает удаление сущности.
   *
   * @param {string} entityName - Имя удаленной сущности.
   * @memberof EntityCardListComponent
   */
  onEntityDeleted(entityName: string): void {
    this.entities = this.entities.filter(
      (entity) => entity.name !== entityName
    );
    this.filterEntities();
    this.cd.markForCheck();
  }

  /**
   * Загружает данные о сущностях.
   *
   * @private
   * @memberof EntityCardListComponent
   */
  private loadData(): void {
    this.sub = this.route.params
      .pipe(switchMap((params) => this.fetchApiData(params['name'])))
      .subscribe({
        next: (apiStructure) => this.handleApiStructureResponse(apiStructure),
        error: () => {
          this.loading = false;
          this.cd.markForCheck();
        },
      });
  }

  /**
   * Получает данные о структуре API.
   *
   * @private
   * @param {string} apiName - Имя API.
   * @returns {Observable<ApiServiceStructure>} - Наблюдаемый объект с данными о структуре API.
   * @memberof EntityCardListComponent
   */
  private fetchApiData(apiName: string): Observable<ApiServiceStructure> {
    if (!apiName) {
      throw new Error('API name is null');
    }
    this.apiName = apiName;
    return this.apiService.getApiStructureList(this.apiName);
  }

  /**
   * Обрабатывает ответ с данными о структуре API.
   *
   * @private
   * @param {ApiServiceStructure} apiStructure - Данные о структуре API.
   * @memberof EntityCardListComponent
   */
  private handleApiStructureResponse(apiStructure: ApiServiceStructure): void {
    this.apiInfo = apiStructure;
    this.entities = apiStructure.entities;
    this.filterEntities();
    this.loading = false;
    this.cd.markForCheck();
  }

  /**
   * Обновляет состояние активности API.
   *
   * @private
   * @param {boolean} newState - Новое состояние активности.
   * @memberof EntityCardListComponent
   */
  private updateApiServiceStatus(newState: boolean): void {
    this.apiInfo.isActive = newState;
    this.apiService.updateApiServiceStatus(this.apiName, newState).subscribe({
      next: (response) => console.log('Состояние сервиса обновлено:', response),
      error: () => {
        this.loading = false;
        this.cd.markForCheck();
      },
    });
  }

  /**
   * Обрабатывает данные из диалога создания сущности.
   *
   * @private
   * @param {Entity} data - Данные новой сущности.
   * @memberof EntityCardListComponent
   */
  private handleCreateDialogData(data: Entity): void {
    if (this.isEntityNameExists(data.name)) {
      this.alerts
        .open('Ошибка: Сущность с таким именем уже существует', {
          appearance: 'negative',
        })
        .subscribe();
      return;
    }
    this.createEntity(data);
  }

  /**
   * Проверяет, существует ли сущность с указанным именем.
   *
   * @private
   * @param {string} name - Имя сущности.
   * @returns {boolean} - Существует ли сущность с указанным именем.
   * @memberof EntityCardListComponent
   */
  private isEntityNameExists(name: string): boolean {
    return this.entities.some((entity) => entity.name === name);
  }

  /**
   * Создает новую сущность.
   *
   * @private
   * @param {Entity} data - Данные новой сущности.
   * @memberof EntityCardListComponent
   */
  private createEntity(data: Entity): void {
    this.entityRepositoryService.createApiEntity(this.apiName, data).subscribe({
      next: (response) => this.handleEntityCreation(response, data),
      error: (error) => {
        this.loading = false;
        this.cd.markForCheck();
        this.handleError(error);
      },
    });
  }

  /**
   * Обрабатывает успешное создание сущности.
   *
   * @private
   * @param {Entity} response - Ответ сервера.
   * @param {Entity} data - Данные новой сущности.
   * @memberof EntityCardListComponent
   */
  private handleEntityCreation(response: Entity, data: Entity): void {
    console.log('Сущность добавлена:', response);
    this.entities.push(data);
    this.filterEntities();
    this.sortCards();
    this.cd.markForCheck();
    this.alerts
      .open('Сущность успешно создана', { appearance: 'success' })
      .subscribe();
  }

  private handleError(error: any): void {
    const userMessage = 'Произошла ошибка. Пожалуйста, попробуйте снова позже. 😊';
    this.alerts.open(userMessage, { appearance: 'negative' }).subscribe();
    this.loading = false;
    this.cd.markForCheck();
  }

  sortCards(): void {
    if (this.isSortedAscending) {
      this.filteredEntities.sort((a, b) => a.name.localeCompare(b.name));
    } else {
      this.filteredEntities.sort((a, b) => b.name.localeCompare(a.name));
    }
  }

  /**
   * Переключает сортировку сущностей.
   *
   * @memberof EntityCardListComponent
   */
  sortCardsOnClick(): void {
    this.isSortedAscending = !this.isSortedAscending;
    this.sortCards();
    this.cd.markForCheck();
  }

  /**
   * Обрабатывает поисковый запрос.
   *
   * @param {string} query - Поисковый запрос.
   * @memberof EntityCardListComponent
   */
  onSearchQuery(query: string): void {
    this.searchQueryActive = !!query;
    this.filteredEntities = this.entities.filter((entity) =>
      entity.name.includes(query)
    );
    this.sortCards();
    this.updatePagination();
  }

  /**
   * Фильтрует сущности по запросу.
   *
   * @private
   * @param {string} [query=''] - Поисковый запрос.
   * @memberof EntityCardListComponent
   */
  private filterEntities(query: string = ''): void {
    this.filteredEntities = this.entities.filter((entity) =>
      entity.name.includes(query)
    );
    this.entityNames = this.filteredEntities.map((entity) => entity.name);
    this.updatePagination();
  }

  /**
   * Возвращает общее количество страниц.
   *
   * @type {number}
   * @memberof EntityCardListComponent
   */
  get totalPages(): number {
    return Math.ceil(this.filteredEntities.length / this.itemsPerPage);
  }

  /**
   * Возвращает отфильтрованные сущности для текущей страницы.
   *
   * @type {Entity[]}
   * @memberof EntityCardListComponent
   */
  get paginatedEntities(): Entity[] {
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    return this.filteredEntities.slice(
      startIndex,
      startIndex + this.itemsPerPage
    );
  }

  /**
   * Обрабатывает изменение страницы.
   *
   * @param {number} page - Номер страницы.
   * @memberof EntityCardListComponent
   */
  onPageChange(page: number): void {
    this.currentPage = page;
  }

  /**
   * Обновляет пагинацию.
   *
   * @private
   * @memberof EntityCardListComponent
   */
  private updatePagination(): void {
    if (this.currentPage > this.totalPages) {
      this.currentPage = this.totalPages;
    }
    if (this.currentPage < 1) {
      this.currentPage = 1;
    }
  }
}

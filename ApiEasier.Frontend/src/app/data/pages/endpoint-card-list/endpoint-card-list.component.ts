import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  OnDestroy,
  OnInit,
} from '@angular/core';
import { TuiCardLarge } from '@taiga-ui/layout';
import { Subscription } from 'rxjs';
import { CardEndpointComponent } from '../../components/card-endpoint/card-endpoint.component';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { HeaderComponent } from '../../components/header/header.component';
import { SwitchComponent } from '../../components/switch/switch.component';
import { LoadingComponent } from '../../components/loading/loading.component';
import { Endpoint } from '../../../interfaces/Endpoint';
import { Entity } from '../../../interfaces/Entity';
import { EndpointDialogComponent } from '../../components/endpoint-dialog/endpoint-dialog.component';
import { TuiAlertService, tuiDialog } from '@taiga-ui/core';
import { EndpointRepositoryService } from '../../../repositories/endpoint-repository.service';
import { EntityRepositoryService } from '../../../repositories/entity-repository.service';
import { CommonModule } from '@angular/common';
/**
 * Компонент EndpointCardListComponent отвечает за отображение списка конечных точек (эндпоинтов)
 * для выбранного API и сущности. Поддерживает создание, удаление и обновление состояния эндпоинтов.
 *
 * @remarks
 * Этот компонент использует реактивные формы для управления состоянием эндпоинтов.
 *
 * @example
 * HTML:
 * ```html
 * <app-endpoint-card-list></app-endpoint-card-list>
 */
@Component({
  selector: 'app-endpoint-card-list',
  imports: [
    CommonModule,
    TuiCardLarge,
    CardEndpointComponent,
    RouterModule,
    HeaderComponent,
    SwitchComponent,
    LoadingComponent,
  ],
  templateUrl: './endpoint-card-list.component.html',
  styleUrls: [
    './endpoint-card-list.component.css',
    '../../styles/card-list.css',
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class EndpointCardListComponent implements OnInit, OnDestroy {
  /**
   * Имя API, для которого отображаются эндпоинты.
   * @type {string}
   * @memberof EndpointCardListComponent
   */
  apiName!: string;

  /**
   * Имя сущности, для которой отображаются эндпоинты.
   * @type {string}
   * @memberof EndpointCardListComponent
   */
  entityName!: string;

  /**
   * Флаг, указывающий, загружаются ли данные в данный момент.
   * @type {boolean}
   * @default true
   * @memberof EndpointCardListComponent
   */
  loading: boolean = true;

  /**
   * Подписка для управления процессом получения данных.
   * @type {Subscription | null}
   * @memberof EndpointCardListComponent
   */
  private subscription: Subscription | null = null;

  /**
   * Список эндпоинтов.
   * @type {Endpoint[]}
   * @memberof EndpointCardListComponent
   */
  endpoints: Endpoint[] = [];

  /**
   * Информация о сущности.
   * @type {Entity}
   * @memberof EndpointCardListComponent
   */
  entityInfo: Entity = {} as Entity;

  /**
   * Сервис для работы с местоположением.
   * @type {Location}
   * @memberof EndpointCardListComponent
   */
  location: Location;

  /**
   * Диалог для создания нового эндпоинта.
   * @type {tuiDialog}
   * @memberof EndpointCardListComponent
   */
  private readonly dialog = tuiDialog(EndpointDialogComponent, {
    dismissible: true,
    label: 'Создать',
  });

  /**
   * Объект для создания нового эндпоинта.
   * @type {Endpoint}
   * @memberof EndpointCardListComponent
   */
  endpoint: Endpoint = {
    route: '',
    type: 'get',
    isActive: false,
  };

  /**
   * Конструктор компонента.
   * @param {ChangeDetectorRef} cd - Ссылка на детектор изменений для ручного обнаружения изменений.
   * @param {ActivatedRoute} route - Активированный маршрут для получения параметров маршрута.
   * @param {EndpointRepositoryService} endpointRepositoryService - Сервис для работы с эндпоинтами.
   * @param {EntityRepositoryService} entityRepositoryService - Сервис для работы с сущностями.
   * @param {TuiAlertService} alerts - Сервис уведомлений для отображения уведомлений.
   * @param {Location} location - Сервис для работы с местоположением.
   */
  constructor(
    private cd: ChangeDetectorRef,
    private route: ActivatedRoute,
    private endpointRepositoryService: EndpointRepositoryService,
    private entityRepositoryService: EntityRepositoryService,
    private alerts: TuiAlertService,
    location: Location
  ) {
    this.location = location;
  }

  /**
   * Метод жизненного цикла, который вызывается при уничтожении компонента.
   * Отписывается от всех активных подписок, чтобы предотвратить утечки памяти.
   *
   * @memberof EndpointCardListComponent
   */
  ngOnDestroy(): void {
    this.subscription?.unsubscribe();
  }

  /**
   * Метод жизненного цикла, который вызывается при инициализации компонента.
   * Подписывается на параметры маршрута и загружает данные.
   *
   * @memberof EndpointCardListComponent
   */
  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.apiName = params['apiServiceName'];
      this.entityName = params['entityName'];
      this.loadData();
    });
  }

  /**
   * Загружает данные о сущности и её эндпоинтах.
   *
   * @private
   * @memberof EndpointCardListComponent
   */
  private loadData(): void {
    this.subscription = this.entityRepositoryService
      .getApiEntity(this.apiName, this.entityName)
      .subscribe({
        next: (entity) => this.handleEntityInfoResponse(entity),
        error: () => {
          this.loading = false;
          this.cd.markForCheck();
        },
      });
  }

  /**
   * Обрабатывает ответ с информацией о сущности.
   *
   * @private
   * @param {Entity} entity - Информация о сущности.
   * @memberof EndpointCardListComponent
   */
  private handleEntityInfoResponse(entity: Entity): void {
    this.entityInfo = entity;
    this.endpoints = entity.endpoints || [];
    this.loading = false;
    this.cd.detectChanges();
  }

  /**
   * Открывает диалог создания нового эндпоинта.
   *
   * @memberof EndpointCardListComponent
   */
  openCreateDialog(): void {
    this.dialog({ ...this.endpoint }).subscribe({
      next: (data) => this.processCreateDialogData(data),
      complete: () => console.info('Dialog closed'),
    });
  }

  /**
   * Обрабатывает данные из диалога создания эндпоинта.
   *
   * @private
   * @param {Endpoint} data - Данные нового эндпоинта.
   * @memberof EndpointCardListComponent
   */
  private processCreateDialogData(data: Endpoint): void {
    if (this.isRouteExists(data.route)) {
      this.showRouteExistsError();
      return;
    }

    this.createEndpoint(data);
  }

  /**
   * Проверяет, существует ли эндпоинт с указанным маршрутом.
   *
   * @private
   * @param {string} route - Маршрут эндпоинта.
   * @returns {boolean} - Существует ли эндпоинт с указанным маршрутом.
   * @memberof EndpointCardListComponent
   */
  private isRouteExists(route: string): boolean {
    return this.endpoints.some((endpoint) => endpoint.route === route);
  }

  /**
   * Показывает ошибку, если эндпоинт с указанным маршрутом уже существует.
   *
   * @private
   * @memberof EndpointCardListComponent
   */
  private showRouteExistsError(): void {
    this.alerts
      .open('Ошибка: Эндпоинт с таким маршрутом уже существует', {
        appearance: 'negative',
      })
      .subscribe();
  }

  /**
   * Создает новый эндпоинт.
   *
   * @private
   * @param {Endpoint} data - Данные нового эндпоинта.
   * @memberof EndpointCardListComponent
   */
  private createEndpoint(data: Endpoint): void {
    this.endpointRepositoryService
      .createEndpoint(this.apiName, this.entityName, data)
      .subscribe({
        next: (response) => this.handleEndpointCreation(response, data),
        error: () => {
          this.loading = false;
          this.cd.markForCheck();
        },
      });
  }

  /**
   * Обрабатывает успешное создание эндпоинта.
   *
   * @private
   * @param {Endpoint} response - Ответ сервера.
   * @param {Endpoint} data - Данные нового эндпоинта.
   * @memberof EndpointCardListComponent
   */
  private handleEndpointCreation(response: Endpoint, data: Endpoint): void {
    console.log('Эндпоинт добавлен:', response);
    this.endpoints.push(data);
    this.cd.markForCheck();
    this.alerts
      .open('Эндпоинт успешно создан', {
        appearance: 'success',
      })
      .subscribe();
  }

  /**
   * Обрабатывает изменение состояния активности сущности.
   *
   * @param {boolean} newState - Новое состояние активности.
   * @memberof EndpointCardListComponent
   */
  onToggleChange(newState: boolean): void {
    this.entityInfo.isActive = newState;
    this.updateEntityStatus(newState);
  }

  /**
   * Обновляет состояние активности сущности.
   *
   * @private
   * @param {boolean} newState - Новое состояние активности.
   * @memberof EndpointCardListComponent
   */
  private updateEntityStatus(newState: boolean): void {
    this.entityRepositoryService
      .updateEntityStatus(this.apiName, this.entityName, newState)
      .subscribe({
        next: (response) =>
          console.log('Состояние сервиса обновлено:', response),
        error: () => {
          this.loading = false;
          this.cd.markForCheck();
        },
      });
  }

  /**
   * Обрабатывает удаление эндпоинта.
   *
   * @param {string} endpointRoute - Маршрут удаленного эндпоинта.
   * @memberof EndpointCardListComponent
   */
  onEndpointDeleted(endpointRoute: string): void {
    this.endpoints = this.endpoints.filter(
      (endpoint) => endpoint.route !== endpointRoute
    );
    this.cd.markForCheck();
  }
}

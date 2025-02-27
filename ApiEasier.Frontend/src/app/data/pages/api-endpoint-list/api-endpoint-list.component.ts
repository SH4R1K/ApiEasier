import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  OnDestroy,
  OnInit,
} from '@angular/core';
import { Endpoint } from "../../../interfaces/Endpoint";
import { Entity } from "../../../interfaces/Entity";
import { ApiServiceStructure } from "../../../interfaces/ApiServiceStructure";
import { TuiAccordion } from '@taiga-ui/experimental';
import { Subscription } from 'rxjs';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { ApiService } from '../../../services/api-service.service';
import { LoadingComponent } from '../../components/loading/loading.component';
import { CommonModule } from '@angular/common';
import { TuiButton } from '@taiga-ui/core';
import { HeaderComponent } from '../../components/header/header.component';
import { TuiAlertService } from '@taiga-ui/core';
import { ApiServiceRepositoryService } from '../../../repositories/api-service-repository.service';
import { EndpointRepositoryService } from '../../../repositories/endpoint-repository.service';
import { EntityRepositoryService } from '../../../repositories/entity-repository.service';
import { SwitchComponent } from '../../components/switch/switch.component';

/**
 * Компонент ApiEndpointListComponent отвечает за отображение списка конечных точек API
 * и управление их состоянием активности. Взаимодействует с различными сервисами для получения
 * и обновления структуры API и их статусов.
 *
 * @remarks
 * Этот компонент использует реактивные формы для управления состоянием переключателей.
 * Поддерживает валидацию и может быть настроен для отображения иконок и изменения внешнего вида.
 *
 * @example
 * HTML:
 * ```html
 * <app-api-endpoint-list></app-api-endpoint-list>
 */
@Component({
  selector: 'app-api-endpoint-list',
  imports: [
    TuiAccordion,
    LoadingComponent,
    CommonModule,
    RouterModule,
    TuiButton,
    HeaderComponent,
    SwitchComponent,
  ],
  templateUrl: './api-endpoint-list.component.html',
  styleUrls: ['./api-endpoint-list.component.css', '../../styles/button.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ApiEndpointListComponent implements OnInit, OnDestroy {
  /**
   * Массив сущностей, полученных из структуры API.
   * @type {Entity[]}
   * @memberof ApiEndpointListComponent
   */
  entities: Entity[] = [];

  /**
   * Подписка для управления процессом получения структуры API.
   * @type {Subscription | null}
   * @memberof ApiEndpointListComponent
   */
  private sub: Subscription | null = null;

  /**
   * Флаг, указывающий, загружаются ли данные в данный момент.
   * @type {boolean}
   * @default true
   * @memberof ApiEndpointListComponent
   */
  loading: boolean = true;

  /**
   * Имя управляемого API.
   * @type {string}
   * @memberof ApiEndpointListComponent
   */
  apiName!: string;

  /**
   * Объект, содержащий состояние активности API.
   * @type {{ isActive: boolean }}
   * @memberof ApiEndpointListComponent
   */
  apiInfo: { isActive: boolean } = { isActive: false };

  /**
   * Флаг, указывающий, был ли URL скопирован в буфер обмена.
   * @type {string | null}
   * @memberof ApiEndpointListComponent
   */
  isCopied: string | null = null;

  /**
   * Конструктор компонента.
   * @param {ActivatedRoute} route - Активированный маршрут для получения параметров маршрута.
   * @param {ApiService} apiService - Сервис API для получения структуры API.
   * @param {ApiServiceRepositoryService} apiServiceRepository - Репозиторий сервиса для обновления статуса API.
   * @param {EntityRepositoryService} entityRepositoryService - Репозиторий сервиса для обновления статуса сущности.
   * @param {EndpointRepositoryService} endpointRepositoryService - Репозиторий сервиса для обновления статуса конечной точки.
   * @param {ChangeDetectorRef} cd - Ссылка на детектор изменений для ручного обнаружения изменений.
   * @param {TuiAlertService} alerts - Сервис уведомлений для отображения уведомлений.
   */
  constructor(
    private route: ActivatedRoute,
    private apiService: ApiService,
    private apiServiceRepository: ApiServiceRepositoryService,
    private entityRepositoryService: EntityRepositoryService,
    private endpointRepositoryService: EndpointRepositoryService,
    private cd: ChangeDetectorRef,
    private alerts: TuiAlertService
  ) {}

  /**
   * Метод жизненного цикла, который вызывается при уничтожении компонента.
   * Отписывается от всех активных подписок, чтобы предотвратить утечки памяти.
   *
   * @memberof ApiEndpointListComponent
   */
  ngOnDestroy(): void {
    this.sub?.unsubscribe();
  }

  /**
   * Метод жизненного цикла, который вызывается при инициализации компонента.
   * Подписывается на параметры маршрута и загружает структуру API.
   *
   * @memberof ApiEndpointListComponent
   */
  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.apiName = params['name'];
      if (this.apiName) {
        this.loadApiStructure();
      }
    });
  }

  /**
   * Загружает структуру API.
   *
   * @private
   * @memberof ApiEndpointListComponent
   */
  private loadApiStructure(): void {
    this.sub = this.apiService.getApiStructureList(this.apiName).subscribe({
      next: (apiStructure) => this.handleApiStructureResponse(apiStructure),
      error: () => {
        this.loading = false;
        this.cd.markForCheck();
      },
    });
  }

  /**
   * Обрабатывает ответ с структурой API.
   *
   * @private
   * @param {ApiServiceStructure} apiStructure - Структура API.
   * @memberof ApiEndpointListComponent
   */
  private handleApiStructureResponse(apiStructure: ApiServiceStructure): void {
    if (apiStructure) {
      this.entities = apiStructure.entities;
      this.apiInfo.isActive = apiStructure.isActive; // Устанавливает состояние API
      this.loading = false;
      this.cd.markForCheck();
    }
  }

  /**
   * Копирует URL в буфер обмена.
   *
   * @param {string} entityName - Имя сущности.
   * @param {Endpoint} endpoint - Конечная точка.
   * @memberof ApiEndpointListComponent
   */
  copyToClipboard(entityName: string, endpoint: Endpoint): void {
    const url = this.getUrl(entityName, endpoint);
    this.copyTextToClipboard(url);
  }

  /**
   * Копирует текст в буфер обмена.
   *
   * @private
   * @param {string} text - Текст для копирования.
   * @memberof ApiEndpointListComponent
   */
  private copyTextToClipboard(text: string): void {
    const textarea = document.createElement('textarea');
    textarea.value = text;
    document.body.appendChild(textarea);
    textarea.select();
    try {
      document.execCommand('copy');
      this.showCopySuccess(text);
    } catch (err) {
      console.error('Ошибка при копировании URL:', err);
    } finally {
      document.body.removeChild(textarea);
    }
  }

  /**
   * Показывает уведомление об успешном копировании URL.
   *
   * @private
   * @param {string} url - Скопированный URL.
   * @memberof ApiEndpointListComponent
   */
  private showCopySuccess(url: string): void {
    this.isCopied = url;
    this.cd.markForCheck();
    setTimeout(() => {
      this.isCopied = null;
      this.cd.markForCheck();
    }, 2000);
  }

  /**
   * Возвращает URL для сущности и конечной точки.
   *
   * @param {string} entityName - Имя сущности.
   * @param {Endpoint} endpoint - Конечная точка.
   * @returns {string} - Сформированный URL.
   * @memberof ApiEndpointListComponent
   */
  getUrl(entityName: string, endpoint: Endpoint): string {
    return `${window.location.origin}/api/ApiEmu/${this.apiName}/${entityName}/${endpoint.route}`;
  }

  /**
   * Обрабатывает изменение состояния активности API.
   *
   * @param {boolean} newState - Новое состояние активности.
   * @memberof ApiEndpointListComponent
   */
  onApiToggleChange(newState: boolean): void {
    this.apiInfo.isActive = newState;
    this.apiServiceRepository
      .updateApiServiceStatus(this.apiName, newState)
      .subscribe({
        next: (response) => {
          console.log('Состояние API обновлено:', response);
        },
        error: (error) => {
          console.error('Ошибка при обновлении состояния API:', error);
        },
      });
  }

  /**
   * Обрабатывает изменение состояния активности сущности.
   *
   * @param {Entity} entity - Сущность.
   * @param {boolean} newState - Новое состояние активности.
   * @memberof ApiEndpointListComponent
   */
  onEntityToggleChange(entity: Entity, newState: boolean): void {
    entity.isActive = newState;
    this.entityRepositoryService
      .updateEntityStatus(this.apiName, entity.name, newState)
      .subscribe({
        next: (response) => {
          console.log('Состояние сущности обновлено:', response);
        },
        error: (error) => {
          console.error('Ошибка при обновлении состояния сущности:', error);
        },
      });
  }

  /**
   * Обрабатывает изменение состояния активности конечной точки.
   *
   * @param {Entity} entity - Сущность.
   * @param {Endpoint} endpoint - Конечная точка.
   * @param {boolean} newState - Новое состояние активности.
   * @memberof ApiEndpointListComponent
   */
  onEndpointToggleChange(
    entity: Entity,
    endpoint: Endpoint,
    newState: boolean
  ): void {
    endpoint.isActive = newState;
    this.endpointRepositoryService
      .updateEndpointStatus(this.apiName, entity.name, endpoint.route, newState)
      .subscribe({
        next: (response) => {
          console.log('Состояние эндпоинта обновлено:', response);
        },
        error: (error) => {
          console.error('Ошибка при обновлении состояния эндпоинта:', error);
        },
      });
  }
}

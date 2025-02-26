import {
  ChangeDetectorRef,
  Component,
  EventEmitter,
  Input,
  Output,
} from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { Endpoint } from "../../../interfaces/Endpoint";
import { Entity } from "../../../interfaces/Entity";
import { apiServiceShortStructure } from "../../../interfaces/apiServiceShortStructure";
import { Subscription } from 'rxjs';
import { SwitchComponent } from '../switch/switch.component';
import { IconTrashComponent } from '../icon-trash/icon-trash.component';
import { tuiDialog, TuiAlertService } from '@taiga-ui/core';
import { EndpointDialogComponent } from '../endpoint-dialog/endpoint-dialog.component';
import { EndpointRepositoryService } from '../../../repositories/endpoint-repository.service';
import { CommonModule } from '@angular/common';

/**
 * Компонент CardEndpointComponent предназначен для отображения и управления информацией об эндпоинтах API.
 * Позволяет редактировать, удалять и изменять состояние эндпоинтов.
 *
 * @remarks
 * Этот компонент интегрируется с Taiga UI для создания интерактивного интерфейса.
 * Использует сервисы для взаимодействия с репозиторием эндпоинтов и управления состоянием.
 *
 * @example
 * html
 * <app-card-endpoint
 *   [entityInfo]="entityData"
 *   [endpointInfo]="endpointData"
 *   [apiName]="apiName"
 *   (endpointDeleted)="handleEndpointDeleted(\$event)">
 * </app-card-endpoint>
 */
@Component({
  selector: 'app-card-endpoint',
  imports: [SwitchComponent, IconTrashComponent, CommonModule, RouterModule],
  templateUrl: './card-endpoint.component.html',
  styleUrls: [
    './card-endpoint.component.css',
    '../../styles/card.css',
    '../../styles/icon.css',
  ],
})
export class CardEndpointComponent {
  /**
   * Входной параметр для получения информации о сущности.
   *
   * @type {Entity}
   * @memberof CardEndpointComponent
   */
  @Input() entityInfo!: Entity;

  /**
   * Входной параметр для получения информации об эндпоинте.
   *
   * @type {Endpoint}
   * @memberof CardEndpointComponent
   */
  @Input() endpointInfo!: Endpoint;

  /**
   * Входной параметр для получения имени API.
   *
   * @type {string}
   * @default ''
   * @memberof CardEndpointComponent
   */
  @Input() apiName: string = '';

  /**
   * Событие, которое вызывается при удалении эндпоинта.
   *
   * @type {EventEmitter<string>}
   * @memberof CardEndpointComponent
   */
  @Output() endpointDeleted = new EventEmitter<string>();

  /**
   * Диалог для редактирования информации об эндпоинте.
   *
   * @type {tuiDialog}
   * @memberof CardEndpointComponent
   */
  private readonly dialog = tuiDialog(EndpointDialogComponent, {
    dismissible: true,
    label: 'Редактировать',
  });

  /**
   * Конструктор компонента CardEndpointComponent.
   *
   * @param endpointRepositoryService - Сервис для взаимодействия с репозиторием эндпоинтов.
   * @param cd - Сервис для управления изменением состояния.
   * @param alerts - Сервис для отображения уведомлений.
   *
   * @memberof CardEndpointComponent
   */
  constructor(
    private endpointRepositoryService: EndpointRepositoryService,
    private cd: ChangeDetectorRef,
    private alerts: TuiAlertService
  ) {}

  /**
   * Обработчик изменения состояния переключателя.
   *
   * @param newState - Новое состояние переключателя.
   * @remarks
   * Обновляет состояние эндпоинта и сохраняет изменения в репозитории.
   *
   * @memberof CardEndpointComponent
   */
  onToggleChange(newState: boolean): void {
    this.endpointInfo.isActive = newState;
    console.log('Состояние переключателя изменилось на:', newState);
    this.endpointRepositoryService
      .updateEndpointStatus(
        this.apiName,
        this.entityInfo.name,
        this.endpointInfo.route,
        newState
      )
      .subscribe({
        next: (response) => {
          console.log('Состояние сервиса обновлено:', response);
        },
        error: (error) => {
          console.error('Ошибка при обновлении состояния сервиса:', error);
        },
      });
  }

  /**
   * Открывает диалог для редактирования информации об эндпоинте.
   *
   * @remarks
   * Обновляет информацию об эндпоинте после закрытия диалога.
   *
   * @memberof CardEndpointComponent
   */
  openEditDialog(): void {
    this.dialog({ ...this.endpointInfo }).subscribe({
      next: (data) => this.processEditDialogData(data),
      complete: () => console.info('Dialog closed'),
    });
  }

  /**
   * Обрабатывает данные, полученные из диалога редактирования.
   *
   * @param data - Данные эндпоинта, полученные из диалога.
   * @private
   * @memberof CardEndpointComponent
   */
  private processEditDialogData(data: Endpoint): void {
    console.info(`Dialog emitted data = ${data} - ${this.apiName}`);
    this.updateEndpoint(data);
  }

  /**
   * Обновляет информацию об эндпоинте в репозитории.
   *
   * @param data - Новые данные эндпоинта.
   * @private
   * @memberof CardEndpointComponent
   */
  private updateEndpoint(data: Endpoint): void {
    this.endpointRepositoryService
      .updateEndpoint(
        this.apiName,
        this.entityInfo.name,
        this.endpointInfo.route,
        data
      )
      .subscribe({
        next: (response) => this.handleEndpointUpdate(response, data),
        error: (error) => this.handleEndpointUpdateError(error),
      });
  }

  /**
   * Обрабатывает успешное обновление эндпоинта.
   *
   * @param response - Ответ от сервера.
   * @param data - Новые данные эндпоинта.
   * @private
   * @memberof CardEndpointComponent
   */
  private handleEndpointUpdate(response: Endpoint, data: Endpoint): void {
    console.log('Эндпоинт обновлен:', response);
    this.endpointInfo = data;
    this.cd.markForCheck();
    this.alerts
      .open('Эндпоинт успешно обновлен', {
        appearance: 'success',
      })
      .subscribe();
  }

  /**
   * Обрабатывает ошибку при обновлении эндпоинта.
   *
   * @param error - Объект ошибки.
   * @private
   * @memberof CardEndpointComponent
   */
  private handleEndpointUpdateError(error: any): void {
    if (error.status === 409) {
      this.alerts
        .open('Ошибка: Эндпоинт с таким именем уже существует', {
          appearance: 'negative',
        })
        .subscribe();
    } else {
      this.alerts
        .open('Ошибка при обновлении эндпоинта', {
          appearance: 'negative',
        })
        .subscribe();
    }
    console.error('Ошибка при обновлении эндпоинта:', error);
  }

  /**
   * Обработчик подтверждения удаления эндпоинта.
   *
   * @remarks
   * Удаляет эндпоинт из репозитория и уведомляет родительский компонент об удалении.
   *
   * @memberof CardEndpointComponent
   */
  onDeleteConfirmed(): void {
    this.endpointRepositoryService
      .deleteEndpoint(
        this.apiName,
        this.entityInfo.name,
        this.endpointInfo.route
      )
      .subscribe({
        next: () => {
          console.log(`Действие "${this.endpointInfo.route}" удалено.`);
          this.endpointDeleted.emit(this.endpointInfo.route);
        },
        error: (error) => {
          console.error('Ошибка при удалении действия:', error);
        },
      });
  }
}

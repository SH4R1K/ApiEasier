import {
  ChangeDetectorRef,
  Component,
  EventEmitter,
  Input,
  Output,
} from '@angular/core';
import { Entity } from "../../../interfaces/Entity";
import { RouterModule } from '@angular/router';
import { IconTrashComponent } from '../icon-trash/icon-trash.component';
import { SwitchComponent } from '../switch/switch.component';
import { Subscription } from 'rxjs';
import { tuiDialog, TuiAlertService } from '@taiga-ui/core';
import { EntityDialogComponent } from '../entity-dialog/entity-dialog.component';
import { CommonModule } from '@angular/common';
import { EntityRepositoryService } from '../../../repositories/entity-repository.service';

/**
 * Компонент CardEntityComponent предназначен для отображения и управления информацией о сущностях API.
 * Позволяет редактировать, удалять и изменять состояние сущностей.
 *
 * @remarks
 * Этот компонент интегрируется с Taiga UI для создания интерактивного интерфейса.
 * Использует сервисы для взаимодействия с репозиторием сущностей и управления состоянием.
 *
 * @example
 * html
 * <app-card-entity
 *   [entityInfo]="entityData"
 *   [apiName]="apiName"
 *    (entityDeleted)="onEntityDeleted(item.name)">
 * </app-card-entity>
 * 
 */
@Component({
  selector: 'app-card-entity',
  imports: [IconTrashComponent, SwitchComponent, CommonModule, RouterModule],
  templateUrl: './card-entity.component.html',
  styleUrls: [
    './card-entity.component.css',
    '../../styles/card.css',
    '../../styles/button.css',
    '../../styles/icon.css',
  ],
})
export class CardEntityComponent {
  /**
   * Входной параметр для получения информации о сущности.
   *
   * @type {Entity}
   * @memberof CardEntityComponent
   */
  @Input() entityInfo!: Entity;

  /**
   * Входной параметр для получения имени API.
   *
   * @type {string}
   * @default ''
   * @memberof CardEntityComponent
   */
  @Input() apiName: string = '';

  /**
   * Событие, которое вызывается при удалении сущности.
   *
   * @type {EventEmitter<string>}
   * @memberof CardEntityComponent
   */
  @Output() entityDeleted = new EventEmitter<string>();

  private oldName: string = '';
  private sub: Subscription | null = null;
  private loading: boolean = false;

  /**
   * Диалог для редактирования информации о сущности.
   *
   * @type {tuiDialog}
   * @memberof CardEntityComponent
   */
  private readonly dialog = tuiDialog(EntityDialogComponent, {
    dismissible: true,
    label: 'Редактировать',
  });

  /**
   * Конструктор компонента CardEntityComponent.
   *
   * @param cd - Сервис для управления изменением состояния.
   * @param entityRepositoryService - Сервис для взаимодействия с репозиторием сущностей.
   * @param alerts - Сервис для отображения уведомлений.
   *
   * @memberof CardEntityComponent
   */
  constructor(
    private cd: ChangeDetectorRef,
    private entityRepositoryService: EntityRepositoryService,
    private alerts: TuiAlertService
  ) {}

  /**
   * Обработчик изменения состояния переключателя.
   *
   * @param newState - Новое состояние переключателя.
   * @remarks
   * Обновляет состояние сущности и сохраняет изменения в репозитории.
   *
   * @memberof CardEntityComponent
   */
  onToggleChange(newState: boolean): void {
    this.entityInfo.isActive = newState;
    this.entityRepositoryService
      .updateEntityStatus(this.apiName, this.entityInfo.name, newState)
      .subscribe({
        next: (response) =>
          console.log('Состояние сервиса обновлено:', response),
        error: (error) =>
          this.handleError('Ошибка при обновлении состояния сервиса', error),
      });
  }

  /**
   * Открывает диалог для редактирования информации о сущности.
   *
   * @remarks
   * Обновляет информацию о сущности после закрытия диалога.
   *
   * @memberof CardEntityComponent
   */
  openEditDialog(): void {
    this.oldName = this.entityInfo.name;
    this.dialog({ ...this.entityInfo }).subscribe({
      next: (data) => this.handleEditDialogData(data),
      complete: () => console.info('Dialog closed'),
    });
  }

  /**
   * Обработчик подтверждения удаления сущности.
   *
   * @remarks
   * Удаляет сущность из репозитория и уведомляет родительский компонент об удалении.
   *
   * @memberof CardEntityComponent
   */
  onDeleteConfirmed(): void {
    this.entityRepositoryService
      .deleteApiEntity(this.apiName, this.entityInfo.name)
      .subscribe({
        next: () => this.handleEntityDeletion(),
        error: (error) =>
          this.handleError('Ошибка при удалении сущности', error),
      });
  }

  /**
   * Обрабатывает данные, полученные из диалога редактирования.
   *
   * @param data - Данные сущности, полученные из диалога.
   * @private
   * @memberof CardEntityComponent
   */
  private handleEditDialogData(data: Entity): void {
    this.entityRepositoryService
      .updateApiEntity(this.apiName, this.oldName, data)
      .subscribe({
        next: (response) => this.handleEntityUpdate(response, data),
        error: (error) => this.handleEntityUpdateError(error),
      });
  }

  /**
   * Обрабатывает успешное обновление сущности.
   *
   * @param response - Ответ от сервера.
   * @param data - Новые данные сущности.
   * @private
   * @memberof CardEntityComponent
   */
  private handleEntityUpdate(response: Entity, data: Entity): void {
    console.log('Сущность обновлена:', response);
    this.entityInfo = data;
    this.cd.markForCheck();
    this.alerts
      .open('Сущность успешно обновлена', { appearance: 'success' })
      .subscribe();
  }

  /**
   * Обрабатывает ошибку при обновлении сущности.
   *
   * @param error - Объект ошибки.
   * @private
   * @memberof CardEntityComponent
   */
  private handleEntityUpdateError(error: any): void {
    this.handleError('Ошибка при обновлении сущности', error);
  }

  /**
   * Обрабатывает удаление сущности.
   *
   * @private
   * @memberof CardEntityComponent
   */
  private handleEntityDeletion(): void {
    console.log(`Сущность "${this.entityInfo.name}" удалена.`);
    this.entityDeleted.emit(this.entityInfo.name);
  }

  /**
   * Обрабатывает ошибки и отображает уведомления.
   *
   * @param message - Сообщение об ошибке.
   * @param error - Объект ошибки.
   * @private
   * @memberof CardEntityComponent
   */
  private handleError(message: string, error: any): void {
    console.error(message, error);
    if (error.status === 409) {
      this.alerts
        .open(`${message}: Сущность с таким именем уже существует`, {
          appearance: 'negative',
        })
        .subscribe();
    } else {
      this.alerts.open(message, { appearance: 'negative' }).subscribe();
    }
  }
}

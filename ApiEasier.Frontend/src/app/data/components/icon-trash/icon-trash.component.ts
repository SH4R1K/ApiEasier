import { Component, EventEmitter, Input, Output } from '@angular/core';
import { TuiAlertService } from '@taiga-ui/core';
import { switchMap, takeUntil } from 'rxjs';
import { Router } from '@angular/router';
import { PolymorpheusComponent } from '@taiga-ui/polymorpheus';
import { AlertDeleteComponent } from '../alert-delete/alert-delete.component';
import { Endpoint } from "../../../interfaces/Endpoint";
import { apiServiceShortStructure } from "../../../interfaces/apiServiceShortStructure";
import { EntityShort } from "../../../interfaces/EntityShort";

/**
 * Компонент IconTrashComponent предназначен для отображения иконки корзины и управления удалением элементов.
 * Позволяет пользователю подтверждать удаление элементов с помощью уведомлений.
 *
 * @remarks
 * Этот компонент интегрируется с Taiga UI для создания интерактивного интерфейса.
 * Использует сервисы для управления уведомлениями и маршрутизацией.
 *
 * @example
 * html
 * <app-icon-trash
 *   [item]="itemData"
 *   [apiInfo]="apiData"
 *   [entityInfo]="entityData"
 *   [endpointInfo]="endpointData"
 *   (responseAlert)="handleResponseAlert(\$event)">
 * </app-icon-trash>
 */
@Component({
  selector: 'app-icon-trash',
  imports: [],
  templateUrl: './icon-trash.component.html',
  styleUrls: ['./icon-trash.component.css', '../../styles/icon.css'],
})
export class IconTrashComponent {
  /**
   * Элемент, который будет удален.
   *
   * @type {any}
   * @memberof IconTrashComponent
   */
  @Input() item: any;

  /**
   * Информация об API, связанная с элементом.
   *
   * @type {apiServiceShortStructure}
   * @memberof IconTrashComponent
   */
  @Input() apiInfo!: apiServiceShortStructure;

  /**
   * Информация о сущности, связанная с элементом.
   *
   * @type {EntityShort}
   * @memberof IconTrashComponent
   */
  @Input() entityInfo!: EntityShort;

  /**
   * Информация об эндпоинте, связанная с элементом.
   *
   * @type {Endpoint}
   * @memberof IconTrashComponent
   */
  @Input() endpointInfo!: Endpoint;

  /**
   * Событие, которое вызывается при подтверждении удаления элемента.
   *
   * @type {EventEmitter<boolean>}
   * @memberof IconTrashComponent
   */
  @Output() responseAlert = new EventEmitter<boolean>();

  /**
   * Конструктор компонента IconTrashComponent.
   *
   * @param alerts - Сервис для отображения уведомлений.
   * @param router - Сервис для управления маршрутизацией.
   *
   * @memberof IconTrashComponent
   */
  constructor(
    private alerts: TuiAlertService,
    private router: Router,
  ) {}

  /**
   * Отображает уведомление для подтверждения удаления элемента.
   *
   * @remarks
   * Использует сервис TuiAlertService для отображения уведомлений.
   * Подписывается на события маршрутизации для автоматического закрытия уведомлений.
   *
   * @memberof IconTrashComponent
   */
  protected showNotification(): void {
    const notification = this.alerts
      .open<boolean>(new PolymorpheusComponent(AlertDeleteComponent), {
        label: 'Вы уверены, что хотите удалить?',
        appearance: 'negative',
        autoClose: 0,
      })
      .pipe(
        switchMap((response) => {
          if (response) {
            this.responseAlert.emit(true);
            console.log(`Удаление карточки: ${this.item.name}`);
            return this.alerts.open(`Карточка "${this.item.name}" удалена.`, { label: 'Успех' });
          } else {
            return this.alerts.open(`Удаление карточки "${this.item.name}" отменено.`, { label: 'Информация' });
          }
        }),
        takeUntil(this.router.events),
      );

    notification.subscribe();
  }
}

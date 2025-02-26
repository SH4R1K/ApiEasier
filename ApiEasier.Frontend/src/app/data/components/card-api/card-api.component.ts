import { CommonModule } from '@angular/common';
import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  EventEmitter,
  Input,
  Output,
} from '@angular/core';
import {
  TuiAppearance,
  TuiButton,
  tuiDialog,
  TuiAlertService,
} from '@taiga-ui/core';
import { RouterModule } from '@angular/router';
import { apiServiceShortStructure } from "../../../interfaces/apiServiceShortStructure";
import { SwitchComponent } from '../switch/switch.component';
import { ApiDialogComponent } from '../api-dialog/api-dialog.component';
import { Location } from '@angular/common';
import { IconTrashComponent } from '../icon-trash/icon-trash.component';
import { ApiServiceRepositoryService } from '../../../repositories/api-service-repository.service';
import { ExportApiButtonComponent } from '../export-api-button/export-api-button.component';

/**
 * Компонент CardApiComponent предназначен для отображения и управления информацией о API.
 * Позволяет редактировать, удалять и изменять состояние API.
 *
 * @remarks
 * Этот компонент интегрируется с Taiga UI для создания интерактивного интерфейса.
 * Использует сервисы для взаимодействия с репозиторием API и управления состоянием.
 *
 * @example
 * html
 * <app-card-api [apiInfo]="apiData" (apiDeleted)="handleApiDeleted()"></app-card-api>
 *
 */
@Component({
  selector: 'app-card-api',
  imports: [
    CommonModule,
    TuiAppearance,
    TuiButton,
    RouterModule,
    SwitchComponent,
    IconTrashComponent,
    ExportApiButtonComponent,
  ],
  templateUrl: './card-api.component.html',
  styleUrls: [
    './card-api.component.css',
    '../../styles/card.css',
    '../../styles/button.css',
    '../../styles/icon.css',
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CardApiComponent {
  /**
   * Входной параметр для получения информации о API.
   *
   * @type {apiServiceShortStructure}
   * @memberof CardApiComponent
   */
  @Input() apiInfo!: apiServiceShortStructure;

  /**
   * Событие, которое вызывается при удалении API.
   *
   * @type {EventEmitter<void>}
   * @memberof CardApiComponent
   */
  @Output() apiDeleted = new EventEmitter<void>();

  oldName: string = '';
  location: Location;

  /**
   * Диалог для редактирования информации о API.
   *
   * @type {tuiDialog}
   * @memberof CardApiComponent
   */
  private readonly dialog = tuiDialog(ApiDialogComponent, {
    dismissible: true,
    label: 'Редактировать',
  });

  /**
   * Конструктор компонента CardApiComponent.
   *
   * @param apiServiceRepository - Сервис для взаимодействия с репозиторием API.
   * @param cd - Сервис для управления изменением состояния.
   * @param alerts - Сервис для отображения уведомлений.
   * @param location - Сервис для работы с историей навигации.
   *
   * @memberof CardApiComponent
   */
  constructor(
    private apiServiceRepository: ApiServiceRepositoryService,
    private cd: ChangeDetectorRef,
    private alerts: TuiAlertService,
    location: Location
  ) {
    this.location = location;
  }

  /**
   * Обработчик изменения состояния переключателя.
   *
   * @param newState - Новое состояние переключателя.
   * @remarks
   * Обновляет состояние API и сохраняет изменения в репозитории.
   *
   * @memberof CardApiComponent
   */
  onToggleChange(newState: boolean) {
    this.apiInfo.isActive = newState;
    console.log('Состояние переключателя изменилось на:', newState);
    this.apiServiceRepository
      .updateApiServiceStatus(this.apiInfo.name, newState)
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
   * Открывает диалог для редактирования информации о API.
   *
   * @remarks
   * Сохраняет старое имя API и открывает диалог для редактирования.
   * Обновляет информацию о API после закрытия диалога.
   *
   * @memberof CardApiComponent
   */
  openEditDialog(): void {
    this.oldName = this.apiInfo.name;
    this.dialog({ ...this.apiInfo }).subscribe({
      next: (data) => {
        console.info(`Dialog emitted data = ${data} - ${this.apiInfo.name}}`);

        this.apiServiceRepository
          .updateApiService(this.oldName, data)
          .subscribe({
            next: (response) => {
              console.log('API обновлена:', response);
              this.apiInfo = data;
              this.cd.markForCheck();
              this.alerts
                .open('API успешно обновлено', {
                  appearance: 'success',
                })
                .subscribe();
            },
            error: (error) => {
              if (error.status === 409) {
                this.alerts
                  .open('Ошибка: API с таким именем уже существует', {
                    appearance: 'negative',
                  })
                  .subscribe();
              } else {
                this.alerts
                  .open('Ошибка при обновлении API', {
                    appearance: 'negative',
                  })
                  .subscribe();
              }
            },
          });
      },
      complete: () => {
        console.info('Dialog closed');
      },
    });
  }

  /**
   * Обработчик подтверждения удаления API.
   *
   * @remarks
   * Удаляет API из репозитория и уведомляет родительский компонент об удалении.
   *
   * @memberof CardApiComponent
   */
  onDeleteConfirmed(): void {
    this.apiServiceRepository.deleteApiService(this.apiInfo.name).subscribe({
      next: () => {
        console.log(`Сервис "${this.apiInfo.name}" удален.`);
        this.apiDeleted.emit();
      },
      error: (error) => {
        console.error('Ошибка при удалении сервиса:', error);
      },
    });
  }
}

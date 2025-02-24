import {
  ChangeDetectorRef,
  Component,
  EventEmitter,
  Input,
  Output,
} from '@angular/core';
import { Entity } from '../../../services/service-structure-api';
import { RouterModule } from '@angular/router';
import { IconTrashComponent } from '../icon-trash/icon-trash.component';
import { SwitchComponent } from '../switch/switch.component';
import { Subscription } from 'rxjs';
import { tuiDialog, TuiAlertService } from '@taiga-ui/core';
import { EntityDialogComponent } from '../entity-dialog/entity-dialog.component';
import { CommonModule } from '@angular/common';
import { EntityRepositoryService } from '../../../repositories/entity-repository.service';

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
  @Input() entityInfo!: Entity;
  @Input() apiName: string = '';
  @Output() entityDeleted = new EventEmitter<string>();

  private oldName: string = '';
  private sub: Subscription | null = null;
  private loading: boolean = false;

  private readonly dialog = tuiDialog(EntityDialogComponent, {
    dismissible: true,
    label: 'Редактировать',
  });

  constructor(
    private cd: ChangeDetectorRef,
    private entityRepositoryService: EntityRepositoryService,
    private alerts: TuiAlertService
  ) {}

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
  openEditDialog(): void {
    this.oldName = this.entityInfo.name;
    this.dialog({ ...this.entityInfo }).subscribe({
      next: (data) => this.handleEditDialogData(data),
      complete: () => console.info('Dialog closed'),
    });
  }

  onDeleteConfirmed(): void {
    this.entityRepositoryService
      .deleteApiEntity(this.apiName, this.entityInfo.name)
      .subscribe({
        next: () => this.handleEntityDeletion(),
        error: (error) =>
          this.handleError('Ошибка при удалении сущности', error),
      });
  }

 

  private handleEditDialogData(data: Entity): void {
    this.entityRepositoryService
      .updateApiEntity(this.apiName, this.oldName, data)
      .subscribe({
        next: (response) => this.handleEntityUpdate(response, data),
        error: (error) => this.handleEntityUpdateError(error),
      });
  }

  private handleEntityUpdate(response: Entity, data: Entity): void {
    console.log('Сущность обновлена:', response);
    this.entityInfo = data;
    this.cd.markForCheck();
    this.alerts
      .open('Сущность успешно обновлена', { appearance: 'success' })
      .subscribe();
  }

  private handleEntityUpdateError(error: any): void {
    this.handleError('Ошибка при обновлении сущности', error);
  }

  private handleEntityDeletion(): void {
    console.log(`Сущность "${this.entityInfo.name}" удалена.`);
    this.entityDeleted.emit(this.entityInfo.name);
  }

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

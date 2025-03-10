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
  @Input() entityInfo!: Entity;
  @Input() endpointInfo!: Endpoint;
  @Input() apiName: string = '';
  @Output() endpointDeleted = new EventEmitter<string>();

  private readonly dialog = tuiDialog(EndpointDialogComponent, {
    dismissible: true,
    label: 'Редактировать',
  });

  constructor(
    private endpointRepositoryService: EndpointRepositoryService,
    private cd: ChangeDetectorRef,
    private alerts: TuiAlertService
  ) {}

  onToggleChange(newState: boolean): void {
    this.endpointInfo.isActive = newState; // Update the state in the parent component
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

  openEditDialog(): void {
    this.dialog({ ...this.endpointInfo }).subscribe({
      next: (data) => this.processEditDialogData(data),
      complete: () => console.info('Dialog closed'),
    });
  }

  private processEditDialogData(data: Endpoint): void {
    console.info(`Dialog emitted data = ${data} - ${this.apiName}`);
    this.updateEndpoint(data);
  }

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

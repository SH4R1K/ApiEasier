import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  OnDestroy,
  OnInit,
} from '@angular/core';
import { Subscription } from 'rxjs';
import { Endpoint } from "../../../interfaces/Endpoint";
import { Entity } from "../../../interfaces/Entity";
import { CommonModule, Location } from '@angular/common';
import { TuiCardLarge } from '@taiga-ui/layout';
import { tuiDialog, TuiAlertService } from '@taiga-ui/core';
import { CardEndpointComponent } from '../../components/card-endpoint/card-endpoint.component';
import { HeaderComponent } from '../../components/header/header.component';
import { Router, RouterModule } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { EndpointDialogComponent } from '../../components/endpoint-dialog/endpoint-dialog.component';
import { SwitchComponent } from '../../components/switch/switch.component';
import { EntityRepositoryService } from '../../../repositories/entity-repository.service';
import { LoadingComponent } from '../../components/loading/loading.component';
import { EndpointRepositoryService } from '../../../repositories/endpoint-repository.service';

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
  apiName!: string;
  entityName!: string;
  loading: boolean = true;
  private subscription: Subscription | null = null;
  endpoints: Endpoint[] = [];
  entityInfo: Entity = {} as Entity;
  location: Location;

  private readonly dialog = tuiDialog(EndpointDialogComponent, {
    dismissible: true,
    label: 'Создать',
  });

  endpoint: Endpoint = {
    route: '',
    type: 'get',
    isActive: false,
  };

  constructor(
    private router: Router,
    private cd: ChangeDetectorRef,
    private route: ActivatedRoute,
    private endpointRepositoryService: EndpointRepositoryService,
    private entityRepositoryService: EntityRepositoryService,
    private alerts: TuiAlertService,
    location: Location
  ) {
    this.location = location;
  }

  ngOnDestroy(): void {
    this.subscription?.unsubscribe();
  }

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.apiName = params['apiServiceName'];
      this.entityName = params['entityName'];
      this.loadData();
    });
  }

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

  private handleEntityInfoResponse(entity: Entity): void {
    this.entityInfo = entity;
    this.endpoints = entity.endpoints || [];
    this.loading = false;
    this.cd.detectChanges();
  }

  openCreateDialog(): void {
    this.dialog({ ...this.endpoint }).subscribe({
      next: (data) => this.processCreateDialogData(data),
      complete: () => console.info('Dialog closed'),
    });
  }

  private processCreateDialogData(data: Endpoint): void {
    if (this.isRouteExists(data.route)) {
      this.showRouteExistsError();
      return;
    }

    this.createEndpoint(data);
  }

  private isRouteExists(route: string): boolean {
    return this.endpoints.some((endpoint) => endpoint.route === route);
  }

  private showRouteExistsError(): void {
    this.alerts
      .open('Ошибка: Эндпоинт с таким маршрутом уже существует', {
        appearance: 'negative',
      })
      .subscribe();
  }

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

  onToggleChange(newState: boolean): void {
    this.entityInfo.isActive = newState;
    this.updateEntityStatus(newState);
  }

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

  onEndpointDeleted(endpointRoute: string): void {
    this.endpoints = this.endpoints.filter(
      (endpoint) => endpoint.route !== endpointRoute
    );
    this.cd.markForCheck();
  }
}

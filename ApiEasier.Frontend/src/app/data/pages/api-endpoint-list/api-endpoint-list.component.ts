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
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
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
  entities: Entity[] = [];
  private sub: Subscription | null = null;
  loading: boolean = true;
  apiName!: string;
  apiInfo: { isActive: boolean } = { isActive: false };
  isCopied: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private apiService: ApiService,
    private apiServiceRepository: ApiServiceRepositoryService,
    private entityRepositoryService: EntityRepositoryService,
    private endpointRepositoryService: EndpointRepositoryService,
    private cd: ChangeDetectorRef,
    private alerts: TuiAlertService
  ) {}

  ngOnDestroy(): void {
    this.sub?.unsubscribe();
  }

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.apiName = params['name'];
      if (this.apiName) {
        this.loadApiStructure();
      }
    });
  }

  private loadApiStructure(): void {
    this.sub = this.apiService.getApiStructureList(this.apiName).subscribe({
      next: (apiStructure) => this.handleApiStructureResponse(apiStructure),
      error: () => {
        this.loading = false;
        this.cd.markForCheck();
      },
    });
  }

  private handleApiStructureResponse(apiStructure: ApiServiceStructure): void {
    if (apiStructure) {
      this.entities = apiStructure.entities;
      this.apiInfo.isActive = apiStructure.isActive; // Set the API state here
      this.loading = false;
      this.cd.markForCheck();
    }
  }

  copyToClipboard(entityName: string, endpoint: Endpoint): void {
    const url = this.getUrl(entityName, endpoint);
    this.copyTextToClipboard(url);
  }

  private copyTextToClipboard(text: string): void {
    const textarea = document.createElement('textarea');
    textarea.value = text;
    document.body.appendChild(textarea);
    textarea.select();
    try {
      document.execCommand('copy');
      this.showCopySuccess(text);
    } catch (err) {
      console.error('Error copying URL:', err);
    } finally {
      document.body.removeChild(textarea);
    }
  }

  private showCopySuccess(url: string): void {
    this.isCopied = url;
    this.cd.markForCheck();
    setTimeout(() => {
      this.isCopied = null;
      this.cd.markForCheck();
    }, 2000);
  }

  getUrl(entityName: string, endpoint: Endpoint): string {
    return `${window.location.origin}/api/ApiEmu/${this.apiName}/${entityName}/${endpoint.route}`;
  }

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

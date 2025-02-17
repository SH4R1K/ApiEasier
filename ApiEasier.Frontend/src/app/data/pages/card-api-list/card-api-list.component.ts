import {
  ChangeDetectorRef,
  Component,
  CUSTOM_ELEMENTS_SCHEMA,
  OnDestroy,
  OnInit,
} from '@angular/core';
import { Subscription } from 'rxjs';
import { ApiHubServiceService } from '../../../services/api-hub-service.service';
import { ApiServiceRepositoryService } from '../../../repositories/api-service-repository.service';
import { Router } from '@angular/router';
import { TuiAlertService } from '@taiga-ui/core';
import { apiServiceShortStructure } from '../../../services/service-structure-api';
import { CommonModule } from '@angular/common';
import { CardApiComponent } from '../../components/card-api/card-api.component';
import { HeaderComponent } from '../../components/header/header.component';
import { RouterModule } from '@angular/router';
import { LoadingComponent } from '../../components/loading/loading.component';
import { PaginationComponent } from '../../components/pagination/pagination.component';
import {
  TuiInputSliderModule,
  TuiTextfieldControllerModule,
} from '@taiga-ui/legacy';
import { tuiDialog } from '@taiga-ui/core';
import { ApiDialogComponent } from '../../components/api-dialog/api-dialog.component';
import { FilterByInputComponent } from '../../components/filter-by-input/filter-by-input.component';

@Component({
  selector: 'app-card-api-list',
  imports: [
    CardApiComponent,
    CommonModule,
    HeaderComponent,
    RouterModule,
    LoadingComponent,
    TuiInputSliderModule,
    TuiTextfieldControllerModule,
    PaginationComponent,
    FilterByInputComponent,
  ],
  templateUrl: './card-api-list.component.html',
  styleUrls: [
    './card-api-list.component.css',
    '../../styles/card-list.css',
    '../../styles/icon.css',
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class CardApiListComponent implements OnInit, OnDestroy {
  cards: apiServiceShortStructure[] = [];
  filteredCards: apiServiceShortStructure[] = [];
  apiNames: string[] = [];
  private sub: Subscription | null = null;
  loading = true;
  itemsPerPage = 16;
  currentPage = 1;
  searchQueryActive = false;
  isSortedAscending: boolean = true;
  errorMessage: string = '';
  errorCode: string = '';

  api: apiServiceShortStructure = {
    name: '',
    isActive: false,
    description: '',
  };

  private readonly dialog = tuiDialog(ApiDialogComponent, {
    dismissible: true,
    label: 'Создать',
  });

  constructor(
    private apiServiceRepository: ApiServiceRepositoryService,
    private changeDetector: ChangeDetectorRef,
    private router: Router,
    private readonly alerts: TuiAlertService,
    private apiServiceHub: ApiHubServiceService
  ) {}

  ngOnDestroy(): void {
    this.sub?.unsubscribe();
  }

  ngOnInit(): void {
    this.loadApiList();
    this.subscribeToApiUpdates();
  }

  private loadApiList(): void {
    this.sub = this.apiServiceRepository.getApiList().subscribe({
      next: (apiList) => {
        this.handleApiListResponse(apiList);
        this.apiServiceHub.initializeData(apiList);
        this.sortCards();
      },
      error: (error) => {
        this.errorMessage = error.message;
        this.errorCode = error.status;
      },
    });
  }

  subscribeToApiUpdates(): void {
    this.apiServiceHub.ordersUpdated$.subscribe({
      next: (updatedApiList) => {
        this.cards = updatedApiList;
        this.sortCards();
        this.filteredCards = updatedApiList;
        this.apiNames = updatedApiList.map((api) => api.name);
        this.updatePagination();
        this.changeDetector.markForCheck();
      },
      error: (error) => {
        this.navigateToErrorPage(error.status, error.message);
      },
    });
  }

  private navigateToErrorPage(errorCode: string, errorMessage: string): void {
    this.router.navigate(['/error'], {
      queryParams: { code: errorCode, message: errorMessage },
    });
  }

  private handleApiListResponse(apiList: apiServiceShortStructure[]): void {
    this.cards = apiList;
    this.filteredCards = apiList;
    this.apiNames = apiList.map((api) => api.name);
    this.updatePagination();
    this.changeDetector.detectChanges();
    this.loading = false;
  }

  openCreateDialog(): void {
    this.dialog({ ...this.api }).subscribe({
      next: (data) => this.processCreateDialogData(data),
      complete: () => this.onDialogClose(),
    });
  }

  private processCreateDialogData(data: apiServiceShortStructure): void {
    if (this.isApiNameExists(data.name)) {
      this.showApiNameExistsError();
      return;
    }
    this.createApiService(data);
  }

  private isApiNameExists(name: string): boolean {
    return this.cards.some((card) => card.name === name);
  }

  private onDialogClose(): void {
    console.info('Диалог закрыт');
  }

  private showApiNameExistsError(): void {
    this.alerts
      .open('Ошибка: API с таким именем уже существует', {
        appearance: 'negative',
      })
      .subscribe();
  }

  private createApiService(data: apiServiceShortStructure): void {
    this.apiServiceRepository.createApiService(data).subscribe({
      next: (response) => this.onApiServiceCreated(response, data),
      error: (error) => {
        this.errorMessage = error.message;
        this.errorCode = error.status;
      },
    });
  }

  private onApiServiceCreated(
    response: any,
    data: apiServiceShortStructure
  ): void {
    console.log('API добавлено:', response);
    this.cards.push(data);
    this.sortCards();
    this.changeDetector.markForCheck();
    this.alerts
      .open('API успешно создано', {
        appearance: 'success',
      })
      .subscribe();
  }

  onApiDeleted(apiName: string): void {
    this.cards = this.cards.filter((card) => card.name !== apiName);
    this.filteredCards = this.filteredCards.filter(
      (card) => card.name !== apiName
    );
    this.apiNames = this.apiNames.filter((name) => name !== apiName);
    this.updatePagination();
    this.changeDetector.markForCheck();
  }

  onSearchQuery(query: string): void {
    this.searchQueryActive = !!query;
    this.filteredCards = this.cards.filter((card) => card.name.includes(query));
    this.sortCards(); // Применяем сортировку после фильтрации
    this.updatePagination();
  }

  get totalPages(): number {
    return Math.ceil(this.filteredCards.length / this.itemsPerPage);
  }

  get paginatedCards(): apiServiceShortStructure[] {
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    return this.filteredCards.slice(startIndex, startIndex + this.itemsPerPage);
  }

  onPageChange(page: number): void {
    this.currentPage = page;
  }

  private updatePagination(): void {
    if (this.currentPage > this.totalPages) {
      this.currentPage = this.totalPages;
    }
    if (this.currentPage < 1) {
      this.currentPage = 1;
    }
  }

  sortCards(): void {
    this.filteredCards.sort((a, b) =>
      this.isSortedAscending
        ? a.name.localeCompare(b.name)
        : b.name.localeCompare(a.name)
    );
  }

  sortCardsOnClick(): void {
    this.isSortedAscending = !this.isSortedAscending; // Инвертируем флаг
    this.sortCards(); // Применяем сортировку
    this.changeDetector.markForCheck(); // Уведомляем Angular о изменениях
  }

  goBack(): void {
    this.router.navigate(['/']);
  }
}

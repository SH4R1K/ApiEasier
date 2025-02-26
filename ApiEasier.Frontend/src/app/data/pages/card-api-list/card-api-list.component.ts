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
import { TuiAlertService, tuiDialog } from '@taiga-ui/core';
import { apiServiceShortStructure, ApiServiceStructure } from '../../../services/service-structure-api';
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
import { ApiDialogComponent } from '../../components/api-dialog/api-dialog.component';
import { FilterByInputComponent } from '../../components/filter-by-input/filter-by-input.component';
import JSZip from 'jszip';

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
    '../../styles/button.css'
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
  isSortedAscending = true;
  errorMessage = '';
  errorCode = '';
  selectedApis = new Set<string>();
  isSelectionMode = false;
  isAllSelected = false;

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
    // private apiServiceHub: ApiHubServiceService
  ) {}

  private handleWindowResize() {
    this.changeDetector.detectChanges();
  }
  
  ngOnInit(): void {
    this.loadApiList();
    window.addEventListener('resize', () => this.handleWindowResize());
  }
  
  ngOnDestroy(): void {
    window.removeEventListener('resize', () => this.handleWindowResize());
    this.sub?.unsubscribe();
  }

  // Переключаем режим выбора
  toggleSelectionMode(): void {
    this.isSelectionMode = !this.isSelectionMode;
    if (!this.isSelectionMode) {
      this.selectedApis.clear();
      this.isAllSelected = false;
    }
  }

  toggleApiSelection(apiName: string): void {
    if (!this.isSelectionMode) return;
    
    if (this.selectedApis.has(apiName)) {
      this.selectedApis.delete(apiName);
    } else {
      this.selectedApis.add(apiName);
    }
    this.checkAllSelectedState();
    this.changeDetector.markForCheck(); // Добавлено
  }
  
    toggleSelectAll(): void {
      if (this.isAllSelected) {
        // Снимаем выделение со всех элементов
        this.filteredCards.forEach(item => this.selectedApis.delete(item.name));
      } else {
        // Добавляем все элементы всех страниц
        this.filteredCards.forEach(item => this.selectedApis.add(item.name));
      }
      this.isAllSelected = !this.isAllSelected;
      this.changeDetector.markForCheck();
    }

  // Проверка состояния "Выбрано все"
  private checkAllSelectedState(): void {
    this.isAllSelected = this.filteredCards.length > 0 && 
      this.filteredCards.every(item => this.selectedApis.has(item.name));
  }

  // Метод для экспорта выбранных API
  exportSelectedApis(): void {
    this.loading = true;
    if (this.selectedApis.size === 0) return;
    
    const selectedNames = Array.from(this.selectedApis);
    const exportPromises = selectedNames.map(name => 
      this.apiServiceRepository.getApiStructureList(name).toPromise()
    );
  
    Promise.all(exportPromises)
      .then(results => {
        const timestamp = new Date().toISOString().replace(/[:.]/g, '-');
        const zip = new JSZip();
        
        results.forEach((data, index) => {
          if (!data) return;
          
          const { name, ...dataWithoutName } = data;
          const fileName = `${name || `api-${index}`}.json`;
          zip.file(fileName, JSON.stringify(dataWithoutName, null, 2));
        });
  
        zip.generateAsync({ type: 'blob' })
          .then((content: Blob) => {
            const url = window.URL.createObjectURL(content);
            const a = document.createElement('a');
            a.href = url;
            a.download = `apis-export-${timestamp}.zip`;
            a.click();
            window.URL.revokeObjectURL(url);
          });
      })
      .catch((error: any) => {
        console.error('Export error:', error);
        this.alerts.open('Ошибка экспорта', { appearance: 'negative' }).subscribe();
      })
      .finally(() => {
        this.loading = false;
        this.selectedApis.clear();
        this.isSelectionMode = false;
      });
  }

  private loadApiList(): void {
    this.sub = this.apiServiceRepository.getApiList().subscribe(this.handleApiResponse());
  }

  // private subscribeToApiUpdates(): void {
  //   this.apiServiceHub.ordersUpdated$.subscribe(this.handleApiResponse());
  // }

  private handleApiResponse() {
    return {
      next: (apiList: apiServiceShortStructure[]) => {
        this.updateApiList(apiList);
      },
      error: (error: any) => {
        this.handleError(error);
      },
    };
  }

  private updateApiList(apiList: apiServiceShortStructure[]): void {
    this.cards = apiList;
    this.filteredCards = [...apiList]; // Создаем копию массива
    this.sortCards(); // Сортируем после обновления
    this.apiNames = apiList.map(api => api.name);
    this.updatePagination();
    this.changeDetector.detectChanges();
    this.loading = false;
  }

  private handleError(error: any): void {
    this.errorMessage = error.message;
    this.errorCode = error.status;
    this.navigateToErrorPage(this.errorCode, this.errorMessage);
  }

  private navigateToErrorPage(errorCode: string, errorMessage: string): void {
    this.router.navigate(['/error'], {
      queryParams: { code: errorCode, message: errorMessage },
    });
  }

  openCreateDialog(event?: Event): void {
    if (event) {
      event.preventDefault();
    }
    this.dialog({ ...this.api }).subscribe(this.processDialogData());
  }

  private processDialogData() {
    return {
      next: (data: apiServiceShortStructure) => {
        if (this.isApiNameExists(data.name)) {
          this.showApiNameExistsError();
        } else {
          this.createApiService(data);
        }
      },
      complete: () => this.onDialogClose(),
    };
  }

  private isApiNameExists(name: string): boolean {
    return this.cards.some(card => card.name === name);
  }

  private onDialogClose(): void {
    console.info('Dialog closed');
  }

  private showApiNameExistsError(): void {
    this.alerts.open('Ошибка: API с таким именем уже существует', {
      appearance: 'negative',
    }).subscribe();
  }

  private createApiService(data: apiServiceShortStructure): void {
    this.apiServiceRepository.createApiService(data).subscribe({
      next: (response) => this.onApiServiceCreated(response, data),
      error: (error) => this.handleError(error),
    });
  }

  private onApiServiceCreated(response: any, data: apiServiceShortStructure): void {
    this.cards.push(data);
    this.sortCards();
    this.changeDetector.markForCheck();
    this.alerts.open('API успешно создано', {
      appearance: 'success',
    }).subscribe();
  }

  onApiDeleted(apiName: string): void {
    this.selectedApis.delete(apiName);
    this.removeApiByName(apiName);
    this.updatePagination();
    this.changeDetector.markForCheck();
  }

  private removeApiByName(apiName: string): void {
    this.cards = this.cards.filter(card => card.name !== apiName);
    this.filteredCards = this.filteredCards.filter(card => card.name !== apiName);
    this.apiNames = this.apiNames.filter(name => name !== apiName);
  }

  onSearchQuery(query: string): void {
    this.searchQueryActive = !!query;
    this.filteredCards = this.cards.filter(card => 
      card.name.toLowerCase().includes(query.toLowerCase())
    );
    this.currentPage = 1;
    this.sortCards();
    this.updatePagination();
    this.changeDetector.markForCheck();
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
    this.checkAllSelectedState(); // Обновляем состояние после смены страницы
    this.changeDetector.markForCheck();
  }

  private updatePagination(): void {
    const newCurrentPage = Math.max(1, Math.min(this.currentPage, this.totalPages));
    if (newCurrentPage !== this.currentPage) {
      this.currentPage = newCurrentPage;
      this.changeDetector.markForCheck();
    }
  }

  sortCards(): void {
    this.filteredCards.sort((a, b) => 
      this.isSortedAscending 
        ? a.name.localeCompare(b.name) 
        : b.name.localeCompare(a.name)
    );
  }
  
  toggleSort(): void {
    this.isSortedAscending = !this.isSortedAscending;
    this.sortCards();
    this.updatePagination();
    this.changeDetector.markForCheck();
  }

  goBack(): void {
    this.router.navigate(['/']);
  }
}

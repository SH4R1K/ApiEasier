import { NgIf, NgFor } from '@angular/common';
import { ChangeDetectionStrategy, Component, ChangeDetectorRef } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { TuiIcon, TuiLink, TuiButton, TuiAlertService } from '@taiga-ui/core';
import { TuiAvatar, TuiFiles } from '@taiga-ui/kit';
import { ApiServiceStructure } from "../../../interfaces/ApiServiceStructure";
import { ApiServiceRepositoryService } from '../../../repositories/api-service-repository.service';
import { TuiDialogContext } from '@taiga-ui/core';
import { injectContext } from '@taiga-ui/polymorpheus';
import { FileStatus } from '../../../interfaces/FileStatus';

/**
 * Компонент ImportDialogComponent предназначен для импорта данных API через диалоговое окно.
 * Позволяет пользователю загружать файлы и обрабатывать их содержимое для создания новых API сервисов.
 *
 * @remarks
 * Этот компонент интегрируется с Taiga UI для создания интерактивного интерфейса.
 * Использует реактивные формы для управления загрузкой файлов и обработки данных.
 *
 * @example
 * html
 * <app-import-dialog></app-import-dialog>
 */
@Component({
  selector: 'app-import-dialog',
  imports: [NgIf, ReactiveFormsModule, TuiAvatar, TuiFiles, TuiLink, NgFor, TuiButton],
  templateUrl: './import-dialog.component.html',
  styleUrls: ['./import-dialog.component.css'], // Исправлено с styleUrl на styleUrls
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ImportDialogComponent {
  /**
   * Контрол для управления загрузкой файлов.
   *
   * @type {FormControl<File[]>}
   * @memberof ImportDialogComponent
   */
  protected readonly control = new FormControl<File[]>([]);

  /**
   * Список загруженных файлов с их статусами.
   *
   * @type {FileStatus[]}
   * @memberof ImportDialogComponent
   */
  protected files: FileStatus[] = [];

  /**
   * Флаг, указывающий, есть ли загруженные файлы.
   *
   * @type {boolean}
   * @default false
   * @memberof ImportDialogComponent
   */
  protected hasFiles = false;

  /**
   * Массив для хранения обработанных данных API.
   *
   * @type {ApiServiceStructure[]}
   * @memberof ImportDialogComponent
   */
  protected processedData: ApiServiceStructure[] = [];

  /**
   * Контекст диалогового окна.
   *
   * @type {TuiDialogContext<boolean>}
   * @memberof ImportDialogComponent
   */
  private readonly context = injectContext<TuiDialogContext<boolean>>();

  /**
   * Конструктор компонента ImportDialogComponent.
   *
   * @param cd - Сервис для управления изменением состояния.
   * @param apiService - Сервис для взаимодействия с репозиторием API.
   * @param alerts - Сервис для отображения уведомлений.
   *
   * @memberof ImportDialogComponent
   */
  constructor(
    private cd: ChangeDetectorRef,
    private apiService: ApiServiceRepositoryService,
    private readonly alerts: TuiAlertService
  ) {
    this.control.valueChanges.subscribe((files) => {
      if (files) {
        // Обновляем только новые файлы
        const newFiles = files.filter(file => !this.files.some(f => f.file.name === file.name));
        this.files = [
          ...this.files,
          ...newFiles.map(file => ({ file, status: 'loading' as const, errorMessage: '' }))
        ];
        this.hasFiles = this.files.filter(f => f.status == 'normal').length > 0;
        this.readFiles(newFiles);
      }
    });
  }

  /**
   * Читает содержимое загруженных файлов.
   *
   * @param files - Список файлов для чтения.
   * @private
   * @memberof ImportDialogComponent
   */
  private readFiles(files: File[]): void {
    files.forEach(file => {
      const reader = new FileReader();
      reader.onload = (e: ProgressEvent<FileReader>) => {
        const text = e.target?.result as string;
        try {
          const json = JSON.parse(text);
          this.updateFileStatus(file, 'normal');
          this.processJson(json, file.name);
        } catch (error) {
          console.error('Ошибка при чтении JSON файла:', error);
          this.updateFileStatus(file, 'error');
        }
      };
      reader.readAsText(file);
    });
  }

  /**
   * Обновляет статус файла.
   *
   * @param file - Файл, статус которого нужно обновить.
   * @param status - Новый статус файла.
   * @private
   * @memberof ImportDialogComponent
   */
  private updateFileStatus(file: File, status: 'loading' | 'normal' | 'error'): void {
    const fileStatus = this.files.find(f => f.file.name === file.name);
    if (fileStatus) {
      fileStatus.status = status;
      this.hasFiles = this.files.filter(f => f.status == 'normal').length > 0;
      this.cd.markForCheck();
    }
  }

  /**
   * Возвращает текстовое описание статуса файла.
   *
   * @param file - Файл, для которого нужно получить статус.
   * @returns {string} Текстовое описание статуса файла.
   * @memberof ImportDialogComponent
   */
  protected getFileStatusText(file: FileStatus): string {
    switch (file.status) {
      case 'loading':
        return 'Файл на проверке';
      case 'normal':
        return 'Файл проверен';
      case 'error':
        return 'Ошибка загрузки файла';
      case 'success':
        return 'Файл загружен';
      default:
        return '';
    }
  }

  /**
   * Возвращает текущий статус файла.
   *
   * @param file - Файл, для которого нужно получить статус.
   * @returns {'loading' | 'normal' | 'error'} Статус файла.
   * @memberof ImportDialogComponent
   */
  protected getFileStatus(file: FileStatus): 'loading' | 'normal' | 'error' {
    switch (file.status) {
      case 'loading':
        return 'loading';
      case 'normal':
        return 'normal';
      case 'error':
        return 'error';
      case 'success':
        return 'normal';
    }
  }

  /**
   * Обрабатывает JSON данные из файла.
   *
   * @param json - JSON данные для обработки.
   * @param fileName - Имя файла.
   * @private
   * @memberof ImportDialogComponent
   */
  private processJson(json: any, fileName: string): void {
    const name = fileName.replace(/\.[^/.]+$/, "");
    const apiServiceStructure: ApiServiceStructure = {
      name: name,
      isActive: json.isActive || false,
      description: json.description || '',
      entities: json.entities || []
    };
    this.processedData.push(apiServiceStructure);
    console.log('Обработанный JSON:', apiServiceStructure);
  }

  /**
   * Отправляет обработанные данные на сервер.
   *
   * @remarks
   * Обновляет статус файлов и отображает уведомления о результатах отправки.
   *
   * @memberof ImportDialogComponent
   */
  protected submit(): void {
    if (this.processedData.length > 0) {
      this.files.filter(f => f.status == 'normal').forEach(file => file.status = 'loading');
      this.cd.markForCheck();

      this.processedData.forEach(service => {
        this.apiService.createFullApiService(service).subscribe({
          next: (response) => {
            console.log('Сервис успешно создан:', response);
            const file = this.files.find(f => f.file.name === service.name + '.json');
            if (file) {
              file.status = 'success';
              file.errorMessage = '';
              this.cd.markForCheck();
            }
            this.hasFiles = this.files.filter(f => f.status == 'normal').length > 0;
            if (this.files.every(file => file.status == 'success')) {
              this.alerts
                .open('Файлы успешно загружены', {
                  appearance: 'success',
                })
                .subscribe();
              this.context.completeWith(false);
            }
          },
          error: (response) => {
            const file = this.files.find(f => f.file.name === service.name + '.json');
            if (file) {
              file.status = 'error';
              file.errorMessage = `Ошибка при создании сервиса: ${response.error}`;
              this.cd.markForCheck();
            }
            this.hasFiles = this.files.filter(f => f.status == 'normal').length > 0;
          }
        });
      });
    } else {
      console.warn('Нет данных для отправки.');
    }
  }

  /**
   * Удаляет файл из списка загруженных.
   *
   * @param fileToRemove - Файл, который нужно удалить.
   * @memberof ImportDialogComponent
   */
  protected removeFile(fileToRemove: FileStatus): void {
    this.files = this.files.filter(file => file !== fileToRemove);
    this.hasFiles = this.files.filter(f => f.status == 'normal').length > 0;

    const currentFiles = this.control.value ? this.control.value.filter(file => file !== fileToRemove.file) : [];
    this.control.setValue(currentFiles);

    this.cd.markForCheck();
  }
}

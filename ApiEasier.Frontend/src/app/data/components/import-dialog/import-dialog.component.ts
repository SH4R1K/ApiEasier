import { NgIf, NgFor } from '@angular/common';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { TuiIcon, TuiLink } from '@taiga-ui/core';
import type { TuiFileLike } from '@taiga-ui/kit';
import { TuiAvatar, TuiFiles } from '@taiga-ui/kit';
import { ChangeDetectorRef } from '@angular/core';
import { TuiButton } from '@taiga-ui/core';
import { ApiServiceStructure } from '../../../services/service-structure-api';
import { ApiServiceRepositoryService } from '../../../repositories/api-service-repository.service';
import { TuiDialogContext, TuiAlertService } from '@taiga-ui/core';
import { injectContext } from '@taiga-ui/polymorpheus';

interface FileStatus {
  file: TuiFileLike;
  status: 'loading' | 'normal' | 'error' | 'success';
  errorMessage: string;
}

@Component({
  selector: 'app-import-dialog',
  imports: [NgIf, ReactiveFormsModule, TuiAvatar, TuiFiles, TuiIcon, TuiLink, NgFor, TuiButton],
  templateUrl: './import-dialog.component.html',
  styleUrl: './import-dialog.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ImportDialogComponent {
  protected readonly control = new FormControl<File[]>([]);
  protected files: FileStatus[] = [];
  protected hasFiles = false;
  protected processedData: ApiServiceStructure[] = []; // Массив для хранения обработанных данных
  private readonly context = injectContext<TuiDialogContext<boolean>>();


  constructor(private cdr: ChangeDetectorRef, private apiService: ApiServiceRepositoryService, private readonly alerts: TuiAlertService) {
    this.control.valueChanges.subscribe((files) => {
      if (files) {
        // Обновляем только новые файлы
        const newFiles = files.filter(file => !this.files.some(f => f.file.name === file.name));
        this.files = [
          ...this.files,
          ...newFiles.map(file => ({ file, status: 'loading' as const, errorMessage: '' })) // Используем 'as const' для явного указания типа
        ];
        this.hasFiles = this.files.filter(f => f.status == 'normal').length > 0;
        this.readFiles(newFiles);
      }
    });
  }

  private readFiles(files: File[]): void {
    files.forEach(file => {
      const reader = new FileReader();
      reader.onload = (e: ProgressEvent<FileReader>) => {
        const text = e.target?.result as string;
        try {
          const json = JSON.parse(text);
          this.updateFileStatus(file, 'normal');
          this.processJson(json, file.name); // Передаем имя файла в processJson
        } catch (error) {
          console.error('Ошибка при чтении JSON файла:', error);
          this.updateFileStatus(file, 'error');
        }
      };
      reader.readAsText(file);
    });
  }

  private updateFileStatus(file: File, status: 'loading' | 'normal' | 'error'): void {
    const fileStatus = this.files.find(f => f.file.name === file.name);
    if (fileStatus) {
      fileStatus.status = status; // Тип уже корректен
      this.hasFiles = this.files.filter(f => f.status == 'normal').length > 0;
      this.cdr.markForCheck();
    }
  }

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

  private processJson(json: any, fileName: string): void {
    const name = fileName.replace(/\.[^/.]+$/, ""); // Убираем расширение файла
    const apiServiceStructure: ApiServiceStructure = {
      name: name,
      isActive: json.isActive || false, // Пример обработки других полей
      description: json.description || '',
      entities: json.entities || []
    };
    this.processedData.push(apiServiceStructure); // Сохраняем обработанные данные
    console.log('Обработанный JSON:', apiServiceStructure);
  }

  protected submit(): void {
    if (this.processedData.length > 0) {
      this.files.filter(f => f.status == 'normal').forEach(file => file.status = 'loading');
      this.cdr.markForCheck();

      // Отправляем каждый объект на сервер
      this.processedData.forEach(service => {
        this.apiService.createFullApiService(service).subscribe({
          next: (response) => {
            console.log('Сервис успешно создан:', response);
            // Обновляем статус файла на "success" (успешно загружен)
            const index = this.processedData.findIndex(d => d.name === service.name);
            if (index !== -1) {
              this.processedData.splice(index, 1);
            }
            const file = this.files.find(f => f.file.name === service.name + '.json');
            if (file) {
              file.status = 'success'; // Используем 'success' для обозначения успеха
              file.errorMessage = '';
              this.cdr.markForCheck(); // Обновляем представление
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
            // Обновляем статус файла и сохраняем ошибку
            const file = this.files.find(f => f.file.name === service.name + '.json');
            if (file) {
              file.status = 'error';
              file.errorMessage = `Ошибка при создании сервиса: ${response.error}`;
              const index = this.processedData.findIndex(d => d.name === service.name);
              if (index !== -1) {
                this.processedData.splice(index, 1);
              }
              this.cdr.markForCheck(); // Обновляем представление
            }
            this.hasFiles = this.files.filter(f => f.status == 'normal').length > 0;
          }
        });
      });
    } else {
      console.warn('Нет данных для отправки.');
    }
  }

  protected removeFile(fileToRemove: FileStatus): void {
    this.files = this.files.filter(file => file !== fileToRemove);
    this.hasFiles = this.files.filter(f => f.status == 'normal').length > 0;

    // Обновляем значение FormControl, чтобы оно соответствовало текущему списку файлов
    const currentFiles = this.control.value ? this.control.value.filter(file => file !== fileToRemove.file) : [];
    this.control.setValue(currentFiles);

    this.cdr.markForCheck(); // Обновляем представление
  }
}
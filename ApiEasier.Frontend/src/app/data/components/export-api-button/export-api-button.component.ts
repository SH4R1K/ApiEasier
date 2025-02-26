import { Component, Input } from '@angular/core';
import { TuiButton } from '@taiga-ui/core';
import { ApiServiceStructure } from "../../../interfaces/ApiServiceStructure";
import { apiServiceShortStructure } from "../../../interfaces/apiServiceShortStructure";
import { ApiServiceRepositoryService } from '../../../repositories/api-service-repository.service';

/**
 * Компонент ExportApiButtonComponent предназначен для экспорта структуры API в формате JSON.
 * Позволяет пользователю скачать структуру API в виде файла.
 *
 * @remarks
 * Этот компонент интегрируется с сервисом репозитория API для получения структуры API.
 * Использует Taiga UI для создания интерактивного интерфейса.
 *
 * @example
 * html
 * <app-export-api-button [apiInfo]="apiData"></app-export-api-button>
 */
@Component({
  selector: 'app-export-api-button',
  imports: [TuiButton],
  templateUrl: './export-api-button.component.html',
  styleUrls: ['./export-api-button.component.css', '../../styles/button.css'],
})
export class ExportApiButtonComponent {
  /**
   * Входной параметр для получения информации о API.
   *
   * @type {apiServiceShortStructure}
   * @memberof ExportApiButtonComponent
   */
  @Input() apiInfo!: apiServiceShortStructure;

  /**
   * Конструктор компонента ExportApiButtonComponent.
   *
   * @param apiServiceRepository - Сервис для взаимодействия с репозиторием API.
   *
   * @memberof ExportApiButtonComponent
   */
  constructor(private apiServiceRepository: ApiServiceRepositoryService) {}

  /**
   * Обработчик нажатия на кнопку экспорта.
   *
   * @remarks
   * Получает структуру API из репозитория и инициирует скачивание файла JSON.
   *
   * @memberof ExportApiButtonComponent
   */
  onClick(): void {
    this.apiServiceRepository.getApiStructureList(this.apiInfo.name).subscribe({
      next: (data: ApiServiceStructure) => {
        const { name, ...dataWithoutName } = data;
        const jsonString = JSON.stringify(dataWithoutName, null, 2);

        const blob = new Blob([jsonString], { type: 'application/json' });
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `${data.name}.json`;
        a.click();
        window.URL.revokeObjectURL(url);
      },
      error: (error) => {
        console.log(error);
      },
    });
  }
}

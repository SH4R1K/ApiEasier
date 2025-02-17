import { Component, Input } from '@angular/core';
import { TuiButton } from '@taiga-ui/core';
import { apiServiceShortStructure, ApiServiceStructure } from '../../../services/service-structure-api';
import { ApiServiceRepositoryService } from '../../../repositories/api-service-repository.service';

@Component({
  selector: 'app-export-api-button',
  imports: [
    TuiButton
  ],
  templateUrl: './export-api-button.component.html',
  styleUrls: ['./export-api-button.component.css', '../../styles/button.css']
})
export class ExportApiButtonComponent {
  @Input() apiInfo!: apiServiceShortStructure;

  constructor(
    private apiServiceRepository: ApiServiceRepositoryService,
  ) { }

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
        console.log(error)
      }
    });
  }
}

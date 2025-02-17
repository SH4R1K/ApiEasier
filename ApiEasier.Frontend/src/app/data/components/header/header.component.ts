import { Component, EventEmitter, Input, Output } from '@angular/core';
import { BackButtonComponent } from '../back-button/back-button.component';
import { CommonModule } from '@angular/common';
import { tuiDialog } from '@taiga-ui/core';
import { ImportDialogComponent } from '../import-dialog/import-dialog.component';

@Component({
  selector: 'app-header',
  imports: [BackButtonComponent,CommonModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css',
})
export class HeaderComponent {
  @Input() logoUrl: string = "https://www.titan2.ru/images/temp/logo__ru.jpg"; // URL логотипа
  @Input() buttonText: string = ''; // Текст кнопки
  @Output() buttonClick: EventEmitter<void> = new EventEmitter<void>(); 
  @Input() isApiPage: boolean = false;

  private readonly dialog = tuiDialog(ImportDialogComponent, {
    closeable: true,
      dismissible: true,
      label: "Импортировать",
    });

  Click(): void {
    this.buttonClick.emit(); // Emit the toggle event
  }
  
  Import(): void {
    this.dialog().subscribe();
  }
}
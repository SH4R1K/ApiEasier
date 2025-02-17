import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { TuiDataListWrapper, TuiFilterByInputPipe } from '@taiga-ui/kit';
import { TuiInputModule } from '@taiga-ui/legacy';

@Component({
  selector: 'app-filter-by-input',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    TuiDataListWrapper,
    TuiFilterByInputPipe,
    TuiInputModule,
  ],
  templateUrl: './filter-by-input.component.html',
  styleUrls: ['./filter-by-input.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FilterByInputComponent {
  @Input() label: string = 'Search';
  @Input() controlName: string = 'search';
  @Input() items: string[] = [];
  @Output() searchQuery = new EventEmitter<string>();

  form = new FormGroup({
    [this.controlName]: new FormControl('', [Validators.pattern('^[a-zA-Z0-9]*$')]),
  });

  get filteredItems() {
    const control = this.form.get(this.controlName);
    if (control) {
      const value = control.value ?? '';
      this.searchQuery.emit(value);
      return this.items.filter(item => item.includes(value));
    }
    return this.items;
  }

  constructor() {
    this.form.controls[this.controlName].valueChanges.subscribe(value => {
      this.searchQuery.emit(value ?? '');
    });
  }

  onKeyPress(event: KeyboardEvent): void {
    const inputChar = event.key;
    const allowedChars = /^[a-zA-Z0-9]$/;
    if (!allowedChars.test(inputChar) && !event.ctrlKey && !event.metaKey) {
      event.preventDefault();
    }
  }
}

import { CommonModule } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  Output,
  OnInit,
} from '@angular/core';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TuiSwitch, tuiSwitchOptionsProvider } from '@taiga-ui/kit';

@Component({
  selector: 'app-switch',
  imports: [CommonModule, FormsModule, ReactiveFormsModule, TuiSwitch],
  templateUrl: './switch.component.html',
  styleUrls: ['./switch.component.css'],
  providers: [
    tuiSwitchOptionsProvider({ showIcons: false, appearance: () => 'primary' }),
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SwitchComponent implements OnInit {
  @Input() value: boolean = false;
  @Output() toggle: EventEmitter<boolean> = new EventEmitter<boolean>();

  protected readonly invalidTrue = new FormControl(true, () => ({
    invalid: true,
  }));
  protected readonly invalidFalse = new FormControl(false, () => ({
    invalid: true,
  }));

  public ngOnInit(): void {
    this.invalidTrue.markAsTouched();
    this.invalidFalse.markAsTouched();
  }

  onToggle(): void {
    this.toggle.emit(this.value);
  }
}
